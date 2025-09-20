using client.Models;
using System.Net.Http.Json;
namespace client.Services;

/// <summary>
/// Service for handling authentication operations
/// </summary>
public class AuthService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:5001/api/auth";

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Authenticates a user with email and password
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>True if authentication successful, false otherwise</returns>
    public async Task<bool> LoginAsync(LoginRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return false;
        }

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", request);
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
    /// Registers a new user account
    /// </summary>
    /// <param name="request">Registration details</param>
    /// <returns>True if registration successful, false otherwise</returns>
    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName))
        {
            return false;
        }

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", request);
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
    /// Logs out the current user
    /// </summary>
    /// <returns>Task representing the logout operation</returns>
    public async Task LogoutAsync()
    {
        try
        {
            await _httpClient.PostAsync($"{BaseUrl}/logout", null);
        }
        catch (HttpRequestException)
        {
            // Silent fail for logout
        }
        catch (TaskCanceledException)
        {
            // Silent fail for logout
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
