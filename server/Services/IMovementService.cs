using Server.DTOs;
using Server.Models;

namespace Server.Services
{
    public interface IMovementService
    {
        Task<IEnumerable<MovementResponseDto>> GetAllMovementsAsync();
        Task<MovementResponseDto?> GetMovementByIdAsync(int id);
        Task<IEnumerable<MovementResponseDto>> GetMovementsByProductIdAsync(int productId);
        Task<MovementResponseDto> CreateMovementAsync(MovementCreateDto movementDto);
        Task<IEnumerable<MovementResponseDto>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MovementResponseDto>> GetMovementsByTypeAsync(MovementType movementType);
    }
}
