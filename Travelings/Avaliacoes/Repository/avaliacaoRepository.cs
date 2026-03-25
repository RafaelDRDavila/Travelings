using Microsoft.EntityFrameworkCore;
using Travelings.Model;

namespace Travelings.Infrastructure
{
    public class avaliacaoRepository : IavaliacaoRepository
    {
        private readonly ConnectionContext _connectionContext = new ConnectionContext();

        public void Add(avaliacao avaliacao)
        {
            _connectionContext.Add(avaliacao);
            _connectionContext.SaveChanges();
        }

        public List<avaliacao> Get()
        {
            return _connectionContext.avaliacoes.OrderByDescending(a => a.datacriacao).ToList();
        }

        public List<avaliacao> GetByProduto(int idproduto)
        {
            return _connectionContext.avaliacoes
                .Where(a => a.idproduto == idproduto)
                .OrderByDescending(a => a.datacriacao)
                .ToList();
        }

        public avaliacao? GetById(int id)
        {
            return _connectionContext.avaliacoes.FirstOrDefault(a => a.id == id);
        }

        public avaliacao? GetByClienteAndProduto(int idcliente, int idproduto)
        {
            return _connectionContext.avaliacoes.FirstOrDefault(a => a.idcliente == idcliente && a.idproduto == idproduto);
        }

        public void Update(avaliacao avaliacao)
        {
            _connectionContext.Set<avaliacao>().Update(avaliacao);
            _connectionContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _connectionContext.avaliacoes.Where(a => a.id == id).ExecuteDelete();
        }

        public double GetMediaByProduto(int idproduto)
        {
            var avaliacoes = _connectionContext.avaliacoes.Where(a => a.idproduto == idproduto);
            if (!avaliacoes.Any()) return 0;
            return avaliacoes.Average(a => a.nota);
        }

        public int GetCountByProduto(int idproduto)
        {
            return _connectionContext.avaliacoes.Count(a => a.idproduto == idproduto);
        }
    }
}
