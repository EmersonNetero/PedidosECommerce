namespace PedidosECommerce.Application.DTO
{
    public class PedidoResponse
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public DateTime CriadoEm { get; set; }
        public string Status { get; set; }
        public string DadosPedido { get; set; }
    }
}
