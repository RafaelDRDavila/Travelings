namespace AuthHexagonalApi.Domain.Ports;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

