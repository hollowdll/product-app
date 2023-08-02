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

    // Gets all products
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var products = await _productsService.GetProductsAsync();
        _logger.LogInformation($"Fetched all products with count {products.Count()}");

        return products
            .Select(i => i.ToDto())
            .ToList();
    }

    // Gets a product by id
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(string id)
    {
        var product = await _productsService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        _logger.LogInformation($"Fetched product with ID '{product.Id}'");

        return product.ToDto();
    }

    // Adds a new product
    [HttpPost]
    public async Task<ActionResult> AddProduct(ProductDto productDto)
    {
        var newProduct = new Product(productDto);

        await _productsService.AddProductAsync(newProduct);
        _logger.LogInformation($"Added a new product with ID '{newProduct.Id}'");

        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
    }

    // Edits a product
    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult> EditProduct(string id, ProductDto productDto)
    {
        var product = await _productsService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.Name = productDto.Name;
        product.Manufacturer = productDto.Manufacturer;
        product.Price = productDto.Price;
        product.Published = productDto.Published;

        await _productsService.EditProductAsync(id, product);
        _logger.LogInformation($"Edited product with ID '{id}'");

        return NoContent();
    }

    // Deletes a product
    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        var product = await _productsService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        await _productsService.DeleteProductAsync(id);
        _logger.LogInformation($"Deleted product with ID '{id}'");

        return NoContent();
    }
}
