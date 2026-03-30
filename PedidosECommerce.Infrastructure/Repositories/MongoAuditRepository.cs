using MongoDB.Driver;
using PedidosECommerce.Application.Abstractions;
using PedidosECommerce.Domain.Entities;

namespace PedidosECommerce.Infrastructure.Repositories
{
    public class MongoAuditRepository : IMongoAuditRepository
    {
        private readonly IMongoCollection<PedidoAuditLog> _collection;

        public MongoAuditRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<PedidoAuditLog>("pedido_audit");
        }

        public async Task RegistrarAsync(PedidoAuditLog log)
        {
            await _collection.InsertOneAsync(log);
        }
    }
}
