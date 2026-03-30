using MassTransit;
using Microsoft.Extensions.Logging;
using PedidosECommerce.Application.Services;
using PedidosECommerce.Domain;

namespace PedidosECommerce.Messaging
{
    public class PedidoConsumer : IConsumer<PedidoCriadoEvent>
    {
        private readonly IPedidoService _pedidoService;
        private readonly ILogger<PedidoConsumer> _logger;
        public PedidoConsumer(IPedidoService pedidoService, ILogger<PedidoConsumer> logger)
        {
            _pedidoService = pedidoService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PedidoCriadoEvent> context)
        {
            _logger.LogInformation("Consumindo mensagem: {CorrelationId}", context.CorrelationId);
            await _pedidoService.ProcessarPedido(context.Message.Id,context.CorrelationId ?? Guid.NewGuid());
        }
    }
}
