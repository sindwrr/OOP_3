using Microsoft.EntityFrameworkCore;
using ShopApp.DB;
using ShopApp.DAL;
using ShopApp.DAL.DBRep;
using ShopApp.DAL.FileRep;
using ShopApp.BLL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Выбор реализации DAL
var dalStorage = builder.Configuration.GetSection("DAL").GetValue<string>("Implementation");
if (dalStorage == "File")
{
    builder.Services.AddTransient<IShopRepository>(sp => new FileShopRepository());
    builder.Services.AddTransient<IProductStockRepository>(sp => new FileProductStockRepository());
}
else if (dalStorage == "Database" || dalStorage == "DB")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "data.db")};"));
    builder.Services.AddTransient<IShopRepository, DatabaseShopRepository>();
    builder.Services.AddTransient<IProductStockRepository, DatabaseProductStockRepository>();
}
else
{
    throw new InvalidOperationException("Неверный тип хранения данных.");
}

builder.Services.AddTransient<ShopService>();
builder.Services.AddTransient<ProductStockService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
