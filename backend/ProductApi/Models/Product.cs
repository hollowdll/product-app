using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductApi.Dtos;

namespace ProductApi.Models;

public class Product
{
    public Product()
    {
    }

    public Product(ProductDto productDto)
    {
        Name = productDto.Name;
        Manufacturer = productDto.Manufacturer;
        Price = productDto.Price;
        Published = productDto.Published;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public decimal Price { get; set; }
    
    public DateTime Published { get; set; }
}
