using Microsoft.EntityFrameworkCore;
using Travelings.Model;

namespace Travelings.Infrastructure
{
    public class lojasRepository : IlojasRepository
    {
        private readonly ConnectionContext _connectionContext = new ConnectionContext();
        public void Add(lojas lojas)
        {
            _connectionContext.Add(lojas);
            _connectionContext.SaveChanges();
        }

        public List<lojas> Get()
        {
            return _connectionContext.lojas.ToList();
        }
        public lojas GetById(int id)
        {
            return _connectionContext.lojas.FirstOrDefault(c => c.id == id);
        }

        public void Update(lojas lojas)
        {
            _connectionContext.Set<lojas>().Update(lojas);
            _connectionContext.SaveChanges();
        }
        public void Delete(int id)
        {
            _connectionContext.lojas.Where(c => c.id == id).ExecuteDeleteAsync();
        }
    }
}
