using AuthHexagonalApi.Domain.Ports;
using AuthHexagonalApi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace AuthHexagonalApi.Infrastructure.IdentityServices;

public sealed class PasswordResetService : IPasswordResetService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public PasswordResetService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string?> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return null;

        // Usuários criados fora do UserManager podem ter SecurityStamp nulo.
        // Identity exige SecurityStamp para gerar tokens (ex.: reset de senha).
        if (string.IsNullOrWhiteSpace(user.SecurityStamp))
        {
            user.SecurityStamp = Guid.NewGuid().ToString("N");
            await _userManager.UpdateAsync(user);
        }

        // Identity gera um token auto-validável; não precisa persistir no banco.
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<PasswordResetResult> ResetPasswordAsync(
        string email,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return PasswordResetResult.Fail("Token inválido ou expirado.");

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (result.Succeeded)
            return PasswordResetResult.Success();

        var errors = result.Errors.Select(e => e.Description).ToArray();
        return new PasswordResetResult(false, errors);
    }

    public async Task InvalidateRefreshTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return;

        user.RefreshTokenHash = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);
    }
}

