using ProductApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductApi.Data;

namespace ProductApi.Services;

public class ProductsService
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductsService(
        IOptions<ProductDatabaseSettings> databaseSettings)
    {
        var client = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var database = client.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _productsCollection = database.GetCollection<Product>(
            databaseSettings.Value.ProductsCollectionName);
    }

    /// <summary>
    /// Gets all documents from products collection.
    /// </summary>
    public async Task<List<Product>> GetProductsAsync() =>
        await _productsCollection.Find(_ => true).ToListAsync();

    /// <summary>
    /// Gets a document from products collection by id.
    /// </summary>
    public async Task<Product?> GetProductByIdAsync(string id) =>
        await _productsCollection.Find(i => i.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Adds a new document to products collection.
    /// </summary>
    public async Task AddProductAsync(Product newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    /// <summary>
    /// Updates a document in products collection by replacing it.
    /// </summary>
    public async Task UpdateProductAsync(string id, Product updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(i => i.Id == id, updatedProduct);

    /// <summary>
    /// Deletes a document from products collection.
    /// </summary>
    public async Task DeleteProductAsync(string id) =>
        await _productsCollection.DeleteOneAsync(i => i.Id == id);
}
