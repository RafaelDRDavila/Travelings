# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

### Backend (ASP.NET Core)

```bash
dotnet build
dotnet run                          # HTTP on port 5260
dotnet run --launch-profile https   # HTTPS on 7161 + HTTP on 5260
```

Swagger UI is available at the root URL (`/`) in Development mode.

### Frontend (Next.js)

```bash
cd frontend
npm install
npm run dev                         # http://localhost:3000
npm run build                       # production build
npm run lint                        # ESLint (flat config in eslint.config.mjs)
```

Playwright is installed as a devDependency but no tests or config exist yet.

### Database Seeding

```bash
# Via API endpoint after server is running:
# POST /api/v1/migrate/seed-v2
# Or directly:
psql -f seed_v2.sql
```

Migration endpoints at `/api/v1/migrate` handle schema changes (add columns, create tables) — there are no EF Core migrations.

## Database

PostgreSQL is required. Connection string is **hardcoded** in `Infrastructure/ConnectionContext.cs`:
- Server: localhost:5432, Database: Web, User: postgres, Password: Admin

EF Core is used as the ORM with Npgsql provider. DbSets: lojas, vendedores, clientes, produtos, carrinhos, vendas, itensvendas, comentarios, favoritos, avaliacoes.

## Architecture

ASP.NET Core 10.0 Web API (`net10.0`) for a travel/outdoor retail e-commerce system. The codebase is in **Portuguese**. There is no `.sln` file — just a single `.csproj`.

### Domain-Driven Layered Structure

Each business domain is a top-level folder with four sub-layers:

```
[Domain]/
  Controller/    → REST endpoints (route: api/v1/{resource})
  ViewModel/     → DTOs for API input/output
  Model/         → Entity class + repository interface (I[Domain]Repository)
  Repository/    → EF Core data access implementation
```

### Business Domains

- **Lojas** (Stores) — retail stores identified by CNPJ
- **Vendedores** (Sellers) — sales reps linked to a store (idloja)
- **Clientes** (Customers) — customer accounts
- **Produtos** (Products) — product catalog with three categories: Praia, Acampamento, Turismo. Supports sale and rental (tipo: venda/aluguel/ambos)
- **Carrinho** (Shopping Cart) — cart items linking customers to products; supports rental mode with date range
- **Vendas** (Sales) — order records linking customer + seller
- **ItensVendas** (Order Items) — line items linking a sale to products
- **Comentarios** (Comments) — customer reviews/comments
- **Avaliacoes** (Reviews/Ratings) — product ratings (0-5) with text and media; prevents duplicate reviews per customer

### Auth System

Two auth controllers live under `Auth/Controller/`:
- **authController** (`/api/v1/auth`) — customer registration, login, password recovery (forgot/reset via email or CPF), profile (`/me`)
- **vendedorAuthController** (`/api/v1/vendedor`) — seller login, seller registration (from existing client), CRUD for seller's products and store management, CNPJ validation

JWT auth configured in `Infrastructure/Security/JwtService.cs` — 60-minute access tokens, 7-day refresh tokens. BCrypt password hashing with legacy plaintext fallback.

### Standalone Controllers (in `Controllers/`)

- **FavoritosController** (`/api/v1/favoritos`) — add/remove/list favorite products per authenticated user. Note: the `Favorito` entity is defined inline in this controller file, not in a separate domain folder
- **UploadController** (`/api/v1/upload`) — file upload for images (.jpg/.jpeg/.png/.gif/.webp, max 5MB) and videos (.mp4/.webm/.mov, max 50MB); saves to `/uploads/` with GUID filenames
- **MigrationController** (`/api/v1/migrate`) — API-driven schema migrations and seeding

### Infrastructure

- `Infrastructure/Program.cs` — DI registration (all repositories as `AddTransient`), JWT bearer auth, CORS (localhost:3000-3002), Swagger config, static file serving for `/uploads/`
- `Infrastructure/ConnectionContext.cs` — EF Core DbContext; each repository instantiates its own context directly (not injected)
- `Infrastructure/Security/JwtService.cs` — JWT token generation and validation

### Frontend

Next.js 16 with React 19, Tailwind 4, TypeScript. API service layer in `frontend/services/api.ts` talks to backend at `localhost:5260/api/v1`. Auth token stored in localStorage.

Key routes: `/login`, `/registro`, `/esqueci-senha`, `/produto`, `/loja`, `/criar-loja`, `/vendedor`, `/perfil`, `/checkout`, `/sobre`, `/privacidade`, `/termos`.

### Patterns

- Repository pattern with interface per domain (e.g., `IlojasRepository` → `lojasRepository`)
- Controllers receive repositories via constructor injection
- CRUD operations: `Add()`, `Get()`, `GetById()`, `Update()`, `Delete()`
- All EF Core calls are synchronous (no async/await) — `Delete()` calls `ExecuteDeleteAsync()` without awaiting
- Naming convention: lowercase class names matching Portuguese table names (e.g., `lojas`, `vendedores`, `clientes`). Exception: `avaliacao` model is singular while its DbSet is `avaliacoes`
- Entity properties use `private set` with constructor initialization
- No test projects exist

### Excluded from Build

- `Sistema De Login/` — experimental hexagonal-architecture auth API (separate .csproj files, excluded in main .csproj)
- `frontend/` and `Public/` — excluded from .NET compilation
