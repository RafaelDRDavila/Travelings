using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Domain.Entities;
using AuthHexagonalApi.Domain.Exceptions;
using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Application.UseCases.Auth;

public interface ILoginUserUseCase
{
    Task<AuthResponse> HandleAsync(LoginRequest request, CancellationToken cancellationToken = default);
}

public sealed class LoginUserUseCase : ILoginUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LoginUserUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IRefreshTokenGenerator refreshTokenGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _refreshTokenGenerator = refreshTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<AuthResponse> HandleAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new DomainException("Credenciais inválidas.", "invalid_credentials");
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null)
        {
            throw new DomainException("Credenciais inválidas.", "invalid_credentials");
        }

        var passwordValid = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (!passwordValid)
        {
            throw new DomainException("Credenciais inválidas.", "invalid_credentials");
        }

        var now = _dateTimeProvider.UtcNow;
        var tokens = _jwtTokenService.GenerateTokens(user, now);
        var refreshTokenHash = _refreshTokenGenerator.HashToken(tokens.RefreshToken);
        user.SetRefreshToken(refreshTokenHash, tokens.RefreshTokenExpiresAtUtc);
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

