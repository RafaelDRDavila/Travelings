using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travelings.Model;

namespace Travelings.Infrastructure
{
    public class clientesRepository : IclientesRepository
    {
        private readonly ConnectionContext _connectionContext = new ConnectionContext();
        public void Add(clientes clientes)
        {
            _connectionContext.Add(clientes);
            _connectionContext.SaveChanges();
        }

        public List<clientes> Get()
        {
            return _connectionContext.clientes.ToList();
        }

        public clientes GetById(int id)
        {
            return _connectionContext.clientes.FirstOrDefault(c => c.id == id);
        }

        public void Update(clientes clientes)
        {
            _connectionContext.Set<clientes>().Update(clientes);
            _connectionContext.SaveChanges();
        }
        public void Delete(int id)
        {
            _connectionContext.clientes.Where(c => c.id == id).ExecuteDeleteAsync();
        }
    }
}
