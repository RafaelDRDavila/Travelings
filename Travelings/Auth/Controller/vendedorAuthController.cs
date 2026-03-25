using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travelings.Auth.ViewModel;
using Travelings.Infrastructure;
using Travelings.Infrastructure.Security;
using Travelings.Model;
using Travelings.Vendedores;

namespace Travelings.Auth.Controller
{
    [ApiController]
    [Route("api/v1/vendedor")]
    public class vendedorAuthController : ControllerBase
    {
        private readonly IvendedoresRepository _vendedoresRepository;
        private readonly IprodutosRepository _produtosRepository;
        private readonly IlojasRepository _lojasRepository;
        private readonly IclientesRepository _clientesRepository;
        private readonly JwtService _jwtService;

        private static readonly string[] CategoriasValidas =
        {
            "Praia", "Acampamento", "Turismo"
        };

        private static readonly string[] SubcategoriasValidas =
        {
            "Proteção", "Conforto", "Diversão", "Acessórios", "Hidratação",
            "Abrigo", "Iluminação", "Cozinha", "Equipamento", "Segurança",
            "Eletrônicos", "Organização"
        };

        public vendedorAuthController(
            IvendedoresRepository vendedoresRepository,
            IprodutosRepository produtosRepository,
            IlojasRepository lojasRepository,
            IclientesRepository clientesRepository,
            JwtService jwtService)
        {
            _vendedoresRepository = vendedoresRepository;
            _produtosRepository = produtosRepository;
            _lojasRepository = lojasRepository;
            _clientesRepository = clientesRepository;
            _jwtService = jwtService;
        }

        // ── LOGIN ──
        [HttpPost("login")]
        public IActionResult Login(VendedorLoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.email) || string.IsNullOrWhiteSpace(model.senha))
                return BadRequest(new { message = "Email e senha são obrigatórios." });

            var vendedores = _vendedoresRepository.Get();
            var vendedor = vendedores.FirstOrDefault(v => v.email == model.email);

            if (vendedor == null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            // Support BCrypt and legacy plain text
            bool passwordValid;
            try
            {
                passwordValid = BCrypt.Net.BCrypt.Verify(model.senha, vendedor.senha);
            }
            catch
            {
                passwordValid = vendedor.senha == model.senha;
            }

            if (!passwordValid)
                return Unauthorized(new { message = "Credenciais inválidas." });

            var (accessToken, expiresAt) = _jwtService.GenerateAccessToken(
                vendedor.id, vendedor.email, vendedor.nomecompleto);

            return Ok(new
            {
                userId = vendedor.id,
                email = vendedor.email,
                nome = vendedor.nomecompleto,
                idloja = vendedor.idloja,
                role = "vendedor",
                accessToken,
                accessTokenExpiresAtUtc = expiresAt
            });
        }

        // ── ME ──
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var vendedor = GetCurrentVendedor();
            if (vendedor == null) return Unauthorized(new { message = "Token inválido." });

