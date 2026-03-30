using PedidosECommerce.Domain.Entities;

namespace PedidosECommerce.Application.Abstractions
{
    public interface IMongoAuditRepository
    {
        Task RegistrarAsync(PedidoAuditLog log);
    }
}
