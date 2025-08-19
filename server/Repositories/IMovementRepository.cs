using Server.Models;

namespace Server.Repositories
{
    public interface IMovementRepository
    {
        Task<IEnumerable<Movement>> GetAllAsync();
        Task<Movement?> GetByIdAsync(int id);
        Task<IEnumerable<Movement>> GetByProductIdAsync(int productId);
        Task<Movement> CreateAsync(Movement movement);
        Task<IEnumerable<Movement>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Movement>> GetByMovementTypeAsync(MovementType movementType);
    }
}
