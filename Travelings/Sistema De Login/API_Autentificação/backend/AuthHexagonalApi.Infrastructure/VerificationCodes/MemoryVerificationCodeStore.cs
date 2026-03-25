using AuthHexagonalApi.Domain.Ports;
using Microsoft.Extensions.Caching.Memory;

namespace AuthHexagonalApi.Infrastructure.VerificationCodes;

public sealed class MemoryVerificationCodeStore : IVerificationCodeStore
{
    private readonly IMemoryCache _cache;

    public MemoryVerificationCodeStore(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task StoreOtpAsync(
        string email,
        string otpCode,
        string passwordResetToken,
        DateTimeOffset expiresAtUtc,
        CancellationToken cancellationToken = default)
    {
        var key = OtpKey(email, otpCode);
        _cache.Set(key, passwordResetToken, new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = expiresAtUtc
        });
        return Task.CompletedTask;
    }

    public Task<string?> ConsumeOtpAsync(
        string email,
        string otpCode,
        CancellationToken cancellationToken = default)
    {
        var key = OtpKey(email, otpCode);
        if (!_cache.TryGetValue(key, out string? token) || string.IsNullOrWhiteSpace(token))
            return Task.FromResult<string?>(null);

        _cache.Remove(key);
        return Task.FromResult<string?>(token);
    }

    public Task StoreTicketAsync(
        string ticket,
        PasswordRecoveryTicketPayload payload,
        DateTimeOffset expiresAtUtc,
        CancellationToken cancellationToken = default)
    {
        var key = TicketKey(ticket);
        _cache.Set(key, payload, new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = expiresAtUtc
        });
        return Task.CompletedTask;
    }

    public Task<PasswordRecoveryTicketPayload?> ConsumeTicketAsync(
        string ticket,
        CancellationToken cancellationToken = default)
    {
        var key = TicketKey(ticket);
        if (!_cache.TryGetValue(key, out PasswordRecoveryTicketPayload? payload) || payload is null)
            return Task.FromResult<PasswordRecoveryTicketPayload?>(null);

        _cache.Remove(key);
        return Task.FromResult<PasswordRecoveryTicketPayload?>(payload);
    }

    private static string NormalizeEmail(string email) =>
        (email ?? string.Empty).Trim().ToUpperInvariant();

    private static string OtpKey(string email, string otpCode) =>
        $"pwdrec:otp:{NormalizeEmail(email)}:{(otpCode ?? string.Empty).Trim()}";

    private static string TicketKey(string ticket) =>
        $"pwdrec:ticket:{(ticket ?? string.Empty).Trim()}";
}

