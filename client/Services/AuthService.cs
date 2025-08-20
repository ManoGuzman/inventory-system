using client.Models;
using System.Net.Http.Json;

namespace client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
                return result ?? new ApiResponse<LoginResponse> { Success = false, Message = "Invalid response" };
            }

            return new ApiResponse<LoginResponse>
            {
                Success = false,
                Message = "Authentication failed"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<LoginResponse>
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<UserDto>> GetCurrentUserAsync(string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/auth/me");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
                return result ?? new ApiResponse<UserDto> { Success = false, Message = "Invalid response" };
            }

            return new ApiResponse<UserDto>
            {
                Success = false,
                Message = "Failed to get user info"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserDto>
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
}
