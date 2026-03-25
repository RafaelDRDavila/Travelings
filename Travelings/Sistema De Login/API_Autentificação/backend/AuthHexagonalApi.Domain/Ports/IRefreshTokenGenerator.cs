namespace AuthHexagonalApi.Domain.Ports;

public interface IRefreshTokenGenerator
{
    string GenerateToken();
    string HashToken(string token);
}

