using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthHexagonalApi.Domain.Entities;
using AuthHexagonalApi.Domain.Ports;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthHexagonalApi.Infrastructure.Security;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public JwtTokenService(
        IOptions<JwtSettings> settings,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _settings = settings.Value;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public TokenResult GenerateTokens(User user, DateTime nowUtc)
    {
        var accessTokenExpires = nowUtc.AddMinutes(_settings.AccessTokenExpirationMinutes);
        var refreshTokenExpires = nowUtc.AddDays(_settings.RefreshTokenExpirationDays);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: accessTokenExpires,
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = _refreshTokenGenerator.GenerateToken();

        return new TokenResult(
            accessToken,
            accessTokenExpires,
            refreshToken,
            refreshTokenExpires);
    }
}
