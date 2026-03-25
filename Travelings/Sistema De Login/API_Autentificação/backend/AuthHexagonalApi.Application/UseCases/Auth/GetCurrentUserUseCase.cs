using AuthHexagonalApi.Application.DTOs.Auth;
using AuthHexagonalApi.Domain.Entities;
using AuthHexagonalApi.Domain.Exceptions;
using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Application.UseCases.Auth;

public interface IGetCurrentUserUseCase
{
    Task<UserResponse> HandleAsync(Guid userId, CancellationToken cancellationToken = default);
}

public sealed class GetCurrentUserUseCase : IGetCurrentUserUseCase
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> HandleAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new DomainException("Usuário não encontrado.", "user_not_found");
        }

        return new UserResponse(
            user.Id,
            user.Email,
            user.EmailConfirmed,
            user.CreatedAt);
    }
}
