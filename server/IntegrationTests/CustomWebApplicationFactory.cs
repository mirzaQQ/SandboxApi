namespace IntegrationTests.Fixtures;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using LinqToDB;
using LinqToDB.AspNet;
using Infrastructure.Data;
using Infrastructure.Entities;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;
    private const string ConnectionString = "Data Source=SandboxDb;Mode=Memory;Cache=Shared";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Remove real DB registration from Program.cs
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DataOptions<AppDataConnection>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // 2. Keep shared In-Memory connection alive
            _connection = new SqliteConnection(ConnectionString);
            _connection.Open();

            // 3. Register linq2db with shared memory connection
            services.AddLinqToDBContext<AppDataConnection>((provider, options) =>
                options.UseSQLite(ConnectionString));

            // 4. Reset database schema before running test suite
            using var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDataConnection>();

            db.DropTable<Product>(tableOptions: TableOptions.DropIfExists);
            db.CreateTable<Product>();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Close();
        _connection?.Dispose();
    }
}