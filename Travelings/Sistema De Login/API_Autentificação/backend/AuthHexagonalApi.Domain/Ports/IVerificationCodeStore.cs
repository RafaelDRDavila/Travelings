namespace AuthHexagonalApi.Domain.Ports;

public sealed record PasswordRecoveryTicketPayload(
    string Email,
    string PasswordResetToken);

public interface IVerificationCodeStore
{
    Task StoreOtpAsync(
        string email,
        string otpCode,
        string passwordResetToken,
        DateTimeOffset expiresAtUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna e remove (uso único) o token associado ao código.
    /// </summary>
    Task<string?> ConsumeOtpAsync(
        string email,
        string otpCode,
        CancellationToken cancellationToken = default);

    Task StoreTicketAsync(
        string ticket,
        PasswordRecoveryTicketPayload payload,
        DateTimeOffset expiresAtUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna e remove (uso único) o payload associado ao ticket.
    /// </summary>
    Task<PasswordRecoveryTicketPayload?> ConsumeTicketAsync(
        string ticket,
        CancellationToken cancellationToken = default);
}

