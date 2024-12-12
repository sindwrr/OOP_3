using Microsoft.AspNetCore.Mvc;

using ShopApp.Models;
using ShopApp.BLL;

namespace ShopApp.Controllers
{
    // контроллеры для API магазинов
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly ShopService _shopService;

        public ShopController(ShopService shopService)
        {
            _shopService = shopService;
        }

        // создать магазин
        [HttpPost]
        public async Task<IActionResult> CreateShop([FromBody] Shop shop)
        {
            await _shopService.CreateShop(shop);
            return Ok();
        }

        // найти магазин с самым дешевым товаром
        [HttpGet("find-cheapest/{productName}")]
        public async Task<IActionResult> FindCheapestShop(string productName)
        {
            var shop = await _shopService.FindCheapestShopForProduct(productName);
            return shop != null ? Ok(shop) : NotFound();
        }

        // получить товары, которые можно купить в магазине на сумму
        [HttpGet("affordable-products/{shopCode}/{budget}")]
        public async Task<IActionResult> GetAffordableProducts(int shopCode, decimal budget)
        {
            var products = await _shopService.GetAffordableProducts(shopCode, budget);
            return Ok(products);
        }

        // купить продукты в магазине
        [HttpPost("buy-products/{shopCode}")]
        public async Task<IActionResult> BuyProducts(int shopCode, [FromBody] Dictionary<string, int> productsToBuy)
        {
            var totalCost = await _shopService.BuyProducts(shopCode, productsToBuy);
            if (totalCost == null)
            {
                return BadRequest("Недостаточный бюджет.");
            }
            return Ok(new { TotalCost = totalCost });
        }

        // получить магазин, где можно купить набор продуктов дешевле всего
        [HttpPost("cheapest-shop-for-batch")]
        public async Task<IActionResult> FindCheapestShopForBatch([FromBody] Dictionary<string, int> productsToBuy)
        {
            var (shop, totalCost) = await _shopService.FindCheapestShopForBatch(productsToBuy);
            if (shop == null)
            {
                return NotFound("Не найден подходящий магазин.");
            }
            return Ok(new { Shop = shop, TotalCost = totalCost });
        }
    }
}
