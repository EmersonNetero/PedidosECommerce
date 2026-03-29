using PedidosECommerce.Domain.Entities;

namespace PedidosECommerce.Application.DTO
{
    public class PedidoDetalheResponse
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public DateTime CriadoEm { get; set; }
        public string Status { get; set; }
        public string DadosPedido { get; set; }
        public Guid CorrelationId {  get; set; }
        public List<PedidoHistoricoResponse> Historico { get; set; }
    }

    public class PedidoHistoricoResponse
    {
        public string Status { get; set; }
        public DateTime DataProcessamento { get; set; }
        public string? Erro { get; set; }
    }
}
