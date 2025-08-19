using Server.DTOs;
using Server.Models;

namespace Server.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request);
        string GenerateJwtToken(User user);
        Task<User?> ValidateUserAsync(string username, string password);
    }
}
