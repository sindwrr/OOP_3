using OOP_3.Models;

namespace OOP_3.DAL
{
    // интерфейс хранилища магазинов
    public interface IShopRepository
    {
        Task CreateShop(Shop shop);
        Task<Shop?> GetShopByCode(int code);
        Task<IEnumerable<Shop>> GetAllShops();
    }
}
