using AuthHexagonalApi.Domain.Entities;
using AuthHexagonalApi.Domain.Ports;
using AuthHexagonalApi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace AuthHexagonalApi.Infrastructure.Security;

public sealed class IdentityPasswordHasher : IPasswordHasher
{
    private readonly IPasswordHasher<ApplicationUser> _identityHasher;

    public IdentityPasswordHasher(IPasswordHasher<ApplicationUser> identityHasher)
    {
        _identityHasher = identityHasher;
    }

    public string HashPassword(User user, string password)
    {
        var appUser = new ApplicationUser
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };
        return _identityHasher.HashPassword(appUser, password);
    }

    public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        var appUser = new ApplicationUser
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            PasswordHash = hashedPassword
        };
        var result = _identityHasher.VerifyHashedPassword(appUser, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
