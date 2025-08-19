using System.ComponentModel.DataAnnotations;
using Server.Models;

namespace Server.DTOs
{
    public class MovementCreateDto
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public MovementType MovementType { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }

    public class MovementResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public MovementType MovementType { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
