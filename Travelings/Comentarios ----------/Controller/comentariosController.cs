using Microsoft.AspNetCore.Mvc;
using Travelings.Carrinho_______________.ViewModel;
using Travelings.Infrastructure;
using Travelings.Model;
using Travelings.Produtos_______________.ViewModel;
using Travelings.ViewModel;

namespace Travelings.Controllers
{

    [ApiController]
    [Route("api/v1/comentarios")]
    public class comentariosController : Controller
    {
        private readonly IcomentariosRepository _comentariosRepository;

        public comentariosController(IcomentariosRepository comentariosRepository)
        {
            this._comentariosRepository = comentariosRepository ?? throw new ArgumentNullException(nameof(comentariosRepository));
        }

        [HttpPost]
        public IActionResult Add(comentariosViewModel comentariosView)
        {
            var comentarios = new comentarios(comentariosView.id, comentariosView.idcliente, comentariosView.corpotexto);
            _comentariosRepository.Add(comentarios);
            return base.Ok((object)comentarios);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var comentarioss = _comentariosRepository.Get();
            return Ok(comentarioss);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var comentario = _comentariosRepository.GetById(id);
            if (comentario == null)
            {

                return NotFound();


            }
            return Ok(comentario);
        }

        [HttpPut]
        public IActionResult Update(int id, comentariosViewModel comentarioView)
        {
            var comentario = _comentariosRepository.GetById(id);
            if (comentario == null)
            {

                return NotFound();

            }

            comentario.idcliente = comentarioView.idcliente;
            comentario.corpotexto = comentarioView.corpotexto;

            _comentariosRepository.Update(comentario);
            return Ok();

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var comentario = _comentariosRepository.GetById(id);
            if (id == null)
                return NotFound();
            _comentariosRepository.Delete(id);
            return Ok();




        }
    }
}

