-- Seed: usuário admin (admin@local / Admin@12345)
-- Execute após criar o schema (database/schema.sql).
-- Para regenerar este arquivo: dotnet run --project database/SeedHashGenerator

INSERT INTO "AspNetUsers" (
    "Id", "RefreshTokenHash", "RefreshTokenExpiryTime", "CreatedAt",
    "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed",
    "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
    "LockoutEnd", "LockoutEnabled", "AccessFailedCount"
) VALUES (
    'd6bf4be5-482d-4772-9216-ad3eab411a11', NULL, NULL, '2026-03-17T22:43:29.6613444Z',
    'admin', 'ADMIN', 'admin@local', 'ADMIN@LOCAL', true,
    'AQAAAAIAAYagAAAAEEUJZD0eyDjSbExjpNcPx7LBWQchRKcgAff2nc/6LpfI/drY4uLF+E2UIKsxg8iCdg==', '9a197b80efab4083a2c844071d27c15b', '4f5cdfdd-61d0-499c-975a-942a9596508c',
    NULL, false, false,
    NULL, false, 0
)
ON CONFLICT ("Id") DO NOTHING;
