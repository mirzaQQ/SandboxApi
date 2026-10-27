namespace Infrastructure.Data;

using LinqToDB;
using LinqToDB.Data;
using Infrastructure.Entities;

public class AppDataConnection(DataOptions options) : DataConnection(options)
{
    public ITable<Product> Products => this.GetTable<Product>();
}