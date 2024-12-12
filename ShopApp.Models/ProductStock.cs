namespace ShopApp.Models
{
    // класс партии товаров
    public class ProductStock
    {
        public int ShopCode { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
