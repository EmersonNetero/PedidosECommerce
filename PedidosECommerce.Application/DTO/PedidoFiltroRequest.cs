using PedidosECommerce.Domain.Enums;

namespace PedidosECommerce.Application.DTO
{
    public class PedidoFiltroRequest
    {
        public PedidoStatus? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Ordenacao Order{ get; set; } = Ordenacao.Desc;

    }

    public enum Ordenacao
    {
        Asc,
        Desc
    }
}
