using PedidosECommerce.Domain.Enums;

namespace PedidosECommerce.Domain.Entities
{
    public class PedidoHistorico
    {
        public int Id { get; private set; }
        public int PedidoId { get; private set; }
        public PedidoStatus Status { get; private set; }
        public DateTime DataProcessamento { get; private set; }
        public string? Erro { get; private set; }

        public PedidoHistorico(PedidoStatus status, string? erro = null)
        {
            Status = status;
            DataProcessamento = DateTime.UtcNow;
            Erro = erro;
        }
    }
}
