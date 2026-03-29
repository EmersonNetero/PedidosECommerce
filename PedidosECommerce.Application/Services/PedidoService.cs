using PedidosECommerce.Application.Abstractions;
using PedidosECommerce.Application.DTO;
using PedidosECommerce.Domain.Entities;

namespace PedidosECommerce.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PagedResult<PedidoResponse>> GetAsync(PedidoFiltroRequest request)
        {
            var result = await _pedidoRepository.GetAsync(
                request.Status,
                request.Page,
                request.PageSize,
                request.Order);

            return new PagedResult<PedidoResponse>
            {
                Total = result.Total,
                Items = result.Items.Select(p => new PedidoResponse
                {
                    Id = p.Id,
                    NomeCliente = p.NomeCliente,
                    CriadoEm = p.DataCriacao,
                    Status = p.Status.ToString(),
                    DadosPedido = p.DadosPedido
                })
            };
        }

        public async Task<PedidoRecebido> ReceberPedido(ReceberPedidoDTO pedido)
        {
            var novoPedido = new Pedido(pedido.NomeCliente, pedido.DadosPedido);

            await _pedidoRepository.ReceberPedidoAsync(novoPedido);
            var pedidoRecebido = new PedidoRecebido()
            {
                DadosPedido = novoPedido.DadosPedido,
                NomeCliente = novoPedido.NomeCliente,
                Status = novoPedido.Status.ToString()
            };

            return pedidoRecebido;
        }


       
    }
}
