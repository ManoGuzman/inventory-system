using System.Net.Http.Json;
using client.Models;

namespace client.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<List<ProductDto>>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/products");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<ProductDto>>>();
                return result ?? new ApiResponse<List<ProductDto>> { Success = false, Message = "Invalid response" };
            }

            return new ApiResponse<List<ProductDto>>
            {
                Success = false,
                Message = "Failed to fetch products"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<ProductDto>>
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<ProductDto>> GetProductAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>();
                return result ?? new ApiResponse<ProductDto> { Success = false, Message = "Invalid response" };
            }

            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = "Product not found"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<ProductDto>> CreateProductAsync(ProductDto product)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/products", product);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>();
                return result ?? new ApiResponse<ProductDto> { Success = false, Message = "Invalid response" };
            }

            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = "Failed to create product"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, ProductDto product)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", product);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductDto>>();
                return result ?? new ApiResponse<ProductDto> { Success = false, Message = "Invalid response" };
            }

            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = "Failed to update product"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<bool> { Success = true, Data = true };
            }

            return new ApiResponse<bool>
            {
                Success = false,
                Message = "Failed to delete product"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
}
