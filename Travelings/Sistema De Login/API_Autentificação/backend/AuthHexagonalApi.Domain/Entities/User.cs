namespace AuthHexagonalApi.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string UserName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string? RefreshTokenHash { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool EmailConfirmed { get; private set; }

    public User(Guid id, string userName, string email, string passwordHash, DateTime createdAt)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("User id cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("Username is required.", nameof(userName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        Id = id;
        UserName = userName.Trim();
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
        EmailConfirmed = false;
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    public void SetRefreshToken(string refreshTokenHash, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenHash))
            throw new ArgumentException("Refresh token hash is required.", nameof(refreshTokenHash));

        RefreshTokenHash = refreshTokenHash;
        RefreshTokenExpiryTime = expiresAt;
    }

    public void ClearRefreshToken()
    {
        RefreshTokenHash = null;
        RefreshTokenExpiryTime = null;
    }

    public bool HasValidRefreshToken(string refreshTokenHash, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenHash) || RefreshTokenHash is null)
            return false;

        if (!string.Equals(RefreshTokenHash, refreshTokenHash, StringComparison.Ordinal))
            return false;

        if (RefreshTokenExpiryTime is null)
            return false;

        return RefreshTokenExpiryTime.Value >= nowUtc;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }
}

