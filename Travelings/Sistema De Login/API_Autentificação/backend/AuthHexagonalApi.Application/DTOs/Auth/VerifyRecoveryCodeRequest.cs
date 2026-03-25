namespace AuthHexagonalApi.Application.DTOs.Auth;

public sealed record VerifyRecoveryCodeRequest(
    string Email,
    string Code
);

