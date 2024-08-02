using AutoMapper;
using MongoDb.Dtos.OrderDtos;
using MongoDB.Driver;
using MongoDb.Entities;
using MongoDb.Settings;

namespace MongoDb.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IMongoCollection<Order> _OrderCollection;
        private readonly IMongoCollection<Customer> _CustomerCollection;
        private readonly IMongoCollection<Product> _ProductCollection;
        private readonly IMapper _mapper;

        public OrderService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _OrderCollection = database.GetCollection<Order>(databaseSettings.OrderCollectionName);
            _CustomerCollection = database.GetCollection<Customer>(databaseSettings.CustomerCollectionName);
            _ProductCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _mapper = mapper;
        }
        public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var value = _mapper.Map<Order>(createOrderDto);
            await _OrderCollection.InsertOneAsync(value);
        }

        public async Task CreateOrderAsync2(CreateOrderDto createOrderDto)
        {
            var customer = await _CustomerCollection.Find<Customer>(x => x.CustomerId == createOrderDto.CustomerId).FirstOrDefaultAsync();
            var product = await _ProductCollection.Find<Product>(x => x.ProductId == createOrderDto.ProductId).FirstOrDefaultAsync();

            decimal quantity = createOrderDto.Quantity;
            decimal price = product.Price;
            decimal firstMoney = customer.Money;
            decimal totalPrice = price * quantity;
            decimal finalAmount = firstMoney - totalPrice;

            var updateDefinition = Builders<Customer>.Update.Set(c => c.Money, finalAmount);

            var filter = Builders<Customer>.Filter.Eq(c => c.CustomerId, createOrderDto.CustomerId);

            await _CustomerCollection.UpdateOneAsync(filter, updateDefinition);
            var value = _mapper.Map<Order>(createOrderDto);
            await _OrderCollection.InsertOneAsync(value);
        }

        public async Task DeleteOrderAsync(string id)
        {
            await _OrderCollection.DeleteOneAsync(x => x.OrderId == id);
        }
        public async Task<List<ResultOrderDto>> GetAllOrderAsync()
        {
            var values = await _OrderCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultOrderDto>>(values);
        }
        public async Task<GetByIdOrderDto> GetByIdOrderAsync(string id)
        {
            var values = await _OrderCollection.Find<Order>(x => x.OrderId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdOrderDto>(values);
        }

        public async Task<List<OrderListWithExcel>> OrderListDownloadExcel()
        {
            var values=await _OrderCollection.Find(x=> true).ToListAsync();
            var result = new List<OrderListWithExcel>();
            foreach (var item in values)
            {
                item.Customer = await _CustomerCollection.Find(x => x.CustomerId == item.CustomerId).FirstAsync();
                item.Product = await _ProductCollection.Find(x => x.ProductId == item.ProductId).FirstAsync();

                var orderWithExcel = new OrderListWithExcel()
                {
                    OrderId = item.OrderId,
                    CustomerName = item.Customer.CustomerName,
                    ProductName = item.Product.ProductName,
                };
                result.Add(orderWithExcel);
            }
            return result;
        }

        public async Task UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            var values = _mapper.Map<Order>(updateOrderDto);
            await _OrderCollection.FindOneAndReplaceAsync(x => x.OrderId == updateOrderDto.OrderId, values);
        }
    }
}
