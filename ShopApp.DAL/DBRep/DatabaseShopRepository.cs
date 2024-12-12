using Microsoft.EntityFrameworkCore;

using ShopApp.DB;
using ShopApp.Models;

namespace ShopApp.DAL.DBRep
{
    // хранение магазинов в БД
    public class DatabaseShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext _context;

        public DatabaseShopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // создать магазин
        public async Task CreateShop(Shop shop)
        {
            await _context.Shops.AddAsync(shop);
            await _context.SaveChangesAsync();
        }

        // получить все магазины
        public async Task<IEnumerable<Shop>> GetAllShops()
        {
            return await _context.Shops.ToListAsync();
        }

        // получить магазин по коду
        public async Task<Shop?> GetShopByCode(int code)
        {
            return await _context.Shops.FirstOrDefaultAsync(s => s.Code == code);
        }
    }
}
