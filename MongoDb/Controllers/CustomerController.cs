using DinkToPdf;
using DinkToPdf.Contracts;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Dtos.CustomerDtos;
using MongoDb.Entities;
using MongoDb.Services.CustomerService;
using System.Text;

namespace MongoDb.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _CustomerService;
        private readonly IConverter _converter;

        public CustomerController(ICustomerService customerService, IConverter converter)
        {
            _CustomerService = customerService;
            _converter = converter;
        }
        public async Task<IActionResult> CustomerList()
        {
            var values = await _CustomerService.GetAllCustomerAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            await _CustomerService.CreateCustomerAsync(createCustomerDto);
            return RedirectToAction("CustomerList");
        }

        public async Task<IActionResult> DeleteCustomer(string id)
        {
            await _CustomerService.DeleteCustomerAsync(id);
            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(string id)
        {
            var value = await _CustomerService.GetByIdCustomerAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            await _CustomerService.UpdateCustomerAsync(updateCustomerDto);
            return RedirectToAction("CustomerList");
        }

        public async Task<IActionResult> DownloadPdf()
        {
            // Müşteri verilerini al
            var customers = await _CustomerService.GetAllCustomerAsync();

            // Bellekte bir akış oluştur
            using (var stream = new MemoryStream())
            {
                // iTextSharp kullanarak PDF dosyasını oluştur
                using (var writer = new PdfWriter(stream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        using (var document = new Document(pdf))
                        {
                            // Başlık ekle
                            document.Add(new Paragraph("Customer List")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(20));

                            // Tablo oluştur
                            var table = new Table(new float[] { 2, 2 }).UseAllAvailableWidth();

                            // Tablo başlıklarını ekle
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Customer ID")));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Customer Name")));

                            // Müşteri bilgilerini tabloya ekle
                            foreach (var customer in customers)
                            {
                                table.AddCell(new Cell().Add(new Paragraph(customer.CustomerId)));
                                table.AddCell(new Cell().Add(new Paragraph(customer.CustomerName)));
                            }

                            // Tabloyu dökümana ekle
                            document.Add(table);
                        }
                    }
                }

                // MemoryStream'in içeriğini byte dizisine dönüştür
                var content = stream.ToArray();

                // Dosya türü ve dosya adı
                var contentType = "application/pdf";
                var fileName = "customers.pdf";

                // Dosyayı indirilebilir olarak döndür
                return File(content, contentType, fileName);
            }
        }
    }
}
