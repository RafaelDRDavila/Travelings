using System;
using System.IO;
using AuthHexagonalApi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

// Gera o INSERT SQL para o usuário admin usando o mesmo algoritmo de hash do Identity.
var hasher = new PasswordHasher<ApplicationUser>();
var user = new ApplicationUser
{
    Id = Guid.NewGuid(),
    UserName = "admin",
    NormalizedUserName = "ADMIN",
    Email = "admin@local",
    NormalizedEmail = "ADMIN@LOCAL",
    EmailConfirmed = true,
    CreatedAt = DateTime.UtcNow
};
var passwordHash = hasher.HashPassword(user, "Admin@12345");
user.PasswordHash = passwordHash;

var id = user.Id;
var createdAt = DateTime.UtcNow;
var securityStamp = Guid.NewGuid().ToString("N");
var concurrencyStamp = Guid.NewGuid().ToString();

// Escape single quotes for SQL: replace ' with ''
string Esc(string? s) => s == null ? "NULL" : $"'{s.Replace("'", "''")}'";

var sql = $"""
-- Seed: usuário admin (admin@local / Admin@12345)
-- Gerado por SeedHashGenerator. Execute após as migrations.

INSERT INTO "AspNetUsers" (
    "Id", "RefreshTokenHash", "RefreshTokenExpiryTime", "CreatedAt",
    "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed",
    "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
    "LockoutEnd", "LockoutEnabled", "AccessFailedCount"
) VALUES (
    '{id}', NULL, NULL, '{createdAt:O}',
    {Esc(user.UserName)}, {Esc(user.NormalizedUserName)}, {Esc(user.Email)}, {Esc(user.NormalizedEmail)}, true,
    {Esc(passwordHash)}, {Esc(securityStamp)}, {Esc(concurrencyStamp)},
    NULL, false, false,
    NULL, false, 0
)
ON CONFLICT ("Id") DO NOTHING;
""";

// Escreve seed.sql na pasta database/ (pasta pai de SeedHashGenerator)
var path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "seed.sql");
var fullPath = Path.GetFullPath(path);
Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
File.WriteAllText(fullPath, sql);
Console.WriteLine("Generated: " + fullPath);
