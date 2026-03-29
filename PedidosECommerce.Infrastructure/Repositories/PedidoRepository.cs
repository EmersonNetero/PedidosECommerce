using Microsoft.EntityFrameworkCore;
using PedidosECommerce.Application.Abstractions;
using PedidosECommerce.Application.DTO;
using PedidosECommerce.Domain.Entities;
using PedidosECommerce.Domain.Enums;
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

        public async Task<PagedResult<Pedido>> GetAsync(
          PedidoStatus? status,
          int page,
          int pageSize, Ordenacao order)
        {
            var query = _context.Pedidos.AsQueryable();

            if (status.HasValue)
                query = query.Where(p => p.Status == status);

            query = order == Ordenacao.Asc
               ? query.OrderBy(p => p.DataCriacao)
               : query.OrderByDescending(p => p.DataCriacao);


            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Pedido>
            {
                Total = total,
                Items = items
            };
        }
        public async Task ReceberPedidoAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
