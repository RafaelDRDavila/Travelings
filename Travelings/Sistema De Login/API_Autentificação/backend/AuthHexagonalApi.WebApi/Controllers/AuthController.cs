using System.Security.Claims;
using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Application.DTOs.Common;
using AuthHexagonalApi.Application.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthHexagonalApi.WebApi.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IRegisterUserUseCase _registerUserUseCase;
    private readonly ILoginUserUseCase _loginUserUseCase;
    private readonly IGetCurrentUserUseCase _getCurrentUserUseCase;
    private readonly IRefreshTokenUseCase _refreshTokenUseCase;
    private readonly IForgotPasswordUseCase _forgotPasswordUseCase;
    private readonly IResetPasswordUseCase _resetPasswordUseCase;
    private readonly IRequestRecoveryCodeUseCase _requestRecoveryCodeUseCase;
    private readonly IVerifyRecoveryCodeUseCase _verifyRecoveryCodeUseCase;
    private readonly ICompleteRecoveryUseCase _completeRecoveryUseCase;

    public AuthController(
        IRegisterUserUseCase registerUserUseCase,
        ILoginUserUseCase loginUserUseCase,
        IGetCurrentUserUseCase getCurrentUserUseCase,
        IRefreshTokenUseCase refreshTokenUseCase,
        IForgotPasswordUseCase forgotPasswordUseCase,
        IResetPasswordUseCase resetPasswordUseCase,
        IRequestRecoveryCodeUseCase requestRecoveryCodeUseCase,
        IVerifyRecoveryCodeUseCase verifyRecoveryCodeUseCase,
        ICompleteRecoveryUseCase completeRecoveryUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _loginUserUseCase = loginUserUseCase;
        _getCurrentUserUseCase = getCurrentUserUseCase;
        _refreshTokenUseCase = refreshTokenUseCase;
        _forgotPasswordUseCase = forgotPasswordUseCase;
        _resetPasswordUseCase = resetPasswordUseCase;
        _requestRecoveryCodeUseCase = requestRecoveryCodeUseCase;
        _verifyRecoveryCodeUseCase = verifyRecoveryCodeUseCase;
        _completeRecoveryUseCase = completeRecoveryUseCase;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        // Registro desabilitado.
        return NotFound();
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _loginUserUseCase.HandleAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _getCurrentUserUseCase.HandleAsync(userId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _refreshTokenUseCase.HandleAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _forgotPasswordUseCase.HandleAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _resetPasswordUseCase.HandleAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("password-recovery/request-code")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RequestRecoveryCode(
        [FromBody] RequestRecoveryCodeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _requestRecoveryCodeUseCase.HandleAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("password-recovery/verify-code")]
    [ProducesResponseType(typeof(VerifyRecoveryCodeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyRecoveryCode(
        [FromBody] VerifyRecoveryCodeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _verifyRecoveryCodeUseCase.HandleAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("password-recovery/complete")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompleteRecovery(
        [FromBody] CompleteRecoveryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _completeRecoveryUseCase.HandleAsync(request, cancellationToken);
        return Ok(result);
    }
}
