using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public enum MovementType
    {
        In,
        Out
    }

    public class Movement
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public MovementType MovementType { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
        
        public DateTime Date { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public Product Product { get; set; } = null!;
    }
}
