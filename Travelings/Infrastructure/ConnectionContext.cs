using Microsoft.EntityFrameworkCore;
using Travelings.Model;

namespace Travelings.Infrastructure
{
    public class ConnectionContext : DbContext
    {
        

        public DbSet<vendedores> vendedores { get; set; }
        public DbSet<lojas> lojas { get; set; }
        public DbSet<clientes> clientes { get; set; }

        public DbSet<produtos> produtos { get; set; }

        public DbSet<vendas> vendas { get; set; }

        public DbSet<itensvendas> itensvendas { get; set; }

        public DbSet<carrinho> carrinho { get; set; }

        public DbSet<comentarios> comentarios { get; set; }

        public DbSet<Travelings.Controllers.Favorito> favoritos { get; set; }

        public DbSet<avaliacao> avaliacoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           => optionsBuilder.UseNpgsql(
            "Server=localhost; " +
            "Port=5432;Database=Web;" +
            "User Id=postgres;" +
            "Password=Admin;");




    }
}
