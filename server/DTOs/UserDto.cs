using System.ComponentModel.DataAnnotations;
using Server.Models;

namespace Server.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }
    }

    public class UserUpdateDto
    {
        [StringLength(50)]
        public string? Username { get; set; }

        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        public UserRole? Role { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public bool IsActive { get; set; }
    }
}
