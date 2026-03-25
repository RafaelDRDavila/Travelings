using Microsoft.EntityFrameworkCore;
using Travelings.Model;

namespace Travelings.Infrastructure
{
    public class itensvendasRepository : IitensvendasRepository
    {
        private readonly ConnectionContext _connectionContext = new ConnectionContext();
        public void Add(itensvendas itensvendas)
        {
            _connectionContext.Add(itensvendas);
            _connectionContext.SaveChanges();
        }

        public List<itensvendas> Get()
        {
            return _connectionContext.itensvendas.ToList();
        }
        public itensvendas GetById(int id)
        {
            return _connectionContext.itensvendas.FirstOrDefault(c => c.id == id);
        }

        public void Update(itensvendas itensvendas)
        {
            _connectionContext.Set<itensvendas>().Update(itensvendas);
            _connectionContext.SaveChanges();
        }
        public void Delete(int id)
        {
            _connectionContext.itensvendas.Where(c => c.id == id).ExecuteDeleteAsync();
        }
    }
}
