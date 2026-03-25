using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Domain.Entities;
using AuthHexagonalApi.Domain.Exceptions;
using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Application.UseCases.Auth;

public interface IRegisterUserUseCase
{
    Task<AuthResponse> HandleAsync(RegisterRequest request, CancellationToken cancellationToken = default);
}

public sealed class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RegisterUserUseCase(
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

    public async Task<AuthResponse> HandleAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.UserName))
            errors["userName"] = new[] { "Username é obrigatório." };

        if (string.IsNullOrWhiteSpace(request.Email))
            errors["email"] = new[] { "Email é obrigatório." };

        if (string.IsNullOrWhiteSpace(request.Password))
            errors["password"] = new[] { "Senha é obrigatória." };

        if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
            errors["confirmPassword"] = new[] { "As senhas não conferem." };

        if (errors.Count > 0)
            throw new ValidationException("Dados inválidos para registro.", errors, "validation_error");

        var userName = request.UserName.Trim();
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        if (await _userRepository.UserNameExistsAsync(userName, cancellationToken))
        {
            throw new DomainException("Já existe um usuário com este username.", "username_already_exists");
        }

        if (await _userRepository.EmailExistsAsync(normalizedEmail, cancellationToken))
        {
            throw new DomainException("Já existe um usuário com este e-mail.", "email_already_exists");
        }

        var now = _dateTimeProvider.UtcNow;
        // O domínio exige PasswordHash não-vazio; criamos com valor temporário e substituímos pelo hash real.
        var user = new User(Guid.NewGuid(), userName, normalizedEmail, "__PENDING_HASH__", now);

        var passwordHash = _passwordHasher.HashPassword(user, request.Password);
        user.SetPasswordHash(passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);

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

