namespace AuthHexagonalApi.Application.DTOs.Auth;

public sealed record RegisterRequest(
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword
);

