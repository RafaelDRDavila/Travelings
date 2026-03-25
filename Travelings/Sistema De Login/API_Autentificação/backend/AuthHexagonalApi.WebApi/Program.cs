using System.Text;
using AuthHexagonalApi.Application.Extensions;
using AuthHexagonalApi.Application.UseCases.Auth;
using AuthHexagonalApi.Domain.Ports;
using AuthHexagonalApi.Infrastructure.Configurations;
using AuthHexagonalApi.Infrastructure.Identity;
using AuthHexagonalApi.Infrastructure.Security;
using AuthHexagonalApi.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AuthHexagonalApi.Infrastructure.Persistence.AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IPasswordHasher, IdentityPasswordHasher>();

builder.Services.AddScoped<IForgotPasswordUseCase>(sp =>
{
    var frontendBaseUrl = builder.Configuration.GetSection("Frontend")["BaseUrl"] ?? "http://localhost:5173";
    return new ForgotPasswordUseCase(
        sp.GetRequiredService<IPasswordResetService>(),
        sp.GetRequiredService<IEmailSender>(),
        frontendBaseUrl);
});

builder.Services.AddScoped<IRequestRecoveryCodeUseCase>(sp =>
{
    var section = builder.Configuration.GetSection("PasswordRecovery");
    var otpLength = section.GetValue("OtpLength", 6);
    var otpTtlMinutes = section.GetValue("OtpTtlMinutes", 10);

    return new RequestRecoveryCodeUseCase(
        sp.GetRequiredService<IPasswordResetService>(),
        sp.GetRequiredService<IVerificationCodeStore>(),
        sp.GetRequiredService<IEmailSender>(),
        otpLength,
        TimeSpan.FromMinutes(otpTtlMinutes));
});

builder.Services.AddScoped<IVerifyRecoveryCodeUseCase>(sp =>
{
    var section = builder.Configuration.GetSection("PasswordRecovery");
    var ticketTtlMinutes = section.GetValue("TicketTtlMinutes", 10);

    return new VerifyRecoveryCodeUseCase(
        sp.GetRequiredService<IVerificationCodeStore>(),
        TimeSpan.FromMinutes(ticketTtlMinutes));
});

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT settings not configured.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "http://localhost:5175",
                "http://localhost:5176",
                "http://127.0.0.1:5173",
                "http://127.0.0.1:5174",
                "http://127.0.0.1:5175",
                "http://127.0.0.1:5176",
                "http://localhost:3000",
                "http://127.0.0.1:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
