namespace AuthHexagonalApi.Application.DTOs.Auth;

public sealed record CompleteRecoveryRequest(
    string Ticket,
    string Action,
    string? NewPassword,
    string? ConfirmNewPassword
);

