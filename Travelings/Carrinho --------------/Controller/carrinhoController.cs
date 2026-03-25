using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travelings.Carrinho_______________.ViewModel;
using Travelings.Infrastructure;
using Travelings.Model;

namespace Travelings.Controllers
{

    [ApiController]
    [Route("api/v1/carrinho")]
    public class carrinhoController : Controller
    {
        private readonly IcarrinhoRepository _carrinhoRepository;
        private readonly IprodutosRepository _produtosRepository;

        public carrinhoController(IcarrinhoRepository carrinhoRepository, IprodutosRepository produtosRepository)
        {
            this._carrinhoRepository = carrinhoRepository ?? throw new ArgumentNullException(nameof(carrinhoRepository));
            this._produtosRepository = produtosRepository ?? throw new ArgumentNullException(nameof(produtosRepository));
        }

        private int? GetAuthUserId()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");
            if (claim != null && int.TryParse(claim.Value, out var id)) return id;
            return null;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(carrinhoViewModel carrinhoView)
        {
            var userId = GetAuthUserId();
            if (userId == null) return Unauthorized();

            // Force idcliente to authenticated user
            carrinhoView.idcliente = userId.Value;

            if (carrinhoView.quantidade <= 0)
                return BadRequest(new { message = "Quantidade deve ser positiva." });

            // Validar datas de aluguel
            if (carrinhoView.modalidade == "aluguel")
            {
                if (carrinhoView.datainicio == null || carrinhoView.datafim == null)
                    return BadRequest(new { message = "Datas de inicio e fim sao obrigatorias para aluguel." });
                if (carrinhoView.datafim <= carrinhoView.datainicio)
                    return BadRequest(new { message = "Data fim deve ser posterior a data inicio." });
                if (carrinhoView.datainicio.Value.Date < DateTime.Today)
                    return BadRequest(new { message = "Data inicio nao pode ser no passado." });

                // Validate min/max days from product
                var produto = _produtosRepository.GetById(carrinhoView.idproduto);
                if (produto != null)
                {
                    var days = (int)Math.Ceiling((carrinhoView.datafim.Value - carrinhoView.datainicio.Value).TotalDays);
                    if (produto.diasminimo.HasValue && days < produto.diasminimo.Value)
                        return BadRequest(new { message = $"Período mínimo de aluguel: {produto.diasminimo} dias." });
                    if (produto.diasmaximo.HasValue && days > produto.diasmaximo.Value)
                        return BadRequest(new { message = $"Período máximo de aluguel: {produto.diasmaximo} dias." });
                }
            }

            // Check if item already exists for this client+product+modalidade
            var existing = _carrinhoRepository.GetByCliente(userId.Value)
                .FirstOrDefault(c => c.idproduto == carrinhoView.idproduto && c.modalidade == carrinhoView.modalidade);

            if (existing != null && carrinhoView.modalidade == "compra")
            {
                existing.quantidade += carrinhoView.quantidade;
                _carrinhoRepository.Update(existing);
                return Ok(existing);
            }

            var carrinho = new carrinho(0, carrinhoView.idproduto, userId.Value, carrinhoView.quantidade, carrinhoView.modalidade, carrinhoView.datainicio, carrinhoView.datafim);
            _carrinhoRepository.Add(carrinho);
            return Created("", carrinho);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get([FromQuery] int? idcliente)
        {
            var userId = GetAuthUserId();
            if (userId == null) return Unauthorized();

            // Always filter by authenticated user
            var items = _carrinhoRepository.GetByCliente(userId.Value);
            return Ok(items);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var userId = GetAuthUserId();
            if (userId == null) return Unauthorized();

            var carrinho = _carrinhoRepository.GetById(id);
            if (carrinho == null || carrinho.idcliente != userId.Value)
                return NotFound();
            return Ok(carrinho);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, carrinhoViewModel carrinhoView)
        {
            var userId = GetAuthUserId();
            if (userId == null) return Unauthorized();

            var carrinho = _carrinhoRepository.GetById(id);
            if (carrinho == null || carrinho.idcliente != userId.Value)
                return NotFound();

            if (carrinhoView.quantidade <= 0)
            {
                _carrinhoRepository.Delete(id);
                return Ok();
            }

            carrinho.quantidade = carrinhoView.quantidade;
            _carrinhoRepository.Update(carrinho);
            return Ok(carrinho);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = GetAuthUserId();
            if (userId == null) return Unauthorized();

            var carrinho = _carrinhoRepository.GetById(id);
            if (carrinho == null || carrinho.idcliente != userId.Value)
                return NotFound();

            _carrinhoRepository.Delete(id);
            return Ok();
        }
    }
}
