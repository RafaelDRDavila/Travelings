using AuthHexagonalApi.Domain.Ports;
using AuthHexagonalApi.Infrastructure.Email;
using AuthHexagonalApi.Infrastructure.IdentityServices;
using AuthHexagonalApi.Infrastructure.Persistence;
using AuthHexagonalApi.Infrastructure.Persistence.Repositories;
using AuthHexagonalApi.Infrastructure.Security;
using AuthHexagonalApi.Infrastructure.VerificationCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthHexagonalApi.Infrastructure.Configurations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddMemoryCache();
        services.AddSingleton<IVerificationCodeStore, MemoryVerificationCodeStore>();

        var emailProvider = configuration.GetSection("Email")["Provider"] ?? "Dev";
        if (string.Equals(emailProvider, "Smtp", StringComparison.OrdinalIgnoreCase))
        {
            services.Configure<SmtpOptions>(configuration.GetSection("Email:Smtp"));
            services.AddScoped<IEmailSender, SmtpEmailSender>();
        }
        else
        {
            services.AddScoped<IEmailSender, DevEmailSender>();
        }

        services.AddScoped<IPasswordResetService, PasswordResetService>();

        return services;
    }
}
