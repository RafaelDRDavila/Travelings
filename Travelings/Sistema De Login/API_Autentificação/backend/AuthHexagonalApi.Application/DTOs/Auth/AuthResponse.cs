namespace AuthHexagonalApi.Application.DTOs.Auth;

public sealed record AuthResponse(
    Guid UserId,
    string Email,
    string AccessToken,
    DateTime AccessTokenExpiresAtUtc,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc
);

