using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Dtos.OrderDtos;
using MongoDb.Services.CustomerService;
using MongoDb.Services.OrderServices;

namespace MongoDb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _OrderService;
        private readonly ICustomerService _CustomerService;

        public OrderController(IOrderService orderService, ICustomerService customerService)
        {
            _OrderService = orderService;
            _CustomerService = customerService;
        }

        public async Task<IActionResult> OrderList()
        {
            var values = await _OrderService.GetAllOrderAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            await _OrderService.CreateOrderAsync2(createOrderDto);
            return RedirectToAction("OrderList");
        }

        public async Task<IActionResult> DeleteOrder(string id)
        {
            await _OrderService.DeleteOrderAsync(id);
            return RedirectToAction("OrderList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOrder(string id)
        {
            var value = await _OrderService.GetByIdOrderAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            await _OrderService.UpdateOrderAsync(updateOrderDto);
            return RedirectToAction("OrderList");
        }
        
        public async Task<IActionResult> DownloadExcel()
        {
            var orders = await _OrderService.OrderListDownloadExcel();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Orders");

                // Başlıkları ekleyin
                worksheet.Cell(1, 1).Value = "OrderId";
                worksheet.Cell(1, 2).Value = "CustomerName";
                worksheet.Cell(1, 3).Value = "ProductName";

                // Verileri ekleyin
                for (int i = 0; i < orders.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = orders[i].OrderId;
                    worksheet.Cell(i + 2, 2).Value = orders[i].CustomerName;
                    worksheet.Cell(i + 2, 3).Value = orders[i].ProductName;
                }

                // Excel dosyasını hafızaya kaydedin
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    // Dosyayı indirin
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
                }
            }
        }
    }
}
