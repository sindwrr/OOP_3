using ShopApp.Models;
using ShopApp.DAL;

namespace ShopApp.BLL
{
    // сервис (BLL) для взаимодействия с магазинами
    public class ShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IProductStockRepository _productStockRepository;

        public ShopService(IShopRepository shopRepository, IProductStockRepository productStockRepository)
        {
            _shopRepository = shopRepository;
            _productStockRepository = productStockRepository;
        }

        // создать магазин
        public async Task CreateShop(Shop shop)
        {
            await _shopRepository.CreateShop(shop);
        }

        // найти магазин с самым дешевым товаром
        public async Task<Shop?> FindCheapestShopForProduct(string productName)
        {
            var cheapestProduct = await _productStockRepository.GetCheapestProduct(productName);
            return cheapestProduct != null
                ? await _shopRepository.GetShopByCode(cheapestProduct.ShopCode)
                : null;
        }

        // получить товары, которые можно купить в магазине на сумму
        public async Task<IEnumerable<AffordableProduct>> GetAffordableProducts(int shopCode, decimal budget)
        {
            var products = await _productStockRepository.GetProductsInShop(shopCode);
            var affordableProducts = new List<AffordableProduct>();

            foreach (var product in products)
            {
                if (product.Price > 0)
                {
                    int maxQuantity = (int)(budget / product.Price);
                    if (maxQuantity > 0)
                    {
                        affordableProducts.Add(new AffordableProduct
                        {
                            Name = product.ProductName,
                            Quantity = Math.Min(maxQuantity, product.Quantity)
                        });
                    }
                }
            }

            return affordableProducts;
        }

        // купить продукты в магазине
        public async Task<decimal?> BuyProducts(int shopCode, Dictionary<string, int> productsToBuy)
        {
            var productStocks = await _productStockRepository.GetProductsInShop(shopCode);
            decimal totalCost = 0;

            foreach (var (productName, quantity) in productsToBuy)
            {
                var stock = productStocks.FirstOrDefault(p => p.ProductName == productName);
                if (stock == null || stock.Quantity < quantity)
                {
                    return null;
                }

                totalCost += stock.Price * quantity;
            }

            foreach (var (productName, quantity) in productsToBuy)
            {
                var stock = productStocks.First(p => p.ProductName == productName);
                stock.Quantity -= quantity;
                await _productStockRepository.UpdateProductStock(stock);
            }

            return totalCost;
        }

        // получить магазин, где можно купить набор продуктов дешевле всего
        public async Task<(Shop? Shop, decimal? TotalCost)> FindCheapestShopForBatch(Dictionary<string, int> productsToBuy)
        {
            var allShops = await _shopRepository.GetAllShops();
            Shop? cheapestShop = null;
            decimal? minCost = null;

            foreach (var shop in allShops)
            {
                var productStocks = await _productStockRepository.GetProductsInShop(shop.Code);
                decimal totalCost = 0;
                bool canFulfill = true;

                foreach (var (productName, quantity) in productsToBuy)
                {
                    var stock = productStocks.FirstOrDefault(p => p.ProductName == productName);
                    if (stock == null || stock.Quantity < quantity)
                    {
                        canFulfill = false;
                        break;
                    }
                    totalCost += stock.Price * quantity;
                }

                if (canFulfill && (minCost == null || totalCost < minCost))
                {
                    minCost = totalCost;
                    cheapestShop = shop;
                }
            }

            return (cheapestShop, minCost);
        }

        // вспомогательный класс для GetAffordableProducts()
        public class AffordableProduct
        {
            public string? Name { get; set; }
            public int Quantity { get; set; }
        }
    }
}
