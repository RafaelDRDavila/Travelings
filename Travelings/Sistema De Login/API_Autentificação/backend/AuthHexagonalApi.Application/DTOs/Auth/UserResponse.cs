namespace AuthHexagonalApi.Application.DTOs.Auth;

public sealed record UserResponse(
    Guid Id,
    string Email,
    bool EmailConfirmed,
    DateTime CreatedAt
);

