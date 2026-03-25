namespace AuthHexagonalApi.Domain.Ports;

public sealed record PasswordResetResult(bool Succeeded, IReadOnlyList<string> Errors)
{
    public static PasswordResetResult Success() => new(true, Array.Empty<string>());
    public static PasswordResetResult Fail(params string[] errors) => new(false, errors);
}

public interface IPasswordResetService
{
    Task<string?> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default);
    Task<PasswordResetResult> ResetPasswordAsync(
        string email,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default);

    Task InvalidateRefreshTokenAsync(string email, CancellationToken cancellationToken = default);
}

