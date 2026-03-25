using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travelings.Avaliacoes.ViewModel;
using Travelings.Infrastructure;
using Travelings.Model;

namespace Travelings.Controllers
{
    [ApiController]
    [Route("api/v1/avaliacoes")]
    public class avaliacaoController : Controller
    {
        private readonly IavaliacaoRepository _avaliacaoRepository;
        private readonly IclientesRepository _clientesRepository;

        public avaliacaoController(IavaliacaoRepository avaliacaoRepository, IclientesRepository clientesRepository)
        {
            this._avaliacaoRepository = avaliacaoRepository ?? throw new ArgumentNullException(nameof(avaliacaoRepository));
            this._clientesRepository = clientesRepository ?? throw new ArgumentNullException(nameof(clientesRepository));
        }

        private int? GetAuthUserId()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");
            if (claim != null && int.TryParse(claim.Value, out var id)) return id;
            return null;
        }

        // GET /api/v1/avaliacoes/produto/{idproduto}
        [HttpGet("produto/{idproduto}")]
        public IActionResult GetByProduto(int idproduto)
        {
            var avaliacoes = _avaliacaoRepository.GetByProduto(idproduto);
            var media = _avaliacaoRepository.GetMediaByProduto(idproduto);
            var total = _avaliacaoRepository.GetCountByProduto(idproduto);

            return Ok(new
            {
                avaliacoes,
                media = Math.Round(media, 1),
                total
            });
        }

        // GET /api/v1/avaliacoes/media/{idproduto}
        [HttpGet("media/{idproduto}")]
        public IActionResult GetMedia(int idproduto)
        {
            var media = _avaliacaoRepository.GetMediaByProduto(idproduto);
            var total = _avaliacaoRepository.GetCountByProduto(idproduto);

            return Ok(new
            {
                media = Math.Round(media, 1),
                total
            });
        }

        // POST /api/v1/avaliacoes
        [HttpPost]
        [Authorize]
        public IActionResult Add(avaliacaoViewModel view)
        {
            var userId = GetAuthUserId();
            if (userId == null) return Unauthorized();

            // Validate nota
            if (view.nota < 0 || view.nota > 5)
                return BadRequest(new { message = "Nota deve ser entre 0 e 5." });

            if (string.IsNullOrWhiteSpace(view.texto))
                return BadRequest(new { message = "Texto é obrigatório." });

            // Check if user already reviewed this product
            var existing = _avaliacaoRepository.GetByClienteAndProduto(userId.Value, view.idproduto);
            if (existing != null)
                return BadRequest(new { message = "Você já avaliou este produto." });

            // Get client info for denormalized fields
            var cliente = _clientesRepository.GetById(userId.Value);
            var nomeCliente = cliente?.nomecompleto ?? "Usuário";
            var fotoCliente = cliente?.foto;

            var avaliacao = new avaliacao(
                0,
                view.idproduto,
                userId.Value,
                view.nota,
                view.texto,
                view.midias,
                nomeCliente,
                fotoCliente,
                DateTime.UtcNow
            );

            _avaliacaoRepository.Add(avaliacao);
            return Created("", avaliacao);
        }

        // PUT /api/v1/avaliacoes/{id}
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, avaliacaoViewModel view)
        {
            var userId = GetAuthUserId();
            if (userId == null) return Unauthorized();

            var avaliacao = _avaliacaoRepository.GetById(id);
            if (avaliacao == null || avaliacao.idcliente != userId.Value)
                return NotFound();

            if (view.nota < 0 || view.nota > 5)
                return BadRequest(new { message = "Nota deve ser entre 0 e 5." });

            avaliacao.nota = view.nota;
            avaliacao.texto = view.texto;
            avaliacao.midias = view.midias;

            _avaliacaoRepository.Update(avaliacao);
            return Ok(avaliacao);
        }

        // DELETE /api/v1/avaliacoes/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = GetAuthUserId();
            if (userId == null) return Unauthorized();

            var avaliacao = _avaliacaoRepository.GetById(id);
            if (avaliacao == null || avaliacao.idcliente != userId.Value)
                return NotFound();

            _avaliacaoRepository.Delete(id);
            return Ok();
        }
    }
}
