namespace MongoDb.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public string CustomerId { get; set; }
        public string ProductId { get; set; } // Her sipariş sadece bir ürün türünü içerecek
        public int Quantity { get; set; } // Ürünün miktarını belirtir
        public DateTime OrderDate { get; set; }
    }
}
