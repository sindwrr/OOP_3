using ShopApp.Models;
using ShopApp.DAL;

namespace ShopApp.BLL
{
    // сервис (BLL) для взаимодействия с продуктами
    public class ProductStockService
    {
        private readonly IProductStockRepository _productStockRepository;

        public ProductStockService(IProductStockRepository productStockRepository)
        {
            _productStockRepository = productStockRepository;
        }

        // завезти партию продуктов в магазин
        public async Task AddProductToShop(ProductStock stock)
        {
            await _productStockRepository.AddProductToShop(stock);
        }

        // получить самый дешевый продукт (его партию в магазине) по названию
        public async Task<ProductStock?> GetCheapestProduct(string productName)
        {
            return await _productStockRepository.GetCheapestProduct(productName);
        }

        // получить все продукты в магазине
        public async Task<IEnumerable<ProductStock>> GetProductsInShop(int shopCode)
        {
            return await _productStockRepository.GetProductsInShop(shopCode);
        }
    }
}
