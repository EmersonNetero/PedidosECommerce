namespace PedidosECommerce.Application.DTO
{
    public class PedidoDetalheResponse
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public DateTime CriadoEm { get; set; }
        public string Status { get; set; }
        public string DadosPedido { get; set; }
        public string Historico{ get; set; } = string.Empty;
    }
}
