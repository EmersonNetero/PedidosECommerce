using PedidosECommerce.Domain.Enums;

namespace PedidosECommerce.Application.DTO
{
    public class PedidoRecebido
    {
        public PedidoStatus Status {  get; set; }
        public string NomeCliente { get; set; }
        public string DadosPedido { get; set; } = string.Empty;
    }
}
