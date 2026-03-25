namespace AuthHexagonalApi.Application.DTOs.Auth;

public sealed record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmNewPassword
);

