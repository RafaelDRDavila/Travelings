using Microsoft.EntityFrameworkCore;
using Travelings.Model;

namespace Travelings.Infrastructure
{
    public class comentariosRepository : IcomentariosRepository
    {
        private readonly ConnectionContext _connectionContext = new ConnectionContext();
        public void Add(comentarios comentarios)
        {
            _connectionContext.Add(comentarios);
            _connectionContext.SaveChanges();
        }

        public List<comentarios> Get()
        {
            return _connectionContext.comentarios.ToList();
        }
        public comentarios GetById(int id)
        {
            return _connectionContext.comentarios.FirstOrDefault(c => c.id == id);
        }

        public void Update(comentarios comentarios)
        {
            _connectionContext.Set<comentarios>().Update(comentarios);
            _connectionContext.SaveChanges();
        }
        public void Delete(int id)
        {
            _connectionContext.comentarios.Where(c => c.id == id).ExecuteDeleteAsync();
        }
    }
}
