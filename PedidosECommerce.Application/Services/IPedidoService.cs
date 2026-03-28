using PedidosECommerce.Application.DTO;

namespace PedidosECommerce.Application.Services
{
    public interface IPedidoService
    {
        Task<PedidoRecebido> ReceberPedido(ReceberPedidoDTO pedido);
    }
}
