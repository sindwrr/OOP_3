using Microsoft.AspNetCore.Mvc;

using ShopApp.Models;
using ShopApp.BLL;

namespace ShopApp.Controllers
{
    // контроллеры для API продуктов
    [ApiController]
    [Route("api/[controller]")]
    public class ProductStockController : ControllerBase
    {
        private readonly ProductStockService _productStockService;

        public ProductStockController(ProductStockService productStockService)
        {
            _productStockService = productStockService;
        }

        // завезти партию продуктов в магазин
        [HttpPost]
        public async Task<IActionResult> AddProductToShop(ProductStock stock)
        {
            await _productStockService.AddProductToShop(stock);
            return Ok("Товары успешно добавлены.");
        }

        // получить все продукты в магазине
        [HttpGet("shop/{shopCode}")]
        public async Task<IActionResult> GetProductsInShop(int shopCode)
        {
            var products = await _productStockService.GetProductsInShop(shopCode);
            if (!products.Any())
            {
                return NotFound($"В магазине с кодом '{shopCode}' не найдено продуктов.");
            }
            return Ok(products);
        }
    }
}
