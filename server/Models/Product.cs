using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Product
    {
        public int Id { get; set; }

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
        public int Quantity { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        // Navigation property for movements
        public ICollection<Movement> Movements { get; set; } = new List<Movement>();
    }
}
