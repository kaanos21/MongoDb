using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDb.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } // Her sipariş sadece bir ürün türünü içerecek

        public int Quantity { get; set; } // Ürünün miktarını belirtir

        public DateTime OrderDate { get; set; }
    }
}
