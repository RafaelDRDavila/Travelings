using Microsoft.EntityFrameworkCore;
using Travelings.Infrastructure;
using Travelings.Model;

namespace Travelings.Vendedores
{
    public class vendedoresRepository : IvendedoresRepository
    {
        private readonly ConnectionContext _connectionContext = new ConnectionContext();
        public void Add(vendedores vendedores)
        {
            _connectionContext.Add(vendedores);
            _connectionContext.SaveChanges();
        }

        public List<vendedores> Get()
        {
            return _connectionContext.vendedores.ToList();
        }

        public vendedores GetById(int id)
        {
            return _connectionContext.vendedores.FirstOrDefault(c => c.id == id);
        }

        public void Update(vendedores vendedores)
        {
            _connectionContext.Set<vendedores>().Update(vendedores);
            _connectionContext.SaveChanges();
        }
        public void Delete(int id)
        {
            _connectionContext.vendedores.Where(c => c.id == id).ExecuteDeleteAsync();
        }
    }
}
