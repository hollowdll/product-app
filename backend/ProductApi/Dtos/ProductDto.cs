namespace ProductApi.Dtos;

public class ProductDto
{
    public string? Id { get; set; }

    public string Name { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public decimal Price { get; set; }
    
    public DateTime Published { get; set; }
}
