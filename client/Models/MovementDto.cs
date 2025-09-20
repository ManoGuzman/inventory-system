using System.ComponentModel.DataAnnotations;

namespace client.Models;

public class MovementDto
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public string MovementType { get; set; } = string.Empty; // "IN" or "OUT"

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [StringLength(200)]
    public string Reason { get; set; } = string.Empty;

    public DateTime MovementDate { get; set; }
    public int UserId { get; set; }

    // Navigation properties
    public string ProductName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}
