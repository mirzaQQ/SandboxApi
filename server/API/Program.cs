using LinqToDB;
using LinqToDB.AspNet;
using Infrastructure.Data;
using Service;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Controllers and OpenAPI/Swagger
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 2. Register linq2db Context
builder.Services.AddLinqToDBContext<AppDataConnection>((provider, options) =>
    options.UseSQLite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=app.db"));

// 3. Register Service Layer in DI Container
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();

app.Run();