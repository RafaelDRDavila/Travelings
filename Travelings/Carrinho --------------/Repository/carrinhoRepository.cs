using Microsoft.EntityFrameworkCore;
using Travelings.Model;

namespace Travelings.Infrastructure
{
    public class carrinhoRepository : IcarrinhoRepository
    {
        private readonly ConnectionContext _connectionContext = new ConnectionContext();
        public void Add(carrinho carrinho)
        {
            _connectionContext.Add(carrinho);
            _connectionContext.SaveChanges();
        }

        public List<carrinho> Get()
        {
            return _connectionContext.carrinho.ToList();
        }

        public List<carrinho> GetByCliente(int idcliente)
        {
            return _connectionContext.carrinho.Where(c => c.idcliente == idcliente).ToList();
        }

        public carrinho GetById(int id)
        {
            return _connectionContext.carrinho.FirstOrDefault(c => c.id == id);
        }

        public void Update(carrinho carrinho)
        {
            _connectionContext.Set<carrinho>().Update(carrinho);
            _connectionContext.SaveChanges();
        }
        public void Delete(int id)
        {
            _connectionContext.carrinho.Where(c => c.id == id).ExecuteDelete();
        }
    }
}
