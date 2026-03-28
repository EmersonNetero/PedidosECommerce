using Microsoft.EntityFrameworkCore;
using PedidosECommerce.Domain.Entities;

namespace PedidosECommerce.Infrastructure.Contexts
{
    public class SqlServer : DbContext
    {

        public DbSet<Pedido> Pedidos { get; set; }
        public SqlServer(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Status)
                .HasConversion<string>();
        }
    }
}
