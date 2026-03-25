using AuthHexagonalApi.Domain.Entities;

namespace AuthHexagonalApi.Domain.Ports;

public interface IPasswordHasher
{
    string HashPassword(User user, string password);
    bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
}

