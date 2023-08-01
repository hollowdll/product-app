using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;
using ProductApi.Helpers;
using ProductApi.Dtos;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ProductsService _productsService;

    public ProductsController(ILogger<ProductsController> logger, ProductsService productsService)
    {
        _logger = logger;
        _productsService = productsService;
    }

    [HttpGet]
    public async Task<List<ProductDto>> GetProducts()
    {
        var products = await _productsService.GetProductsAsync();
        _logger.LogInformation($"Fetched all products from the database with count {products.Count()}");

        return products
            .Select(i => i.ToDto())
            .ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(string id)
    {
        var product = await _productsService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        _logger.LogInformation($"Fetched product with ID {product.Id} from the database");

        return product.ToDto();
    }

    [HttpPost]
    public async Task<ActionResult> AddProduct(ProductDto productDto)
    {
        var newProduct = new Product(productDto);
        await _productsService.AddProductAsync(newProduct);
        _logger.LogInformation("Added new product to the database");

        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
    }
}
