using PedidosECommerce.Domain.Entities;

namespace PedidosECommerce.Application.Abstractions
{
    public interface IPedidoRepository
    {
        Task ReceberPedidoAsync(Pedido pedido);
    }
}
