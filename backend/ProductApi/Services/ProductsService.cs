using ProductApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductApi.Data;

namespace ProductApi.Services;

public class ProductsService : IDisposable
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductsService(
        IOptions<ProductDatabaseSettings> databaseSettings, ProductDbContext context)
    {
        _productsCollection = context.Database.GetCollection<Product>(
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
    /// Edits a document in products collection by replacing it.
    /// </summary>
    public async Task EditProductAsync(string id, Product updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(i => i.Id == id, updatedProduct);

    /// <summary>
    /// Deletes a document from products collection.
    /// </summary>
    public async Task DeleteProductAsync(string id) =>
        await _productsCollection.DeleteOneAsync(i => i.Id == id);

    public void Dispose()
    {
    }
}
