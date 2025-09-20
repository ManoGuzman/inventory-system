using System.Net.Http.Json;
using client.Models;
namespace client.Services;

/// <summary>
/// Service for managing product operations and API communication
/// </summary>
public class ProductService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:5001/api/products";

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Retrieves all products from the API
    /// </summary>
    /// <returns>List of products or empty list if error occurs</returns>
    public async Task<List<ProductDto>> GetProductsAsync()
    {
        try
        {
            var products = await _httpClient.GetFromJsonAsync<List<ProductDto>>(BaseUrl);
            return products ?? new List<ProductDto>();
        }
        catch (HttpRequestException)
        {
            // Log error in production
            return new List<ProductDto>();
        }
        catch (TaskCanceledException)
        {
            // Request timeout
            return new List<ProductDto>();
        }
    }

    /// <summary>
    /// Retrieves a specific product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product if found, null otherwise</returns>
    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Product ID must be greater than zero", nameof(id));
        }

        try
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>($"{BaseUrl}/{id}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (TaskCanceledException)
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="product">Product to create</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> CreateProductAsync(ProductDto product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, product);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="product">Updated product data</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> UpdateProductAsync(int id, ProductDto product)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Product ID must be greater than zero", nameof(id));
        }

        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", product);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    /// <summary>
    /// Deletes a product by ID
    /// </summary>
    /// <param name="id">Product ID to delete</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> DeleteProductAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Product ID must be greater than zero", nameof(id));
        }

        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }
}
