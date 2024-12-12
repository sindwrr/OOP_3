using ShopApp.Models;

namespace ShopApp.DAL.FileRep
{
    // хранение магазинов в файле
    public class FileShopRepository : IShopRepository
    {
        private string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "shop.csv");

        // создать магазин
        public async Task CreateShop(Shop shop)
        {
            var line = $"{shop.Code},{shop.Name},{shop.Address}";
            await File.AppendAllTextAsync(FilePath, line + Environment.NewLine);
        }

        // получить все магазины
        public async Task<IEnumerable<Shop>> GetAllShops()
        {
            if (!File.Exists(FilePath)) return Enumerable.Empty<Shop>();

            var lines = await File.ReadAllLinesAsync(FilePath);
            return lines.Select(line =>
            {
                var parts = line.Split(',');
                return new Shop { Code = int.Parse(parts[0]), Name = parts[1], Address = parts[2] };
            });
        }

        // получить магазин по коду
        public async Task<Shop?> GetShopByCode(int code)
        {
            var shops = await GetAllShops();
            return shops.FirstOrDefault(s => s.Code == code);
        }
    }
}
