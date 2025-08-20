using System.ComponentModel.DataAnnotations;
using Server.Models;

namespace Server.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;
    }

    public class ProductUpdateDto
    {
        [StringLength(50)]
        public string? Code { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int? Quantity { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }
    }

    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
    }
}
