using System.Net;
using System.Text.Encodings.Web;
using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Application.DTOs.Common;
using AuthHexagonalApi.Domain.Exceptions;
using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Application.UseCases.Auth;

public interface IForgotPasswordUseCase
{
    Task<MessageResponse> HandleAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
}

public sealed class ForgotPasswordUseCase : IForgotPasswordUseCase
{
    private readonly IPasswordResetService _passwordResetService;
    private readonly IEmailSender _emailSender;
    private readonly string _frontendBaseUrl;

    public ForgotPasswordUseCase(
        IPasswordResetService passwordResetService,
        IEmailSender emailSender,
        string frontendBaseUrl)
    {
        _passwordResetService = passwordResetService;
        _emailSender = emailSender;
        _frontendBaseUrl = frontendBaseUrl;
    }

    public async Task<MessageResponse> HandleAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ValidationException(
                "Dados inválidos.",
                new Dictionary<string, string[]> { ["email"] = new[] { "Email é obrigatório." } },
                "validation_error");
        }

        var email = request.Email.Trim();

        // Se o e-mail não existir, retornamos a mesma mensagem (anti-enumeração).
        var token = await _passwordResetService.GeneratePasswordResetTokenAsync(email, cancellationToken);
        if (!string.IsNullOrEmpty(token))
        {
            var encodedEmail = WebUtility.UrlEncode(email);
            var encodedToken = WebUtility.UrlEncode(token);
            var resetUrl = $"{_frontendBaseUrl.TrimEnd('/')}/reset-password?email={encodedEmail}&token={encodedToken}";

            var subject = "Redefinição de senha";
            var html = $"""
                       <p>Você solicitou redefinição de senha.</p>
                       <p>Use o link abaixo para criar uma nova senha:</p>
                       <p><a href="{HtmlEncoder.Default.Encode(resetUrl)}">Redefinir senha</a></p>
                       <p>Se você não solicitou isso, ignore este e-mail.</p>
                       """;

            await _emailSender.SendAsync(email, subject, html, cancellationToken);
        }

        return new MessageResponse("Se o e-mail existir, enviaremos instruções para redefinir a senha.");
    }
}

