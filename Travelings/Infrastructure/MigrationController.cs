using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Travelings.Infrastructure
{
    [ApiController]
    [Route("api/v1/migrate")]
    public class MigrationController : ControllerBase
    {
        [HttpPost("add-ativo")]
        public IActionResult AddAtivoColumn()
        {
            try
            {
                using var ctx = new ConnectionContext();
                ctx.Database.ExecuteSqlRaw(
                    "ALTER TABLE produtos ADD COLUMN IF NOT EXISTS ativo BOOLEAN NOT NULL DEFAULT TRUE"
                );
                return Ok(new { message = "Coluna 'ativo' adicionada com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("create-favoritos")]
        public IActionResult CreateFavoritosTable()
        {
            try
            {
                using var ctx = new ConnectionContext();
                ctx.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS favoritos (
                        id SERIAL PRIMARY KEY,
                        idcliente INTEGER NOT NULL,
                        idproduto INTEGER NOT NULL,
                        UNIQUE(idcliente, idproduto)
                    );
                ");
                return Ok(new { message = "Tabela 'favoritos' criada com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("create-avaliacoes")]
        public IActionResult CreateAvaliacoesTable()
        {
            try
            {
                using var ctx = new ConnectionContext();
                ctx.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS avaliacoes (
                        id SERIAL PRIMARY KEY,
                        idproduto INTEGER NOT NULL,
                        idcliente INTEGER NOT NULL,
                        nota INTEGER NOT NULL CHECK (nota >= 0 AND nota <= 5),
                        texto TEXT NOT NULL,
                        midias TEXT,
                        nomecliente TEXT,
                        fotocliente TEXT,
                        datacriacao TIMESTAMP NOT NULL DEFAULT NOW(),
                        UNIQUE(idcliente, idproduto)
                    );
                    CREATE INDEX IF NOT EXISTS idx_avaliacoes_idproduto ON avaliacoes(idproduto);
                ");
                return Ok(new { message = "Tabela 'avaliacoes' criada com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost("add-aluguel")]
        public IActionResult AddAluguelColumns()
        {
            try
            {
                using var ctx = new ConnectionContext();
                ctx.Database.ExecuteSqlRaw(@"
                    ALTER TABLE produtos ADD COLUMN IF NOT EXISTS tipo VARCHAR(10) NOT NULL DEFAULT 'venda';
                    ALTER TABLE produtos ADD COLUMN IF NOT EXISTS precoaluguel DECIMAL;
                ");
                return Ok(new { message = "Colunas tipo e precoaluguel adicionadas." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("set-rental-products")]
        public IActionResult SetRentalProducts()
        {
            try
            {
                using var ctx = new ConnectionContext();
                // Produtos que podem ser alugados (ambos: venda e aluguel)
                ctx.Database.ExecuteSqlRaw(@"
                    UPDATE produtos SET tipo = 'ambos', precoaluguel = 89.90 WHERE nome LIKE '%Barraca%';
                    UPDATE produtos SET tipo = 'ambos', precoaluguel = 29.90 WHERE nome LIKE '%Saco de Dormir%';
                    UPDATE produtos SET tipo = 'ambos', precoaluguel = 69.90 WHERE nome LIKE '%Mochila Trekking%';
                    UPDATE produtos SET tipo = 'ambos', precoaluguel = 19.90 WHERE nome LIKE '%Prancha Bodyboard%';
                    UPDATE produtos SET tipo = 'ambos', precoaluguel = 14.90 WHERE nome LIKE '%Kit Snorkel%';
                    UPDATE produtos SET tipo = 'aluguel', precoaluguel = 9.90 WHERE nome LIKE '%Cadeira de Praia%';
                    UPDATE produtos SET tipo = 'aluguel', precoaluguel = 19.90 WHERE nome LIKE '%Guarda-Sol%';
                    UPDATE produtos SET tipo = 'ambos', precoaluguel = 49.90 WHERE nome LIKE '%Mala Spinner%';
                ");
                return Ok(new { message = "Produtos de aluguel configurados." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("add-carrinho-aluguel")]
        public IActionResult AddCarrinhoAluguelColumns()
        {
            try
            {
                using var ctx = new ConnectionContext();
                ctx.Database.ExecuteSqlRaw(@"
                    ALTER TABLE carrinho ADD COLUMN IF NOT EXISTS modalidade VARCHAR(10) NOT NULL DEFAULT 'compra';
                    ALTER TABLE carrinho ADD COLUMN IF NOT EXISTS datainicio TIMESTAMP;
                    ALTER TABLE carrinho ADD COLUMN IF NOT EXISTS datafim TIMESTAMP;
                ");
                return Ok(new { message = "Colunas modalidade, datainicio e datafim adicionadas ao carrinho." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("add-rental-fields")]
        public IActionResult AddRentalFields()
        {
            try
            {
                using var ctx = new ConnectionContext();
                ctx.Database.ExecuteSqlRaw(@"
                    ALTER TABLE produtos ADD COLUMN IF NOT EXISTS diasminimo INTEGER;
                    ALTER TABLE produtos ADD COLUMN IF NOT EXISTS diasmaximo INTEGER;
                    ALTER TABLE produtos ADD COLUMN IF NOT EXISTS quantidadedisponivel INTEGER;
                ");
                return Ok(new { message = "Colunas diasminimo, diasmaximo e quantidadedisponivel adicionadas." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("seed-v2")]
        public IActionResult SeedV2()
        {
            try
            {
                var sqlPath = Path.Combine(Directory.GetCurrentDirectory(), "seed_v2.sql");
                if (!System.IO.File.Exists(sqlPath))
                    return NotFound(new { message = "seed_v2.sql nao encontrado." });

                var sql = System.IO.File.ReadAllText(sqlPath);

                // Remove lines starting with SELECT (verification queries)
                var lines = sql.Split('\n')
                    .Where(l => !l.TrimStart().StartsWith("SELECT "))
                    .ToArray();
                sql = string.Join('\n', lines);

                using var ctx = new ConnectionContext();
                ctx.Database.ExecuteSqlRaw(sql);
                return Ok(new { message = "Seed V2 executado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, inner = ex.InnerException?.Message });
            }
        }
    }
}
