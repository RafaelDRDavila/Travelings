using AuthHexagonalApi.Domain.Ports;
using Microsoft.Extensions.Logging;

namespace AuthHexagonalApi.Infrastructure.Email;

public sealed class DevEmailSender : IEmailSender
{
    private readonly ILogger<DevEmailSender> _logger;

    public DevEmailSender(ILogger<DevEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "DEV EMAIL\nTo: {To}\nSubject: {Subject}\nBody:\n{Body}",
            toEmail,
            subject,
            htmlBody);

        return Task.CompletedTask;
    }
}

