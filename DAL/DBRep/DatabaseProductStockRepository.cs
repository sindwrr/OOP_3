using Microsoft.EntityFrameworkCore;

using ShopApp.DB;
using ShopApp.Models;

namespace ShopApp.DAL.DBRep
{
    // хранение продуктов в БД
    public class DatabaseProductStockRepository : IProductStockRepository
    {
        private readonly ApplicationDbContext _context;

        public DatabaseProductStockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // завезти партию продуктов в магазин
        public async Task AddProductToShop(ProductStock stock)
        {
            var existingStock = await _context.ProductStocks.FirstOrDefaultAsync(
                ps => ps.ShopCode == stock.ShopCode && ps.ProductName == stock.ProductName);

            if (existingStock != null)
            {
                existingStock.Quantity = stock.Quantity;
                existingStock.Price = stock.Price;
            }
            else
            {
                await _context.ProductStocks.AddAsync(stock);
            }

            await _context.SaveChangesAsync();
        }

        // обновить информацию о продуктах
        public async Task UpdateProductStock(ProductStock stock)
        {
            var existingStock = await _context.ProductStocks
                .FirstOrDefaultAsync(ps => ps.ProductName == stock.ProductName && ps.ShopCode == stock.ShopCode);

            if (existingStock != null)
            {
                existingStock.Quantity = stock.Quantity;
                existingStock.Price = stock.Price;

                _context.ProductStocks.Update(existingStock);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Партия продуктов не найдена.");
            }
        }

        // получить самый дешевый продукт (его партию в магазине) по названию
        public async Task<ProductStock?> GetCheapestProduct(string productName)
        {
            return await _context.ProductStocks
                .Where(ps => ps.ProductName == productName && ps.Quantity > 0)
                .OrderBy(ps => (double)ps.Price)
                .FirstOrDefaultAsync();
        }

        // получить все продукты в магазине
        public async Task<IEnumerable<ProductStock>> GetProductsInShop(int shopCode)
        {
            return await _context.ProductStocks
                .Where(ps => ps.ShopCode == shopCode)
                .ToListAsync();
        }
    }
}
