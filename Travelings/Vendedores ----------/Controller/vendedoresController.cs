using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Travelings.Infrastructure;
using Travelings.Model;
using Travelings.ViewModel;

namespace Travelings.Vendedores
{

    [ApiController]
    [Route("api/v1/vendedores")]
    public class vendedoresController : Controller
    {
        private readonly IvendedoresRepository _vendedoresRepository;

        public vendedoresController(IvendedoresRepository vendedoresRepository)
        {
            this._vendedoresRepository = vendedoresRepository ?? throw new ArgumentNullException(nameof(vendedoresRepository));
        }

        [HttpPost]
        public IActionResult Add(vendedoresViewModel vendedoresView)
        {
            var vendedores = new vendedores(vendedoresView.id, vendedoresView.nomecompleto, vendedoresView.email, vendedoresView.senha, vendedoresView.cpf, vendedoresView.idloja);
            _vendedoresRepository.Add(vendedores);
            return Ok(vendedores);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vendedoress = _vendedoresRepository.Get();
            return Ok(vendedoress);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var vendedores = _vendedoresRepository.GetById(id);
            if (vendedores == null)
            {

                return NotFound();


            }
            return Ok(vendedores);
        }

        [HttpPut]
        public IActionResult Update(int id, vendedoresViewModel vendedoresView)
        {
            var vendedores = _vendedoresRepository.GetById(id);
            if (vendedores == null)
            {

                return NotFound();

            }

            vendedores.nomecompleto = vendedoresView.nomecompleto;
            vendedores.email = vendedoresView.email;
            vendedores.cpf = vendedoresView.cpf;
            vendedores.idloja = vendedoresView.idloja;
            vendedores.senha = vendedoresView.senha;

            _vendedoresRepository.Update(vendedores);
            return Ok();

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var vendedores = _vendedoresRepository.GetById(id);
            if (id == null)
                return NotFound();
            _vendedoresRepository.Delete(id);
            return Ok();




        }
        
    }
}
