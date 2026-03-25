using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Domain.Exceptions;
using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Application.UseCases.Auth;

public interface IRefreshTokenUseCase
{
    Task<AuthResponse> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}

public sealed class RefreshTokenUseCase : IRefreshTokenUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RefreshTokenUseCase(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IRefreshTokenGenerator refreshTokenGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _refreshTokenGenerator = refreshTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<AuthResponse> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new DomainException("Refresh token é obrigatório.", "invalid_refresh_token");
        }

        var refreshTokenHash = _refreshTokenGenerator.HashToken(request.RefreshToken);
        var user = await _userRepository.GetByRefreshTokenHashAsync(refreshTokenHash, cancellationToken);

        if (user is null)
        {
            throw new DomainException("Refresh token inválido ou expirado.", "invalid_refresh_token");
        }

        var now = _dateTimeProvider.UtcNow;
        if (!user.HasValidRefreshToken(refreshTokenHash, now))
        {
            throw new DomainException("Refresh token inválido ou expirado.", "invalid_refresh_token");
        }

        var tokens = _jwtTokenService.GenerateTokens(user, now);
        var newRefreshTokenHash = _refreshTokenGenerator.HashToken(tokens.RefreshToken);
        user.SetRefreshToken(newRefreshTokenHash, tokens.RefreshTokenExpiresAtUtc);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return new AuthResponse(
            user.Id,
            user.Email,
            tokens.AccessToken,
            tokens.AccessTokenExpiresAtUtc,
            tokens.RefreshToken,
            tokens.RefreshTokenExpiresAtUtc);
    }
}
