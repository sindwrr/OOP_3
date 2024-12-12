using ShopApp.Models;

namespace ShopApp.DAL
{
    // интерфейс хранилища магазинов
    public interface IShopRepository
    {
        Task CreateShop(Shop shop);
        Task<Shop?> GetShopByCode(int code);
        Task<IEnumerable<Shop>> GetAllShops();
    }
}
