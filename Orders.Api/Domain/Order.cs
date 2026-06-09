using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Orders.Api.Domain
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public Guid UserId { get; set; } // referencia al microservicio de usuarios
        public List<OrderItem> items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string OrderDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public class OrderItem
    {
        public Guid ProductId { get; set; } // referencia al microservicio de productos
        public int Quantity { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal UnitPrice { get; set; }
    }
}
