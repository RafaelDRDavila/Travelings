using Microsoft.AspNetCore.Identity;

namespace AuthHexagonalApi.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public string? RefreshTokenHash { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime CreatedAt { get; set; }
}
