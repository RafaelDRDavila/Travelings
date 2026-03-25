using Microsoft.AspNetCore.Mvc;
using Travelings.Infrastructure;
using Travelings.Model;
using Travelings.ViewModel;

namespace Travelings.Controllers
{

    [ApiController]
    [Route("api/v1/lojas")]
    public class lojasController : Controller
    {
        private readonly IlojasRepository _lojasRepository;
        private readonly IprodutosRepository _produtosRepository;

        public lojasController(IlojasRepository lojasRepository, IprodutosRepository produtosRepository)
        {
            this._lojasRepository = lojasRepository ?? throw new ArgumentNullException(nameof(lojasRepository));
            this._produtosRepository = produtosRepository ?? throw new ArgumentNullException(nameof(produtosRepository));
        }

        [HttpPost]
        public IActionResult Add(lojasViewModel lojasView)
        {
            var lojas = new lojas(lojasView.id, lojasView.nome, lojasView.cnpj, lojasView.descricao, lojasView.logo, lojasView.banner, lojasView.email, lojasView.telefone, lojasView.endereco);
            _lojasRepository.Add(lojas);
            return base.Ok((object)lojas);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var lojass = _lojasRepository.Get();
            return Ok(lojass);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var lojas = _lojasRepository.GetById(id);
            if (lojas == null)
            {
                return NotFound();
            }
            return Ok(lojas);
        }

        [HttpGet]
        [Route("{id}/produtos")]
        public IActionResult GetProdutosByLoja(int id)
        {
            var loja = _lojasRepository.GetById(id);
            if (loja == null)
                return NotFound(new { message = "Loja não encontrada." });

            var produtos = _produtosRepository.Get()
                .Where(p => p.idloja == id && p.ativo)
                .OrderByDescending(p => p.id)
                .ToList();

            return Ok(produtos);
        }

        [HttpPut]
        public IActionResult Update(int id, lojasViewModel lojasView)
        {
            var lojas = _lojasRepository.GetById(id);
            if (lojas == null)
            {
                return NotFound();
            }

            lojas.nome = lojasView.nome;
            lojas.cnpj = lojasView.cnpj;
            lojas.descricao = lojasView.descricao;
            lojas.logo = lojasView.logo;
            lojas.banner = lojasView.banner;
            lojas.email = lojasView.email;
            lojas.telefone = lojasView.telefone;
            lojas.endereco = lojasView.endereco;

            _lojasRepository.Update(lojas);
            return Ok(lojas);
        }

        [HttpGet]
        [Route("check-cnpj/{cnpj}")]
        public IActionResult CheckCnpj(string cnpj, [FromQuery] int? excludeId)
        {
            var digits = new string(cnpj.Where(char.IsDigit).ToArray());
            if (!ValidateCnpj(digits))
                return Ok(new { valid = false, available = false, message = "CNPJ inválido." });

            var lojas = _lojasRepository.Get();
            var existing = lojas.FirstOrDefault(l =>
                new string(l.cnpj.Where(char.IsDigit).ToArray()) == digits);

            if (existing != null && existing.id != excludeId)
                return Ok(new { valid = true, available = false, message = "CNPJ já está em uso por outra loja." });

            return Ok(new { valid = true, available = true });
        }

        private static bool ValidateCnpj(string digits)
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

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var lojas = _lojasRepository.GetById(id);
            if (lojas == null)
                return NotFound();
            _lojasRepository.Delete(id);
            return Ok();
        }
    }
}
