using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PedidosECommerce.Domain.Entities
{
    public class PedidoAuditLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int PedidoId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid CorrelationId { get; set; }
        public string Etapa { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string? Erro { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
