﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDb.Dtos.OrderDtos
{
    public class GetByIdOrderDto
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; } // Her sipariş sadece bir ürün türünü içerecek
        public int Quantity { get; set; } // Ürünün miktarını belirtir
        public DateTime OrderDate { get; set; }
    }
}
