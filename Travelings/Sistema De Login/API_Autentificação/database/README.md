# Banco de dados (SQL)

Este diretório contém **scripts SQL** para criar o schema e popular dados iniciais.

## 1) Criar schema: `schema.sql`

Cria as tabelas do ASP.NET Core Identity (incluindo `AspNetUsers`) **com as colunas customizadas** usadas pela API.

### Como importar no pgAdmin4

1. Crie um database (ex.: `AuthHexagonalDb`)
2. Abra o **Query Tool** conectado nesse database
3. Execute o conteúdo de `schema.sql`

## 2) Seed: `seed.sql`

Insere o usuário administrador inicial na tabela `AspNetUsers` (após o schema existir).

- **E-mail:** `admin@local`
- **Senha:** `Admin@12345`

### Como aplicar o seed

No pgAdmin4, execute `schema.sql` primeiro e depois execute `seed.sql`.

O script usa `ON CONFLICT ("Id") DO NOTHING`, então pode ser executado mais de uma vez sem duplicar o usuário.

---

## Regenerar `seed.sql` (opcional)

O hash da senha é gerado pelo mesmo algoritmo do ASP.NET Core Identity. Se quiser regenerar o arquivo (por exemplo após alterar a senha padrão no código):

```bash
dotnet run --project database/SeedHashGenerator
```

O comando gera um novo hash e sobrescreve `database/seed.sql`. É necessário ter o .NET 10 SDK e o projeto `backend` compilado.
