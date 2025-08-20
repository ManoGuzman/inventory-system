namespace client.Models;

public enum MovementType
{
    In,
    Out
}

public class MovementDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public MovementType MovementType { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
}
