using Server.DTOs;
using Server.Models;
using Server.Repositories;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToUserResponseDto);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToUserResponseDto(user) : null;
        }

        public async Task<UserResponseDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user != null ? MapToUserResponseDto(user) : null;
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = userDto.Role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var createdUser = await _userRepository.CreateAsync(user);
            return MapToUserResponseDto(createdUser);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int id, UserUpdateDto userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                return null;

            existingUser.Username = userDto.Username ?? existingUser.Username;
            existingUser.Role = userDto.Role ?? existingUser.Role;
            existingUser.IsActive = userDto.IsActive ?? existingUser.IsActive;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            var updatedUser = await _userRepository.UpdateAsync(id, existingUser);
            return updatedUser != null ? MapToUserResponseDto(updatedUser) : null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            await _userRepository.UpdateLastLoginAsync(userId);
        }

        private static UserResponseDto MapToUserResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive
            };
        }
    }
}
