using AuthHexagonalApi.Domain.Ports;

namespace AuthHexagonalApi.Infrastructure.Security;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
