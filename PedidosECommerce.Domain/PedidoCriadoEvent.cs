using PedidosECommerce.Domain.Enums;

namespace PedidosECommerce.Domain
{
    public class PedidoCriadoEvent
    {
        public Guid CorrelationId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public string DadosPedido { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public int Id { get; set; }
        public PedidoStatus Status { get; set; }
    }
}
