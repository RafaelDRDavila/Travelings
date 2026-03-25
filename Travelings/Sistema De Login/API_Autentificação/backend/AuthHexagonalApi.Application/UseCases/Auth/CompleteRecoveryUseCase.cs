using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Domain.Exceptions;
using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Application.UseCases.Auth;

public interface ICompleteRecoveryUseCase
{
    Task<AuthResponse> HandleAsync(CompleteRecoveryRequest request, CancellationToken cancellationToken = default);
}

public sealed class CompleteRecoveryUseCase : ICompleteRecoveryUseCase
{
    private readonly IVerificationCodeStore _verificationCodeStore;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetService _passwordResetService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CompleteRecoveryUseCase(
        IVerificationCodeStore verificationCodeStore,
        IUserRepository userRepository,
        IPasswordResetService passwordResetService,
        IJwtTokenService jwtTokenService,
        IRefreshTokenGenerator refreshTokenGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _verificationCodeStore = verificationCodeStore;
        _userRepository = userRepository;
        _passwordResetService = passwordResetService;
        _jwtTokenService = jwtTokenService;
        _refreshTokenGenerator = refreshTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<AuthResponse> HandleAsync(
        CompleteRecoveryRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Ticket))
            errors["ticket"] = new[] { "Ticket é obrigatório." };

        if (string.IsNullOrWhiteSpace(request.Action))
            errors["action"] = new[] { "Ação é obrigatória." };

        var action = (request.Action ?? string.Empty).Trim().ToLowerInvariant();
        if (action != "access" && action != "change_password")
            errors["action"] = new[] { "Ação inválida." };

        if (errors.Count > 0)
            throw new ValidationException("Dados inválidos.", errors, "validation_error");

        var ticketPayload = await _verificationCodeStore.ConsumeTicketAsync(request.Ticket.Trim(), cancellationToken);
        if (ticketPayload is null)
        {
            throw new ValidationException(
                "Sessão expirada.",
                new Dictionary<string, string[]> { ["ticket"] = new[] { "Ticket inválido ou expirado." } },
                "invalid_recovery_ticket");
        }

        var normalizedEmail = ticketPayload.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (user is null)
        {
            // Não revelamos detalhes; mas aqui não tem como completar sem usuário.
            throw new DomainException("Não foi possível completar a recuperação.", "recovery_failed");
        }

        if (action == "change_password")
        {
            var pwErrors = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(request.NewPassword))
                pwErrors["newPassword"] = new[] { "Nova senha é obrigatória." };
            if (!string.Equals(request.NewPassword, request.ConfirmNewPassword, StringComparison.Ordinal))
                pwErrors["confirmNewPassword"] = new[] { "As senhas não conferem." };

            if (pwErrors.Count > 0)
                throw new ValidationException("Dados inválidos.", pwErrors, "validation_error");

            var result = await _passwordResetService.ResetPasswordAsync(
                ticketPayload.Email,
                ticketPayload.PasswordResetToken,
                request.NewPassword!,
                cancellationToken);

            if (!result.Succeeded)
            {
                throw new ValidationException(
                    "Não foi possível redefinir a senha.",
                    new Dictionary<string, string[]> { ["newPassword"] = result.Errors.ToArray() },
                    "reset_password_failed");
            }
        }

        // Em ambos os casos (access/change_password), invalidamos refresh antigo e emitimos novos tokens.
        await _passwordResetService.InvalidateRefreshTokenAsync(ticketPayload.Email, cancellationToken);

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

