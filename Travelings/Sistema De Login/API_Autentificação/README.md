# AuthHexagonalApi

API de autenticação em .NET 10 com arquitetura hexagonal, Identity, JWT, PostgreSQL e frontend React.

## Arquitetura

- **Backend (.NET 10)**: API em camadas (Domain, Application, Infrastructure, WebApi) seguindo Ports and Adapters.
- **Domain**: Entidades (`User`), ports (repositório, JWT, hash, etc.) e exceções de domínio.
- **Application**: DTOs, casos de uso (registro, login, me, refresh-token) e orquestração.
- **Infrastructure**: EF Core + PostgreSQL, Identity (`ApplicationUser`), repositórios, JWT e geração de refresh token.
- **WebApi**: Controllers, middleware de exceções, autenticação JWT e DI.
- **Frontend**: React (Vite + TypeScript) com contexto de autenticação, interceptors axios e refresh automático em 401.
- **Database**: Scripts de seed em `database/` (fora do backend).

## Estrutura de pastas

```
backend/           # Solution .NET (Domain, Application, Infrastructure, WebApi)
frontend/          # App React (Vite + TS)
database/          # Scripts de seed (seed.ps1, README)
docker-compose.yml
README.md
```

## Pré-requisitos

- .NET 10 SDK
- PostgreSQL (local ou Docker)
- Node.js 20+ (para o frontend)
- (Opcional) Docker e Docker Compose para rodar tudo em containers

---

## Rodar localmente (sem Docker)

### 1. Banco de dados

Subir PostgreSQL (ex.: porta 5432) e criar o banco `AuthHexagonalDb` (ou usar a connection string em `backend/AuthHexagonalApi.WebApi/appsettings.json`).

Se você quiser criar o schema **sem EF migrations**, use os scripts SQL em `database/`:

- Execute `database/schema.sql` (cria as tabelas Identity)
- Execute `database/seed.sql` (cria o usuário admin)

### 2. Migrations

Na pasta raiz do repositório:

```bash
dotnet ef database update --project backend/AuthHexagonalApi.Infrastructure --startup-project backend/AuthHexagonalApi.WebApi
```

### 3. Backend

```bash
cd backend
dotnet run --project AuthHexagonalApi.WebApi
```

A API fica em `http://localhost:5116` (ou a URL em `launchSettings.json`). Swagger: `http://localhost:5116/swagger`.

### 4. Frontend

```bash
cd frontend
cp .env.example .env   # ou edite .env com VITE_API_BASE_URL=http://localhost:5116
npm install
npm run dev
```

Acesse o app no endereço indicado pelo Vite (ex.: `http://localhost:5173`).

### 5. Seed (usuário admin)

Execute o script SQL na base já migrada:

```bash
psql -h localhost -p 5432 -U postgres -d AuthHexagonalDb -f database/seed.sql
```

Credenciais: `admin@local` / `Admin@12345`.

---

## Rodar com Docker

Na pasta raiz:

```bash
docker compose up -d db
```

Aplicar migrations (uma vez), com .NET SDK no host e connection string apontando para o PostgreSQL no Docker:

```bash
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=AuthHexagonalDb;Username=postgres;Password=postgres;"
dotnet ef database update --project backend/AuthHexagonalApi.Infrastructure --startup-project backend/AuthHexagonalApi.WebApi
```

Depois subir backend e frontend:

```bash
docker compose up -d backend frontend
```

- **API**: http://localhost:8080  
- **Frontend**: http://localhost:3000  
- **PostgreSQL**: localhost:5432  

Para criar o usuário admin, execute o seed SQL no banco (ex.: com `psql` ou outro cliente):

```bash
psql -h localhost -p 5432 -U postgres -d AuthHexagonalDb -f database/seed.sql
```

---

## Variáveis de ambiente

### Backend (appsettings / Docker)

| Variável | Descrição |
|----------|-----------|
| `ConnectionStrings__DefaultConnection` | Connection string do PostgreSQL |
| `Jwt__Secret` | Chave secreta para assinatura do JWT (mín. 32 caracteres) |
| `Jwt__Issuer` | Issuer do token |
| `Jwt__Audience` | Audience do token |
| `Jwt__AccessTokenExpirationMinutes` | Expiração do access token (minutos) |
| `Jwt__RefreshTokenExpirationDays` | Expiração do refresh token (dias) |

### Frontend

| Variável | Descrição |
|----------|-----------|
| `VITE_API_BASE_URL` | URL base da API (ex.: http://localhost:5116 ou http://localhost:8080) |

---

## Exemplos de requisições HTTP

Base URL local: `http://localhost:5116` (ou `http://localhost:8080` com Docker).

### Registrar

```http
POST /auth/register
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "password": "Senha@123",
  "confirmPassword": "Senha@123"
}
```

Resposta (201): `userId`, `email`, `accessToken`, `accessTokenExpiresAtUtc`, `refreshToken`, `refreshTokenExpiresAtUtc`.

### Login

```http
POST /auth/login
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "password": "Senha@123"
}
```

Resposta (200): mesmo formato de tokens e dados do usuário.

### Dados do usuário autenticado

```http
GET /auth/me
Authorization: Bearer <accessToken>
```

Resposta (200): `id`, `email`, `emailConfirmed`, `createdAt`.

### Renovar tokens

```http
POST /auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "<refreshToken>"
}
```

Resposta (200): novo `accessToken` e `refreshToken`.

---

## Resposta de erro padrão

Em caso de erro, a API retorna:

```json
{
  "message": "Mensagem de erro",
  "errors": { "campo": ["erro1", "erro2"] },
  "statusCode": 400
}
```

`errors` pode ser `null` quando não há erros de validação por campo.

---

## Comandos úteis

- **Nova migration**:  
  `dotnet ef migrations add NomeDaMigration --project backend/AuthHexagonalApi.Infrastructure --startup-project backend/AuthHexagonalApi.WebApi`

- **Atualizar banco**:  
  `dotnet ef database update --project backend/AuthHexagonalApi.Infrastructure --startup-project backend/AuthHexagonalApi.WebApi`

- **Compilar solution**:  
  `dotnet build backend/AuthHexagonalApi.sln`

- **Seed do admin**:  
  `psql -h localhost -p 5432 -U postgres -d AuthHexagonalDb -f database/seed.sql`
