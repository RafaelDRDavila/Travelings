using Microsoft.AspNetCore.Mvc;
using Travelings.Infrastructure;
using Travelings.ItensVendas____________.ViewModel;
using Travelings.Model;
using Travelings.Vendas_________________.ViewModel;
using Travelings.ViewModel;

namespace Travelings.Controllers
{

    [ApiController]
    [Route("api/v1/itensvendas")]
    public class itensvendasController : Controller
    {
        private readonly IitensvendasRepository _itensvendasRepository;

        public itensvendasController(IitensvendasRepository itensvendasRepository)
        {
            this._itensvendasRepository = itensvendasRepository ?? throw new ArgumentNullException(nameof(itensvendasRepository));
        }

        [HttpPost]
        public IActionResult Add(itensvendasViewModel itensvendasView)
        {
            var itensvendas = new itensvendas(itensvendasView.id, itensvendasView.idvenda, itensvendasView.idproduto, itensvendasView.quantidade, itensvendasView.precounitario);
            _itensvendasRepository.Add(itensvendas);
            return base.Ok((object)itensvendas);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var itensvendass = _itensvendasRepository.Get();
            return Ok(itensvendass);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var itensvendas = _itensvendasRepository.GetById(id);
            if (itensvendas == null)
            {

                return NotFound();


            }
            return Ok(itensvendas);
        }

        [HttpPut]
        public IActionResult Update(int id, itensvendasViewModel itensvendasView)
        {
            var itensvendas = _itensvendasRepository.GetById(id);
            if (itensvendas == null)
            {

                return NotFound();

            }

            itensvendas.idvenda = itensvendasView.idvenda;
            itensvendas.idproduto = itensvendasView.idproduto;
            itensvendas.quantidade = itensvendasView.quantidade;
            itensvendas.precounitario = itensvendasView.precounitario;

            _itensvendasRepository.Update(itensvendas);
            return Ok();

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var itensvendas = _itensvendasRepository.GetById(id);
            if (id == null)
                return NotFound();
            _itensvendasRepository.Delete(id);
            return Ok();




        }
    }
}

