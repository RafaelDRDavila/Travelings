using Microsoft.AspNetCore.Mvc;
using Travelings.Infrastructure;
using Travelings.Model;
using Travelings.Produtos_______________.ViewModel;
using Travelings.ViewModel;

namespace Travelings.Controllers
{

    [ApiController]
    [Route("api/v1/produtos")]
    public class produtosController : Controller
    {
        private readonly IprodutosRepository _produtosRepository;

        public produtosController(IprodutosRepository produtosRepository)
        {
            this._produtosRepository = produtosRepository ?? throw new ArgumentNullException(nameof(produtosRepository));
        }

        [HttpPost]
        public IActionResult Add(produtosViewModel produtosView)
        {
            var produtos = new produtos(
                produtosView.id, produtosView.nome, produtosView.preco, produtosView.descricao,
                produtosView.estoque, produtosView.categoria, produtosView.imagem, produtosView.idloja,
                produtosView.ativo, produtosView.subcategoria, produtosView.midias,
                produtosView.tipo, produtosView.precoaluguel,
                diasminimo: produtosView.diasminimo, diasmaximo: produtosView.diasmaximo, quantidadedisponivel: produtosView.quantidadedisponivel
            );
            _produtosRepository.Add(produtos);
            return base.Ok((object)produtos);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var produtoss = _produtosRepository.Get()
                .Where(p => p.ativo)
                .ToList();
            return Ok(produtoss);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var produtos = _produtosRepository.GetById(id);
            if (produtos == null)
            {
                return NotFound();
            }
            return Ok(produtos);
        }

        [HttpGet]
        [Route("loja/{idloja}")]
        public IActionResult GetByLoja(int idloja)
        {
            var produtos = _produtosRepository.Get()
                .Where(p => p.idloja == idloja && p.ativo)
                .OrderByDescending(p => p.id)
                .ToList();
            return Ok(produtos);
        }

        [HttpPut]
        public IActionResult Update(int id, produtosViewModel produtosView)
        {
            var produtos = _produtosRepository.GetById(id);
            if (produtos == null)
            {

                return NotFound();

            }

            produtos.nome = produtosView.nome;
            produtos.preco = produtosView.preco;
            produtos.descricao = produtosView.descricao;
            produtos.estoque = produtosView.estoque;
            produtos.categoria = produtosView.categoria;
            produtos.subcategoria = produtosView.subcategoria;
            produtos.imagem = produtosView.imagem;
            produtos.midias = produtosView.midias;
            produtos.idloja = produtosView.idloja;
            produtos.ativo = produtosView.ativo;
            produtos.tipo = produtosView.tipo;
            produtos.precoaluguel = produtosView.precoaluguel;
            produtos.diasminimo = produtosView.diasminimo;
            produtos.diasmaximo = produtosView.diasmaximo;
            produtos.quantidadedisponivel = produtosView.quantidadedisponivel;

            _produtosRepository.Update(produtos);
            return Ok();

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var produtos = _produtosRepository.GetById(id);
            if (produtos == null)
                return NotFound();
            _produtosRepository.Delete(id);
            return Ok();




        }
    }
}

