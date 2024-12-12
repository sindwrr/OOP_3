using ShopApp.Models;

namespace ShopApp.DAL.FileRep
{
    // хранение продуктов в файле
    public class FileProductStockRepository : IProductStockRepository
    {
        private string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "product_stock.csv");

        // завезти партию продуктов в магазин
        public async Task AddProductToShop(ProductStock stock)
        {
            var line = $"{stock.ProductName},{stock.ShopCode},{stock.Quantity},{stock.Price}";
            await File.AppendAllTextAsync(FilePath, line + Environment.NewLine);
        }

        // получить самый дешевый продукт (его партию в магазине) по названию
        public async Task<ProductStock?> GetCheapestProduct(string productName)
        {
            var stocks = await GetAllStocks();
            return stocks
                .Where(s => s.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase) && s.Quantity > 0)
                .OrderBy(s => s.Price)
                .FirstOrDefault();
        }

        // получить все продукты в магазине
        public async Task<IEnumerable<ProductStock>> GetProductsInShop(int shopCode)
        {
            var stocks = await GetAllStocks();
            return stocks.Where(s => s.ShopCode == shopCode);
        }

        // обновить информацию о продуктах
        public async Task UpdateProductStock(ProductStock stock)
        {
            var allStocks = (await GetAllStocks()).ToList();
            var index = allStocks.FindIndex(s =>
                s.ProductName == stock.ProductName && s.ShopCode == stock.ShopCode);

            if (index != -1)
            {
                allStocks[index].Quantity = stock.Quantity;
                allStocks[index].Price = stock.Price;

                await SaveProductStocksToFile(allStocks);
            }
            else
            {
                throw new InvalidOperationException("Партия продуктов не найдена.");
            }
        }

        // сохранить продукты в файл
        private async Task SaveProductStocksToFile(IEnumerable<ProductStock> stocks)
        {
            var lines = stocks.Select(stock =>
                $"{stock.ProductName},{stock.ShopCode},{stock.Quantity},{stock.Price}");
            await File.WriteAllLinesAsync(FilePath, lines);
        }

        // получить все партии продуктов
        private async Task<IEnumerable<ProductStock>> GetAllStocks()
        {
            if (!File.Exists(FilePath)) return Enumerable.Empty<ProductStock>();

            var lines = await File.ReadAllLinesAsync(FilePath);
            return lines.Select(line =>
            {
                var parts = line.Split(',');
                return new ProductStock
                {
                    ProductName = parts[0].Trim(),
                    ShopCode = int.Parse(parts[1].Trim()),
                    Quantity = int.Parse(parts[2].Trim()),
                    Price = decimal.Parse(parts[3].Trim())
                };
            });
        }
    }
}