            return Ok(new
            {
                id = vendedor.id,
                nomecompleto = vendedor.nomecompleto,
                email = vendedor.email,
                idloja = vendedor.idloja,
                role = "vendedor"
            });
        }

        // ── LIST MY PRODUCTS ──
        [Authorize]
        [HttpGet("produtos")]
        public IActionResult GetMyProducts()
        {
            var vendedor = GetCurrentVendedor();
            if (vendedor == null) return Unauthorized(new { message = "Token inválido." });

            var produtos = _produtosRepository.Get()
                .Where(p => p.idloja == vendedor.idloja)
                .OrderByDescending(p => p.id)
                .ToList();

            return Ok(produtos);
        }

        // ── CREATE PRODUCT ──
        [Authorize]
        [HttpPost("produtos")]
        public IActionResult CreateProduct([FromBody] ProdutoCreateViewModel model)
        {
            var vendedor = GetCurrentVendedor();
            if (vendedor == null) return Unauthorized(new { message = "Token inválido." });

            // Validations
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(model.nome) || model.nome.Trim().Length < 5)
                errors["nome"] = "Nome deve ter pelo menos 5 caracteres.";

            if (model.preco <= 0)
                errors["preco"] = "Preço deve ser um valor positivo.";

            if (model.estoque < 0)
                errors["estoque"] = "Estoque não pode ser negativo.";

            if (string.IsNullOrWhiteSpace(model.categoria) || !CategoriasValidas.Contains(model.categoria))
                errors["categoria"] = "Categoria invalida. Use: Praia, Acampamento ou Turismo.";

            if (string.IsNullOrWhiteSpace(model.descricao))
                errors["descricao"] = "Descrição é obrigatória.";

            if (model.tipo == "aluguel" || model.tipo == "ambos")
            {
                if (model.precoaluguel == null || model.precoaluguel <= 0)
                    errors["precoaluguel"] = "Preço de aluguel deve ser positivo.";
                if (model.diasminimo.HasValue && model.diasminimo < 1)
                    errors["diasminimo"] = "Dias mínimo deve ser pelo menos 1.";
                if (model.diasmaximo.HasValue && model.diasminimo.HasValue && model.diasmaximo < model.diasminimo)
                    errors["diasmaximo"] = "Dias máximo deve ser maior ou igual ao mínimo.";
            }

            if (errors.Count > 0)
                return BadRequest(new { message = "Dados inválidos.", errors });

            var produto = new produtos(
                id: 0,
                nome: model.nome.Trim(),
                preco: model.preco,
                descricao: model.descricao.Trim(),
                estoque: model.estoque,
                categoria: model.categoria,
                imagem: string.IsNullOrWhiteSpace(model.imagem) ? "" : model.imagem,
                idloja: vendedor.idloja,
                subcategoria: string.IsNullOrWhiteSpace(model.subcategoria) ? null : model.subcategoria.Trim(),
                midias: string.IsNullOrWhiteSpace(model.midias) ? null : model.midias,
                ativo: true,
                tipo: model.tipo ?? "venda",
                precoaluguel: model.precoaluguel,
                diasminimo: model.diasminimo,
                diasmaximo: model.diasmaximo,
                quantidadedisponivel: model.quantidadedisponivel
            );

            _produtosRepository.Add(produto);

            return Created("", produto);
        }

        // ── UPDATE PRODUCT ──
        [Authorize]
        [HttpPut("produtos/{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProdutoCreateViewModel model)
        {
            var vendedor = GetCurrentVendedor();
            if (vendedor == null) return Unauthorized(new { message = "Token inválido." });

            using var ctx = new ConnectionContext();
            var produto = ctx.produtos.FirstOrDefault(p => p.id == id && p.idloja == vendedor.idloja);

            if (produto == null)
                return NotFound(new { message = "Produto não encontrado." });

            // Validations
            var errors = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(model.nome) && model.nome.Trim().Length < 5)
                errors["nome"] = "Nome deve ter pelo menos 5 caracteres.";
            if (model.preco <= 0)
                errors["preco"] = "Preço deve ser um valor positivo.";
            if (model.estoque < 0)
                errors["estoque"] = "Estoque não pode ser negativo.";

            if (errors.Count > 0)
                return BadRequest(new { message = "Dados inválidos.", errors });

            produto.nome = model.nome?.Trim() ?? produto.nome;
            produto.preco = model.preco;
            produto.descricao = model.descricao?.Trim() ?? produto.descricao;
            produto.estoque = model.estoque;
            produto.categoria = model.categoria ?? produto.categoria;
            produto.subcategoria = string.IsNullOrWhiteSpace(model.subcategoria) ? produto.subcategoria : model.subcategoria.Trim();
            produto.imagem = model.imagem ?? produto.imagem;
            produto.midias = string.IsNullOrWhiteSpace(model.midias) ? produto.midias : model.midias;
            produto.tipo = model.tipo ?? produto.tipo;
            produto.precoaluguel = model.precoaluguel ?? produto.precoaluguel;
            produto.diasminimo = model.diasminimo;
            produto.diasmaximo = model.diasmaximo;
            produto.quantidadedisponivel = model.quantidadedisponivel;

            ctx.SaveChanges();

            return Ok(produto);
        }

        // ── TOGGLE STATUS (Pausar/Ativar) ──
        [Authorize]
        [HttpPatch("produtos/{id}/toggle")]
        public IActionResult ToggleProduct(int id)
        {
            var vendedor = GetCurrentVendedor();
            if (vendedor == null) return Unauthorized(new { message = "Token inválido." });

            using var ctx = new ConnectionContext();
            var produto = ctx.produtos.FirstOrDefault(p => p.id == id && p.idloja == vendedor.idloja);

            if (produto == null)
                return NotFound(new { message = "Produto não encontrado." });

            produto.ativo = !produto.ativo;
            ctx.SaveChanges();

            return Ok(new { id = produto.id, ativo = produto.ativo });
        }

        // ── DELETE PRODUCT ──
        [Authorize]
        [HttpDelete("produtos/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var vendedor = GetCurrentVendedor();
            if (vendedor == null) return Unauthorized(new { message = "Token inválido." });

            using var ctx = new ConnectionContext();
            var produto = ctx.produtos.FirstOrDefault(p => p.id == id && p.idloja == vendedor.idloja);

            if (produto == null)
                return NotFound(new { message = "Produto não encontrado." });

            ctx.produtos.Remove(produto);
            ctx.SaveChanges();

            return Ok(new { message = "Produto removido." });
        }

        // ── GET MY STORE ──
        [Authorize]
        [HttpGet("loja")]
        public IActionResult GetMyStore()
        {
            var vendedor = GetCurrentVendedor();
            if (vendedor == null) return Unauthorized(new { message = "Token inválido." });

            if (vendedor.idloja == 0)
                return Ok(new { exists = false });

            var loja = _lojasRepository.GetById(vendedor.idloja);
            if (loja == null)
                return Ok(new { exists = false });

            return Ok(new
            {
                exists = true,
                id = loja.id,
                nome = loja.nome,
                cnpj = loja.cnpj,
                descricao = loja.descricao,
                logo = loja.logo,
                banner = loja.banner,
                email = loja.email,
                telefone = loja.telefone,
                endereco = loja.endereco
            });
        }

        // ── CREATE OR UPDATE MY STORE ──
        [Authorize]
        [HttpPut("loja")]
        public IActionResult UpdateMyStore([FromBody] LojaUpdateViewModel model)
        {
            var vendedor = GetCurrentVendedor();
            if (vendedor == null) return Unauthorized(new { message = "Token inválido." });

            var errors = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(model.nome) || model.nome.Trim().Length < 2)
                errors["nome"] = "Nome da loja deve ter pelo menos 2 caracteres.";

            // CNPJ is only validated/required when creating a new store
            var isUpdate = vendedor.idloja != 0;

            if (!isUpdate)
            {
                var cnpjDigits = new string((model.cnpj ?? "").Where(char.IsDigit).ToArray());
                if (!ValidateCnpjFormat(cnpjDigits))
                    errors["cnpj"] = "CNPJ inválido.";
                else
                {
                    var lojas = _lojasRepository.Get();
                    var existing = lojas.FirstOrDefault(l =>
                        new string(l.cnpj.Where(char.IsDigit).ToArray()) == cnpjDigits);
                    if (existing != null && existing.id != vendedor.idloja)
                        errors["cnpj"] = "CNPJ já está em uso por outra loja.";
                }
            }

            if (errors.Count > 0)
                return BadRequest(new { message = "Dados inválidos.", errors });

            using var ctx = new ConnectionContext();

            if (isUpdate)
            {
                var loja = ctx.lojas.FirstOrDefault(l => l.id == vendedor.idloja);
                if (loja != null)
                {
                    loja.nome = model.nome.Trim();
                    // CNPJ cannot be changed after store creation
                    loja.descricao = model.descricao?.Trim() ?? "";
                    loja.logo = model.logo ?? "";
                    loja.banner = model.banner ?? "";
                    loja.email = model.email?.Trim() ?? "";
                    loja.telefone = model.telefone?.Trim() ?? "";
                    loja.endereco = model.endereco?.Trim() ?? "";
                    ctx.SaveChanges();
                    return Ok(new { message = "Loja atualizada.", id = loja.id });
                }
            }

            // Create new store
            var novaLoja = new lojas(
                id: 0,
                nome: model.nome.Trim(),
                cnpj: model.cnpj.Trim(),
                descricao: model.descricao?.Trim() ?? "",
                logo: model.logo ?? "",
                banner: model.banner ?? "",
                email: model.email?.Trim() ?? "",
                telefone: model.telefone?.Trim() ?? "",
                endereco: model.endereco?.Trim() ?? ""
            );
            ctx.lojas.Add(novaLoja);
            ctx.SaveChanges();

            // Link vendedor to new store
            var vend = ctx.vendedores.FirstOrDefault(v => v.id == vendedor.id);
            if (vend != null)
            {
                vend.idloja = novaLoja.id;
                ctx.SaveChanges();
            }

            return Ok(new { message = "Loja criada.", id = novaLoja.id });
        }

        // ── CATEGORIES LIST ──
        [HttpGet("categorias")]
        public IActionResult GetCategorias()
        {
            return Ok(CategoriasValidas);
        }

        // ── CNPJ Validation ──
        private static bool ValidateCnpjFormat(string digits)
        {
            if (digits.Length != 14) return false;
            if (digits.Distinct().Count() == 1) return false;

            int[] w1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;
            for (int i = 0; i < 12; i++) sum += (digits[i] - '0') * w1[i];
            int r = sum % 11;
            int d1 = r < 2 ? 0 : 11 - r;
            if ((digits[12] - '0') != d1) return false;

            int[] w2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            sum = 0;
            for (int i = 0; i < 13; i++) sum += (digits[i] - '0') * w2[i];
            r = sum % 11;
            int d2 = r < 2 ? 0 : 11 - r;
            if ((digits[13] - '0') != d2) return false;

            return true;
        }

        // ── REGISTER FROM CLIENT ──
        [Authorize]
        [HttpPost("register")]
        public IActionResult RegisterFromClient(VendedorRegisterViewModel model)
        {
            // Get client from JWT
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");
            if (claim == null || !int.TryParse(claim.Value, out var clienteId))
                return Unauthorized(new { message = "Token invalido." });

            var cliente = _clientesRepository.GetById(clienteId);
            if (cliente == null)
                return NotFound(new { message = "Usuario nao encontrado." });

            // Check if client is already a vendor
            var existingVendedor = _vendedoresRepository.Get()
                .FirstOrDefault(v => v.email == cliente.email);
            if (existingVendedor != null)
                return Conflict(new { message = "Voce ja e um vendedor." });

            // Validate store data
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(model.nomeLoja))
                errors["nomeLoja"] = "Nome da loja e obrigatorio.";
            if (string.IsNullOrWhiteSpace(model.logo))
                errors["logo"] = "Foto da loja e obrigatoria.";
            if (string.IsNullOrWhiteSpace(model.banner))
                errors["banner"] = "Banner da loja e obrigatorio.";

            var cnpjDigits = new string(model.cnpj?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());
            if (cnpjDigits.Length != 14 || !ValidateCnpjFormat(cnpjDigits))
                errors["cnpj"] = "CNPJ invalido.";
            else
            {
                var existingLoja = _lojasRepository.Get()
                    .FirstOrDefault(l => new string(l.cnpj.Where(char.IsDigit).ToArray()) == cnpjDigits);
                if (existingLoja != null)
                    errors["cnpj"] = "CNPJ ja esta em uso.";
            }

            if (model.categorias == null || model.categorias.Length == 0)
                errors["categorias"] = "Selecione pelo menos 1 categoria.";

            if (errors.Count > 0)
                return BadRequest(new { message = "Dados invalidos.", errors });

            using var ctx = new ConnectionContext();

            // Create store
            var loja = new lojas(
                id: 0,
                nome: model.nomeLoja.Trim(),
                cnpj: cnpjDigits,
                descricao: string.Join(", ", model.categorias),
                logo: model.logo ?? "",
                banner: model.banner ?? "",
                email: cliente.email,
                telefone: cliente.telefone ?? "",
                endereco: model.endereco?.Trim() ?? ""
            );
            ctx.lojas.Add(loja);
            ctx.SaveChanges();

            // Create vendor with hashed password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(cliente.senha);
            var vendedor = new vendedores(
                id: 0,
                nomecompleto: cliente.nomecompleto,
                email: cliente.email,
                senha: hashedPassword,
                cpf: cliente.cpf,
                idloja: loja.id
            );
            ctx.vendedores.Add(vendedor);
            ctx.SaveChanges();

            // Generate vendor token
            var (accessToken, expiresAt) = _jwtService.GenerateAccessToken(
                vendedor.id, vendedor.email, vendedor.nomecompleto);

            return Ok(new
            {
                userId = vendedor.id,
                email = vendedor.email,
                nome = vendedor.nomecompleto,
                idloja = loja.id,
                role = "vendedor",
                accessToken,
                accessTokenExpiresAtUtc = expiresAt
            });
        }

        // ── CHECK IF CLIENT IS VENDOR ──
        [Authorize]
        [HttpGet("check")]
        public IActionResult CheckVendor()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");
            if (claim == null || !int.TryParse(claim.Value, out var userId))
                return Unauthorized();

            var cliente = _clientesRepository.GetById(userId);
            if (cliente == null) return NotFound();

            var vendedor = _vendedoresRepository.Get()
                .FirstOrDefault(v => v.email == cliente.email);

            if (vendedor == null || vendedor.idloja == 0)
                return Ok(new { isVendor = false });

            return Ok(new
            {
                isVendor = true,
                idloja = vendedor.idloja,
                vendedorId = vendedor.id
            });
        }

        // ── DELETE STORE ──
        [Authorize]
        [HttpDelete("loja")]
        public IActionResult DeleteStore()
        {
            // Support both vendor token (vendedor ID) and client token (cliente ID)
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");
            if (claim == null || !int.TryParse(claim.Value, out var userId))
                return Unauthorized(new { message = "Token invalido." });

            // Try finding vendor directly by ID
            var vendedor = _vendedoresRepository.GetById(userId);

            // If not found, the token is from a client — find vendor by client's email
            if (vendedor == null)
            {
                var cliente = _clientesRepository.GetById(userId);
                if (cliente == null) return Unauthorized(new { message = "Token invalido." });

                vendedor = _vendedoresRepository.Get()
                    .FirstOrDefault(v => v.email == cliente.email);
            }

            if (vendedor == null) return NotFound(new { message = "Vendedor nao encontrado." });
            if (vendedor.idloja == 0) return NotFound(new { message = "Voce nao tem loja." });

            using var ctx = new ConnectionContext();

            var productIds = ctx.produtos
                .Where(p => p.idloja == vendedor.idloja)
                .Select(p => p.id)
                .ToList();

            if (productIds.Count > 0)
            {
                // Delete all references to these products
                ctx.favoritos.Where(f => productIds.Contains(f.idproduto)).ExecuteDelete();
                ctx.avaliacoes.Where(a => productIds.Contains(a.idproduto)).ExecuteDelete();
                ctx.carrinho.Where(c => productIds.Contains(c.idproduto)).ExecuteDelete();

                // Delete itensvendas referencing these products
                ctx.itensvendas.Where(iv => productIds.Contains(iv.idproduto)).ExecuteDelete();

                // Delete the products
                ctx.produtos.Where(p => p.idloja == vendedor.idloja).ExecuteDelete();
            }

            // Delete the store
            ctx.lojas.Where(l => l.id == vendedor.idloja).ExecuteDelete();

            // Delete the vendor record
            ctx.vendedores.Where(v => v.id == vendedor.id).ExecuteDelete();

            return Ok(new { message = "Loja excluida com sucesso." });
        }

        // ── Helper ──
        private vendedores? GetCurrentVendedor()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");
            if (claim == null || !int.TryParse(claim.Value, out var userId))
                return null;

            // Try as vendor ID first
            var vendedor = _vendedoresRepository.GetById(userId);
            if (vendedor != null) return vendedor;

            // Fallback: token is from a client — find vendor by client's email
            var cliente = _clientesRepository.GetById(userId);
            if (cliente == null) return null;

            return _vendedoresRepository.Get()
                .FirstOrDefault(v => v.email == cliente.email);
        }
    }

    // ── ViewModels ──
    public class ProdutoCreateViewModel
    {
        public string nome { get; set; } = "";
        public decimal preco { get; set; }
        public string descricao { get; set; } = "";
        public int estoque { get; set; }
        public string categoria { get; set; } = "";
        public string subcategoria { get; set; } = "";
        public string imagem { get; set; } = "";
        public string midias { get; set; } = "";
        public string tipo { get; set; } = "venda";
        public decimal? precoaluguel { get; set; }
        public int? diasminimo { get; set; }
        public int? diasmaximo { get; set; }
        public int? quantidadedisponivel { get; set; }
    }

    public class LojaUpdateViewModel
    {
        public string nome { get; set; } = "";
        public string cnpj { get; set; } = "";
        public string descricao { get; set; } = "";
        public string logo { get; set; } = "";
        public string banner { get; set; } = "";
        public string email { get; set; } = "";
        public string telefone { get; set; } = "";
        public string endereco { get; set; } = "";
    }

    public class VendedorRegisterViewModel
    {
        public string nomeLoja { get; set; } = "";
        public string cnpj { get; set; } = "";
        public string logo { get; set; } = "";
        public string banner { get; set; } = "";
        public string endereco { get; set; } = "";
        public string cep { get; set; } = "";
        public string[] categorias { get; set; } = Array.Empty<string>();
    }
}
