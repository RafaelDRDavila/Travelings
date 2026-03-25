using Microsoft.EntityFrameworkCore;
using Travelings.Model;

namespace Travelings.Infrastructure
{
    public class produtosRepository : IprodutosRepository
    {
        private readonly ConnectionContext _connectionContext = new ConnectionContext();
        public void Add(produtos produtos)
        {
            _connectionContext.Add(produtos);
            _connectionContext.SaveChanges();
        }

        public List<produtos> Get()
        {
            return _connectionContext.produtos.ToList();
        }
        public produtos GetById(int id)
        {
            return _connectionContext.produtos.FirstOrDefault(c => c.id == id);
        }

        public void Update(produtos produtos)
        {
            _connectionContext.Set<produtos>().Update(produtos);
            _connectionContext.SaveChanges();
        }
        public void Delete(int id)
        {
            _connectionContext.produtos.Where(c => c.id == id).ExecuteDeleteAsync();
        }
    }
}
