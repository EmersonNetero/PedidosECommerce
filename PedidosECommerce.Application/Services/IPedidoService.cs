using PedidosECommerce.Application.DTO;

namespace PedidosECommerce.Application.Services
{
    public interface IPedidoService
    {
        Task<PedidoRecebido> ReceberPedido(ReceberPedidoDTO pedido);
        Task<PagedResult<PedidoResponse>> GetAsync(PedidoFiltroRequest request);
        Task<PedidoDetalheResponse> GetDetalhe(int id);
        Task ProcessarPedido(int id, Guid correlationId);
        Task Reprocessar(int id);

    }
}
