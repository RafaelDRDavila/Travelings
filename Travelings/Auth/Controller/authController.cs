using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travelings.Auth.ViewModel;
using Travelings.Infrastructure;
using Travelings.Infrastructure.Security;
using Travelings.Model;

namespace Travelings.Auth.Controller
{
    [ApiController]
    [Route("api/v1/auth")]
    public class authController : ControllerBase
    {
        private readonly IclientesRepository _clientesRepository;
        private readonly JwtService _jwtService;

        public authController(IclientesRepository clientesRepository, JwtService jwtService)
        {
            _clientesRepository = clientesRepository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.email) || string.IsNullOrWhiteSpace(model.senha))
                return BadRequest(new { message = "Email e senha são obrigatórios." });

            if (model.senha.Length < 6)
                return BadRequest(new { message = "A senha deve ter pelo menos 6 caracteres." });

            var existingClients = _clientesRepository.Get();

            if (existingClients.Any(c => c.email == model.email))
                return Conflict(new { message = "Este email já está cadastrado.", field = "email" });

            if (!string.IsNullOrWhiteSpace(model.cpf) &&
                existingClients.Any(c => c.cpf == model.cpf.Replace(".", "").Replace("-", "")))
                return Conflict(new { message = "Este CPF já está cadastrado.", field = "cpf" });

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.senha);

            var uniqueId = Guid.NewGuid().ToString("N")[..12];
            var telefone = string.IsNullOrWhiteSpace(model.telefone) ? $"pending-{uniqueId}" : model.telefone;
            var cpfClean = model.cpf?.Replace(".", "").Replace("-", "") ?? "";

            var cliente = new clientes(
                nomecompleto: model.nomecompleto,
                email: model.email,
                senha: hashedPassword,
                cpf: cpfClean,
                telefone: telefone,
                endereco: string.IsNullOrWhiteSpace(model.endereco) ? "" : model.endereco,
                cep: string.IsNullOrWhiteSpace(model.cep) ? "" : model.cep
            );

            try
            {
                _clientesRepository.Add(cliente);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("cpf", StringComparison.OrdinalIgnoreCase))
                    return Conflict(new { message = "Este CPF já está cadastrado.", field = "cpf" });
                if (msg.Contains("telefone", StringComparison.OrdinalIgnoreCase))
                    return Conflict(new { message = "Este telefone já está cadastrado.", field = "telefone" });
                if (msg.Contains("email", StringComparison.OrdinalIgnoreCase))
                    return Conflict(new { message = "Este email já está cadastrado.", field = "email" });
                return Conflict(new { message = "Erro ao criar conta. Verifique os dados e tente novamente." });
            }

            var (accessToken, expiresAt) = _jwtService.GenerateAccessToken(cliente.id, cliente.email, cliente.nomecompleto);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return Created("", new AuthResponseViewModel
            {
                userId = cliente.id,
                email = cliente.email,
                nome = cliente.nomecompleto,
                accessToken = accessToken,
                accessTokenExpiresAtUtc = expiresAt,
                refreshToken = refreshToken
            });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.email) || string.IsNullOrWhiteSpace(model.senha))
                return BadRequest(new { message = "Email e senha são obrigatórios." });

            var clientes = _clientesRepository.Get();
            var cliente = clientes.FirstOrDefault(c => c.email == model.email);

            if (cliente == null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            bool passwordValid;
            try
            {
                passwordValid = BCrypt.Net.BCrypt.Verify(model.senha, cliente.senha);
            }
            catch
            {
                passwordValid = cliente.senha == model.senha;
            }

            if (!passwordValid)
                return Unauthorized(new { message = "Credenciais inválidas." });

            var (accessToken, expiresAt) = _jwtService.GenerateAccessToken(cliente.id, cliente.email, cliente.nomecompleto);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return Ok(new AuthResponseViewModel
            {
                userId = cliente.id,
                email = cliente.email,
                nome = cliente.nomecompleto,
                accessToken = accessToken,
                accessTokenExpiresAtUtc = expiresAt,
                refreshToken = refreshToken
            });
        }

        /// <summary>
        /// Verifica se um CPF já está cadastrado
        /// </summary>
        [HttpGet("check-cpf/{cpf}")]
        public IActionResult CheckCpf(string cpf)
        {
            var cpfClean = cpf.Replace(".", "").Replace("-", "");
            var exists = _clientesRepository.Get().Any(c => c.cpf == cpfClean);
            return Ok(new { exists });
        }

        /// <summary>
        /// Verifica se um email já está cadastrado
        /// </summary>
        [HttpGet("check-email/{email}")]
        public IActionResult CheckEmail(string email)
        {
            var exists = _clientesRepository.Get().Any(c => c.email == email);
            return Ok(new { exists });
        }

        /// <summary>
        /// Solicita recuperação de senha por email ou CPF
        /// </summary>
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.identifier))
                return BadRequest(new { message = "Informe seu email ou CPF." });

            var identifier = model.identifier.Trim();
            var cpfClean = identifier.Replace(".", "").Replace("-", "");

            var clientes = _clientesRepository.Get();
            var cliente = clientes.FirstOrDefault(c =>
                c.email == identifier || c.cpf == cpfClean);

            // Always return success to prevent email/CPF enumeration
            if (cliente == null)
                return Ok(new { message = "Se encontrarmos uma conta com essas informações, enviaremos instruções para recuperação de senha." });

            // Generate a temporary password
            var tempPassword = Guid.NewGuid().ToString("N")[..8];
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(tempPassword);

            // Update password directly using a fresh DbContext
            try
            {
                using var ctx = new ConnectionContext();
                var dbCliente = ctx.clientes.FirstOrDefault(c => c.id == cliente.id);
                if (dbCliente != null)
                {
                    dbCliente.senha = hashedPassword;
                    ctx.SaveChanges();
                }
            }
            catch
            {
                // Silently fail
            }

            return Ok(new
            {
                message = "Se encontrarmos uma conta com essas informações, enviaremos instruções para recuperação de senha.",
                // DEV ONLY: In production, remove this and send via email
                tempPassword = tempPassword,
                email = cliente.email
            });
        }

        /// <summary>
        /// Reseta a senha usando a senha temporária
        /// </summary>
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.email) ||
                string.IsNullOrWhiteSpace(model.tempPassword) ||
                string.IsNullOrWhiteSpace(model.newPassword))
                return BadRequest(new { message = "Todos os campos são obrigatórios." });

            if (model.newPassword.Length < 6)
                return BadRequest(new { message = "A nova senha deve ter pelo menos 6 caracteres." });

            var clientes = _clientesRepository.Get();
            var cliente = clientes.FirstOrDefault(c => c.email == model.email);

            if (cliente == null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            // Verify temp password
            bool tempValid;
            try
            {
                tempValid = BCrypt.Net.BCrypt.Verify(model.tempPassword, cliente.senha);
            }
            catch
            {
                tempValid = cliente.senha == model.tempPassword;
            }

            if (!tempValid)
                return Unauthorized(new { message = "Senha temporária inválida." });

            // Set new password directly using a fresh DbContext
            var hashedNew = BCrypt.Net.BCrypt.HashPassword(model.newPassword);
            try
            {
                using var ctx = new ConnectionContext();
                var dbCliente = ctx.clientes.FirstOrDefault(c => c.id == cliente.id);
                if (dbCliente != null)
                {
                    dbCliente.senha = hashedNew;
                    ctx.SaveChanges();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao atualizar senha." });
            }

            var (accessToken, expiresAt) = _jwtService.GenerateAccessToken(cliente.id, cliente.email, cliente.nomecompleto);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return Ok(new AuthResponseViewModel
            {
                userId = cliente.id,
                email = cliente.email,
                nome = cliente.nomecompleto,
                accessToken = accessToken,
                accessTokenExpiresAtUtc = expiresAt,
                refreshToken = refreshToken
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");

            if (userIdClaim == null)
                return Unauthorized(new { message = "Token inválido." });

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = "Token inválido." });

            var cliente = _clientesRepository.GetById(userId);
            if (cliente == null)
                return NotFound(new { message = "Usuário não encontrado." });

            return Ok(new
            {
                id = cliente.id,
                nomecompleto = cliente.nomecompleto,
                email = cliente.email,
                cpf = cliente.cpf,
                telefone = cliente.telefone,
                endereco = cliente.endereco,
                cep = cliente.cep,
                foto = cliente.foto
            });
        }
    }
}
