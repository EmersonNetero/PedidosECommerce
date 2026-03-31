using PedidosECommerce.Application.DTO;
using PedidosECommerce.Domain.Entities;
using PedidosECommerce.Domain.Enums;

namespace PedidosECommerce.Application.Abstractions
{
    public interface IPedidoRepository
    {
        Task ReceberPedidoAsync(Pedido pedido);

        Task<PagedResult<Pedido>> GetAsync(
        PedidoStatus? status,
        int page,
        int pageSize,
        Ordenacao order);

        Task<Pedido> GetOneAsync(int id);
        Task SaveChangesAsync();

        Task<bool> MarcarComoReprocessado(int id);
    }
}
