using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Travelings.Infrastructure;
using Travelings.Infrastructure.Security;
using Travelings.Model;
using Travelings.Vendedores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:3002")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// JWT Authentication
builder.Services.AddSingleton<JwtService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = JwtService.IssuerValue,
        ValidateAudience = true,
        ValidAudience = JwtService.AudienceValue,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtService.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// Repositories
builder.Services.AddTransient<IvendedoresRepository, vendedoresRepository>();
builder.Services.AddTransient<IlojasRepository, lojasRepository>();
builder.Services.AddTransient<IclientesRepository, clientesRepository>();
builder.Services.AddTransient<IprodutosRepository, produtosRepository>();
builder.Services.AddTransient<IvendasRepository, vendasRepository>();
builder.Services.AddTransient<IitensvendasRepository, itensvendasRepository>();
builder.Services.AddTransient<IcarrinhoRepository, carrinhoRepository>();
builder.Services.AddTransient<IcomentariosRepository, comentariosRepository>();
builder.Services.AddTransient<IavaliacaoRepository, avaliacaoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Travelings");
        c.RoutePrefix = string.Empty;
    });
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");

// Serve uploaded images from /uploads/
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "uploads");
Directory.CreateDirectory(uploadsPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
