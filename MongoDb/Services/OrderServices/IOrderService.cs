using MongoDb.Dtos.OrderDtos;

namespace MongoDb.Services.OrderServices
{
    public interface IOrderService
    {
        Task<List<ResultOrderDto>> GetAllOrderAsync();
        Task CreateOrderAsync(CreateOrderDto createOrderDto);
        Task UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        Task DeleteOrderAsync(string id);
        Task<GetByIdOrderDto> GetByIdOrderAsync(string id);
        Task<List<OrderListWithExcel>> OrderListDownloadExcel();
        Task CreateOrderAsync2(CreateOrderDto createOrderDto);
    }
}
