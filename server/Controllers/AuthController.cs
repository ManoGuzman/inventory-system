using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.Services;
using Server.DTOs;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Autentica un usuario y devuelve un JWT token
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(LoginRequestDto request)
        {
            try
            {
                var result = await _authService.AuthenticateAsync(request);

                if (result == null)
                {
                    return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse(
                        "Invalid username or password"));
                }

                return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(
                    result, "Login successful"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LoginResponseDto>.ErrorResponse(
                    "An error occurred during authentication", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Verifica el token JWT actual
        /// </summary>
        [HttpGet("verify")]
        [Authorize]
        public ActionResult<ApiResponse<UserInfoDto>> VerifyToken()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username))
                {
                    return Unauthorized(ApiResponse<UserInfoDto>.ErrorResponse("Invalid token"));
                }

                var userInfo = new UserInfoDto
                {
                    Id = int.Parse(userId),
                    Username = username,
                    Role = role ?? "Unknown",
                    LastLoginAt = DateTime.UtcNow
                };

                return Ok(ApiResponse<UserInfoDto>.SuccessResponse(
                    userInfo, "Token is valid"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserInfoDto>.ErrorResponse(
                    "An error occurred during token verification", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Logout (en cliente, simplemente borrar el token)
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public ActionResult<ApiResponse<object>> Logout()
        {
            // En JWT, el logout se maneja en el cliente eliminando el token
            // Aquí podríamos agregar lógica para blacklist tokens si fuera necesario

            return Ok(ApiResponse<object>.SuccessResponse(
                new { message = "Please remove the token from your client" },
                "Logged out successfully. Please remove the token from your client."));
        }

        /// <summary>
        /// Obtiene información del usuario actual
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public ActionResult<ApiResponse<UserInfoDto>> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username))
                {
                    return Unauthorized(ApiResponse<UserInfoDto>.ErrorResponse("Invalid token"));
                }

                var userInfo = new UserInfoDto
                {
                    Id = int.Parse(userId),
                    Username = username,
                    Role = role ?? "Unknown",
                    LastLoginAt = DateTime.UtcNow
                };

                return Ok(ApiResponse<UserInfoDto>.SuccessResponse(
                    userInfo, "User information retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserInfoDto>.ErrorResponse(
                    "An error occurred while retrieving user information", new List<string> { ex.Message }));
            }
        }
    }
}
