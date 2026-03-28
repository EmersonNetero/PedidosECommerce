using PedidosECommerce.Application.Abstractions;
using PedidosECommerce.Domain.Entities;
using PedidosECommerce.Infrastructure.Contexts;

namespace PedidosECommerce.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly SqlServer _context;

        public PedidoRepository(SqlServer context)
        {
            _context = context;
        }

        public async Task ReceberPedidoAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
