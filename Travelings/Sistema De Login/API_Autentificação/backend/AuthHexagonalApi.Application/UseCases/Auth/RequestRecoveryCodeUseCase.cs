using System.Security.Cryptography;
using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Application.DTOs.Common;
using AuthHexagonalApi.Domain.Exceptions;
using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Application.UseCases.Auth;

public interface IRequestRecoveryCodeUseCase
{
    Task<MessageResponse> HandleAsync(RequestRecoveryCodeRequest request, CancellationToken cancellationToken = default);
}

public sealed class RequestRecoveryCodeUseCase : IRequestRecoveryCodeUseCase
{
    private readonly IPasswordResetService _passwordResetService;
    private readonly IVerificationCodeStore _verificationCodeStore;
    private readonly IEmailSender _emailSender;
    private readonly int _otpLength;
    private readonly TimeSpan _otpTtl;

    public RequestRecoveryCodeUseCase(
        IPasswordResetService passwordResetService,
        IVerificationCodeStore verificationCodeStore,
        IEmailSender emailSender,
        int otpLength,
        TimeSpan otpTtl)
    {
        _passwordResetService = passwordResetService;
        _verificationCodeStore = verificationCodeStore;
        _emailSender = emailSender;
        _otpLength = otpLength;
        _otpTtl = otpTtl;
    }

    public async Task<MessageResponse> HandleAsync(
        RequestRecoveryCodeRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ValidationException(
                "Dados inválidos.",
                new Dictionary<string, string[]> { ["email"] = new[] { "Email é obrigatório." } },
                "validation_error");
        }

        var email = request.Email.Trim();

        // Anti-enumeração: se não existir, retornamos a mesma mensagem.
        var token = await _passwordResetService.GeneratePasswordResetTokenAsync(email, cancellationToken);
        if (!string.IsNullOrEmpty(token))
        {
            var otp = GenerateOtp(_otpLength);
            var expiresAt = DateTimeOffset.UtcNow.Add(_otpTtl);

            await _verificationCodeStore.StoreOtpAsync(email, otp, token, expiresAt, cancellationToken);

            var subject = "Código de verificação";
            var html = $"""
                       <p>Seu código de verificação é:</p>
                       <p style="font-size: 24px; letter-spacing: 4px;"><strong>{otp}</strong></p>
                       <p>Este código expira em {(int)_otpTtl.TotalMinutes} minutos.</p>
                       <p>Se você não solicitou isso, ignore este e-mail.</p>
                       """;

            await _emailSender.SendAsync(email, subject, html, cancellationToken);
        }

        return new MessageResponse("Se o e-mail existir, enviaremos um código de verificação.");
    }

    private static string GenerateOtp(int length)
    {
        if (length < 4 || length > 10)
            throw new DomainException("Tamanho de código inválido.", "invalid_otp_length");

        Span<byte> bytes = stackalloc byte[length];
        RandomNumberGenerator.Fill(bytes);

        Span<char> chars = stackalloc char[length];
        for (var i = 0; i < length; i++)
        {
            chars[i] = (char)('0' + (bytes[i] % 10));
        }

        return new string(chars);
    }
}

