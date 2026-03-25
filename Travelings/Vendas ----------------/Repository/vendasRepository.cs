using Microsoft.EntityFrameworkCore;
using Travelings.Model;

namespace Travelings.Infrastructure
{
    public class vendasRepository : IvendasRepository
    {
        private readonly ConnectionContext _connectionContext = new ConnectionContext();
        public void Add(vendas vendas)
        {
            _connectionContext.Add(vendas);
            _connectionContext.SaveChanges();
        }

        public List<vendas> Get()
        {
            return _connectionContext.vendas.ToList();
        }
        public vendas GetById(int id)
        {
            return _connectionContext.vendas.FirstOrDefault(c => c.id == id);
        }

        public void Update(vendas vendas)
        {
            _connectionContext.Set<vendas>().Update(vendas);
            _connectionContext.SaveChanges();
        }
        public void Delete(int id)
        {
            _connectionContext.vendas.Where(c => c.id == id).ExecuteDeleteAsync();
        }
    }
}
