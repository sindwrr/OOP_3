using ShopApp.Models;

namespace ShopApp.DAL
{
    // интерфейс хранилища продуктов
    public interface IProductStockRepository
    {
        Task AddProductToShop(ProductStock stock);
        Task<ProductStock?> GetCheapestProduct(string productName);
        Task<IEnumerable<ProductStock>> GetProductsInShop(int shopCode);
        Task UpdateProductStock(ProductStock stock);
    }
}
