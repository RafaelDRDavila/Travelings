using AuthHexagonalApi.Domain.Entities;

namespace AuthHexagonalApi.Domain.Ports;

public sealed record TokenResult(
    string AccessToken,
    DateTime AccessTokenExpiresAtUtc,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc
);

public interface IJwtTokenService
{
    TokenResult GenerateTokens(User user, DateTime nowUtc);
}

