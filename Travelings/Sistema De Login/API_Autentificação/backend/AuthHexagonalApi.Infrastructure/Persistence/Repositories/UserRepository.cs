using AuthHexagonalApi.Domain.Entities;
using AuthHexagonalApi.Domain.Ports;
using AuthHexagonalApi.Infrastructure.Identity;
using AuthHexagonalApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthHexagonalApi.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var appUser = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return appUser is null ? null : MapToDomain(appUser);
    }

    public async Task<Domain.Entities.User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        // Identity armazena NormalizedEmail em MAIÚSCULO
        var normalized = email.Trim().ToUpperInvariant();
        var appUser = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalized, cancellationToken);

        return appUser is null ? null : MapToDomain(appUser);
    }

    public async Task<Domain.Entities.User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        var normalized = userName.Trim().ToUpperInvariant();
        var appUser = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedUserName == normalized, cancellationToken);

        return appUser is null ? null : MapToDomain(appUser);
    }

    public async Task<Domain.Entities.User?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken = default)
    {
        var appUser = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.RefreshTokenHash == refreshTokenHash, cancellationToken);

        return appUser is null ? null : MapToDomain(appUser);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.Trim().ToUpperInvariant();
        return await _context.Users
            .AnyAsync(u => u.NormalizedEmail == normalized, cancellationToken);
    }

    public async Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default)
    {
        var normalized = userName.Trim().ToUpperInvariant();
        return await _context.Users
            .AnyAsync(u => u.NormalizedUserName == normalized, cancellationToken);
    }

    public async Task AddAsync(Domain.Entities.User user, CancellationToken cancellationToken = default)
    {
        var appUser = MapToApplicationUser(user);
        _context.Users.Add(appUser);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Domain.Entities.User user, CancellationToken cancellationToken = default)
    {
        var appUser = await _context.Users.FindAsync(new object[] { user.Id }, cancellationToken);
        if (appUser is null)
            return;

        appUser.PasswordHash = user.PasswordHash;
        appUser.RefreshTokenHash = user.RefreshTokenHash;
        appUser.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
        appUser.EmailConfirmed = user.EmailConfirmed;

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static Domain.Entities.User MapToDomain(ApplicationUser appUser)
    {
        var user = new Domain.Entities.User(
            appUser.Id,
            appUser.UserName ?? string.Empty,
            appUser.Email ?? string.Empty,
            appUser.PasswordHash ?? string.Empty,
            appUser.CreatedAt);

        if (appUser.RefreshTokenHash is not null && appUser.RefreshTokenExpiryTime is not null)
        {
            user.SetRefreshToken(appUser.RefreshTokenHash, appUser.RefreshTokenExpiryTime.Value);
        }

        if (appUser.EmailConfirmed)
        {
            user.ConfirmEmail();
        }

        return user;
    }

    private static ApplicationUser MapToApplicationUser(Domain.Entities.User user)
    {
        return new ApplicationUser
        {
            Id = user.Id,
            UserName = user.UserName,
            NormalizedUserName = user.UserName.ToUpperInvariant(),
            Email = user.Email,
            NormalizedEmail = user.Email.ToUpperInvariant(),
            PasswordHash = user.PasswordHash,
            // Identity usa SecurityStamp em tokens (ex.: reset de senha)
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            RefreshTokenHash = user.RefreshTokenHash,
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
            CreatedAt = user.CreatedAt,
            EmailConfirmed = user.EmailConfirmed
        };
    }
}
