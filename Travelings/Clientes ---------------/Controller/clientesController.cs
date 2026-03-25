using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travelings.Infrastructure;
using Travelings.Model;
using Travelings.ViewModel;

namespace Travelings.Controllers
{

    [ApiController]
    [Route("api/v1/clientes")]
    public class clientesController : Controller
    {
        private readonly IclientesRepository _clientesRepository;

        public clientesController(IclientesRepository clientesRepository)
        {
            this._clientesRepository = clientesRepository ?? throw new ArgumentNullException(nameof(clientesRepository));
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var clientes = _clientesRepository.GetById(id);
            if (clientes == null)
                return NotFound();

            // Never expose password
            return Ok(new
            {
                id = clientes.id,
                nomecompleto = clientes.nomecompleto,
                email = clientes.email,
                cpf = clientes.cpf,
                telefone = clientes.telefone,
                endereco = clientes.endereco,
                cep = clientes.cep,
                foto = clientes.foto
            });
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, clientesViewModel clientesView)
        {
            // Verify the authenticated user is updating their own profile
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");
            if (claim == null || !int.TryParse(claim.Value, out var authId) || authId != id)
                return Forbid();

            var clientes = _clientesRepository.GetById(id);
            if (clientes == null)
                return NotFound();

            clientes.nomecompleto = clientesView.nomecompleto;
            clientes.email = clientesView.email;
            clientes.cep = clientesView.cep;
            clientes.telefone = clientesView.telefone;
            clientes.endereco = clientesView.endereco;
            clientes.foto = clientesView.foto;

            // Only update password if provided, and hash it
            if (!string.IsNullOrEmpty(clientesView.senha))
                clientes.senha = BCrypt.Net.BCrypt.HashPassword(clientesView.senha);

            _clientesRepository.Update(clientes);

            return Ok(new
            {
                id = clientes.id,
                nomecompleto = clientes.nomecompleto,
                email = clientes.email,
                cpf = clientes.cpf,
                telefone = clientes.telefone,
                endereco = clientes.endereco,
                cep = clientes.cep,
                foto = clientes.foto
            });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");
            if (claim == null || !int.TryParse(claim.Value, out var authId) || authId != id)
                return Forbid();

            var clientes = _clientesRepository.GetById(id);
            if (clientes == null)
                return NotFound();
            _clientesRepository.Delete(id);
            return Ok();
        }

    }
}
