using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ProductApi.Models;

namespace ProductApi.Data;

public class ProductDbContext
{
    public IMongoClient Client { get; }

    public IMongoDatabase Database { get; }

    public ProductDbContext(
        IOptions<ProductDatabaseSettings> databaseSettings)
    {
        Client = new MongoClient(
            databaseSettings.Value.ConnectionString);

        Database = Client.GetDatabase(
            databaseSettings.Value.DatabaseName);
    }
}
