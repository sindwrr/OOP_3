using Microsoft.EntityFrameworkCore;
using OOP_3.DB;
using OOP_3.DAL;
using OOP_3.DAL.DBRep;
using OOP_3.DAL.FileRep;
using OOP_3.BLL;

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
else if (dalStorage == "Database")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite("Data Source=Data/data.db;"));
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
