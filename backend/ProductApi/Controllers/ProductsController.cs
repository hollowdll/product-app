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
        List<Product> products = new List<Product>();
        try
        {
            products = await _productsService.GetProductsAsync();
        }
        catch(Exception e)
        {
            _logger.LogError($"Failed to fetch all products: {e.Message}");
            return Problem();
        }
        _logger.LogInformation($"Fetched all products with count {products.Count()}");

        return products
            .Select(i => i.ToDto())
            .ToList();
    }

    // Gets a product by id
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ProductDto>> GetProductById(string id)
    {
        Product? product = null;
        try
        {
            product = await _productsService.GetProductByIdAsync(id);
        }
        catch(FormatException e)
        {
            _logger.LogError($"Failed to fetch product with ID '{id}': Failed to parse document ID: {e.Message}");
            return NotFound();
        }
        catch(Exception e)
        {
            _logger.LogError($"Failed to fetch product with ID '{id}': {e.Message}");
            return Problem();
        }

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
        try
        {
            await _productsService.AddProductAsync(newProduct);
        }
        catch(Exception e)
        {
            _logger.LogError($"Failed to add a new product: {e.Message}");
            return Problem();
        }
        _logger.LogInformation($"Added a new product with ID '{newProduct.Id}'");

        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
    }

    // Edits a product
    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult> EditProduct(string id, ProductDto productDto)
    {
        Product? product = null;
        try
        {
            product = await _productsService.GetProductByIdAsync(id);
        }
        catch(FormatException e)
        {
            _logger.LogError($"Failed to fetch product with ID '{id}': Failed to parse document ID: {e.Message}");
            return NotFound();
        }
        catch(Exception e)
        {
            _logger.LogError($"Failed to fetch product with ID '{id}': {e.Message}");
            return Problem();
        }

        if (product == null)
        {
            return NotFound();
        }

        product.Name = productDto.Name;
        product.Manufacturer = productDto.Manufacturer;
        product.Price = productDto.Price;
        product.Published = productDto.Published;

        try
        {
            await _productsService.EditProductAsync(id, product);
        }
        catch(Exception e)
        {
            _logger.LogError($"Failed to edit product with ID '{id}': {e.Message}");
            return Problem();
        }
        _logger.LogInformation($"Edited product with ID '{id}'");

        return NoContent();
    }

    // Deletes a product
    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        Product? product = null;
        try
        {
            product = await _productsService.GetProductByIdAsync(id);
        }
        catch(FormatException e)
        {
            _logger.LogError($"Failed to fetch product with ID '{id}': Failed to parse document ID: {e.Message}");
            return NotFound();
        }
        catch(Exception e)
        {
            _logger.LogError($"Failed to fetch product with ID '{id}': {e.Message}");
            return Problem();
        }

        if (product == null)
        {
            return NotFound();
        }

        try
        {
            await _productsService.DeleteProductAsync(id);
        }
        catch(Exception e)
        {
            _logger.LogError($"Failed to delete product with ID '{id}': {e.Message}");
            return Problem();
        }
        _logger.LogInformation($"Deleted product with ID '{id}'");

        return NoContent();
    }
}
