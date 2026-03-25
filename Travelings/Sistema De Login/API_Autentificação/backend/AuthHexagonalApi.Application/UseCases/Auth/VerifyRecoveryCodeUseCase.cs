using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Domain.Exceptions;
using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Application.UseCases.Auth;

public interface IVerifyRecoveryCodeUseCase
{
    Task<VerifyRecoveryCodeResponse> HandleAsync(VerifyRecoveryCodeRequest request, CancellationToken cancellationToken = default);
}

public sealed class VerifyRecoveryCodeUseCase : IVerifyRecoveryCodeUseCase
{
    private readonly IVerificationCodeStore _verificationCodeStore;
    private readonly TimeSpan _ticketTtl;

    public VerifyRecoveryCodeUseCase(
        IVerificationCodeStore verificationCodeStore,
        TimeSpan ticketTtl)
    {
        _verificationCodeStore = verificationCodeStore;
        _ticketTtl = ticketTtl;
    }

    public async Task<VerifyRecoveryCodeResponse> HandleAsync(
        VerifyRecoveryCodeRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Email))
            errors["email"] = new[] { "Email é obrigatório." };

        if (string.IsNullOrWhiteSpace(request.Code))
            errors["code"] = new[] { "Código é obrigatório." };

        if (errors.Count > 0)
            throw new ValidationException("Dados inválidos.", errors, "validation_error");

        var email = request.Email.Trim();
        var code = request.Code.Trim();

        var token = await _verificationCodeStore.ConsumeOtpAsync(email, code, cancellationToken);
        if (string.IsNullOrEmpty(token))
        {
            throw new ValidationException(
                "Código inválido ou expirado.",
                new Dictionary<string, string[]> { ["code"] = new[] { "Código inválido ou expirado." } },
                "invalid_recovery_code");
        }

        var ticket = Guid.NewGuid().ToString("N");
        var expiresAt = DateTimeOffset.UtcNow.Add(_ticketTtl);

        await _verificationCodeStore.StoreTicketAsync(
            ticket,
            new PasswordRecoveryTicketPayload(email, token),
            expiresAt,
            cancellationToken);

        return new VerifyRecoveryCodeResponse(ticket);
    }
}

