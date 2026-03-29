using PedidosECommerce.Domain.Enums;

namespace PedidosECommerce.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; private set; }
        public string NomeCliente { get; private set; }
        public string DadosPedido { get; private set; }
        public PedidoStatus Status { get; private set; }
        public Guid CorrelationId { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAtualizacao { get; private set; }
        public List<PedidoHistorico> Historico { get; private set; } = new();


        public Pedido(string nomeCliente, string dadosPedido)
        {
            DataCriacao = DateTime.UtcNow;
            Status = PedidoStatus.Recebido;
            CorrelationId = Guid.NewGuid();
            NomeCliente = nomeCliente;
            DadosPedido = dadosPedido;
        }

        public void Processar(PedidoStatus novoStatus, string? erro = null)
        {
            if(Status == PedidoStatus.Processado)
                throw new InvalidOperationException("Pedido já processado");
            Status = novoStatus;
            DataAtualizacao = DateTime.UtcNow;
            Historico.Add(new PedidoHistorico(novoStatus, erro));
        }
    }
}
