using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Application.DTOs.Common;
using AuthHexagonalApi.Domain.Exceptions;
using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Application.UseCases.Auth;

public interface IResetPasswordUseCase
{
    Task<MessageResponse> HandleAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
}

public sealed class ResetPasswordUseCase : IResetPasswordUseCase
{
    private readonly IPasswordResetService _passwordResetService;

    public ResetPasswordUseCase(IPasswordResetService passwordResetService)
    {
        _passwordResetService = passwordResetService;
    }

    public async Task<MessageResponse> HandleAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Email))
            errors["email"] = new[] { "Email é obrigatório." };

        if (string.IsNullOrWhiteSpace(request.Token))
            errors["token"] = new[] { "Token é obrigatório." };

        if (string.IsNullOrWhiteSpace(request.NewPassword))
            errors["newPassword"] = new[] { "Nova senha é obrigatória." };

        if (!string.Equals(request.NewPassword, request.ConfirmNewPassword, StringComparison.Ordinal))
            errors["confirmNewPassword"] = new[] { "As senhas não conferem." };

        if (errors.Count > 0)
            throw new ValidationException("Dados inválidos.", errors, "validation_error");

        var result = await _passwordResetService.ResetPasswordAsync(
            request.Email.Trim(),
            request.Token,
            request.NewPassword,
            cancellationToken);

        if (!result.Succeeded)
        {
            throw new ValidationException(
                "Não foi possível redefinir a senha.",
                new Dictionary<string, string[]> { ["token"] = result.Errors.ToArray() },
                "reset_password_failed");
        }

        await _passwordResetService.InvalidateRefreshTokenAsync(request.Email.Trim(), cancellationToken);

        return new MessageResponse("Senha redefinida com sucesso.");
    }
}

