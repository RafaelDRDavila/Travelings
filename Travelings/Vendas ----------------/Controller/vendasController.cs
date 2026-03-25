using Microsoft.AspNetCore.Mvc;
using Travelings.Infrastructure;
using Travelings.Model;
using Travelings.Vendas_________________.ViewModel;
using Travelings.ViewModel;

namespace Travelings.Controllers
{

    [ApiController]
    [Route("api/v1/vendas")]
    public class vendasController : Controller
    {
        private readonly IvendasRepository _vendasRepository;

        public vendasController(IvendasRepository vendasRepository)
        {
            this._vendasRepository = vendasRepository ?? throw new ArgumentNullException(nameof(vendasRepository));
        }

        [HttpPost]
        public IActionResult Add(vendasViewModel produtosView)
        {
            var vendas = new vendas(produtosView.id, produtosView.idcliente, produtosView.idvendedor, produtosView.datavenda);
            _vendasRepository.Add(vendas);
            return base.Ok((object)vendas);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vendass = _vendasRepository.Get();
            return Ok(vendass);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var vendas = _vendasRepository.GetById(id);
            if (vendas == null)
            {

                return NotFound();


            }
            return Ok(vendas);
        }

        [HttpPut]
        public IActionResult Update(int id, vendasViewModel vendasView)
        {
            var vendas = _vendasRepository.GetById(id);
            if (vendas == null)
            {

                return NotFound();

            }

            vendas.idcliente = vendasView.idcliente;
            vendas.idvendedor = vendasView.idvendedor;
            vendas.datavenda = vendasView.datavenda;

            _vendasRepository.Update(vendas);
            return Ok();

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var vendas = _vendasRepository.GetById(id);
            if (id == null)
                return NotFound();
            _vendasRepository.Delete(id);
            return Ok();




        }

    }
}

