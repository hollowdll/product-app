using ProductApi.Models;
using ProductApi.Dtos;

namespace ProductApi.Helpers;

public static class DtoExtensions
{
    /// <summary>
    /// Converts <see cref="Product" /> class to <see cref="ProductDto" /> class.
    /// </summary>
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Manufacturer = product.Manufacturer,
            Price = product.Price,
            Published = product.Published
        };
    }
}
