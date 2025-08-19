using Server.DTOs;
using Server.Models;
using Server.Repositories;

namespace Server.Services
{
    public class MovementService : IMovementService
    {
        private readonly IMovementRepository _movementRepository;
        private readonly IProductRepository _productRepository;

        public MovementService(IMovementRepository movementRepository, IProductRepository productRepository)
        {
            _movementRepository = movementRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<MovementResponseDto>> GetAllMovementsAsync()
        {
            var movements = await _movementRepository.GetAllAsync();
            return movements.Select(MapToMovementResponseDto);
        }

        public async Task<MovementResponseDto?> GetMovementByIdAsync(int id)
        {
            var movement = await _movementRepository.GetByIdAsync(id);
            return movement != null ? MapToMovementResponseDto(movement) : null;
        }

        public async Task<IEnumerable<MovementResponseDto>> GetMovementsByProductIdAsync(int productId)
        {
            var movements = await _movementRepository.GetByProductIdAsync(productId);
            return movements.Select(MapToMovementResponseDto);
        }

        public async Task<IEnumerable<MovementResponseDto>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var movements = await _movementRepository.GetByDateRangeAsync(startDate, endDate);
            return movements.Select(MapToMovementResponseDto);
        }

        public async Task<IEnumerable<MovementResponseDto>> GetMovementsByTypeAsync(MovementType movementType)
        {
            var movements = await _movementRepository.GetAllAsync();
            var filtered = movements.Where(m => m.MovementType == movementType);
            return filtered.Select(MapToMovementResponseDto);
        }

        public async Task<MovementResponseDto> CreateMovementAsync(MovementCreateDto movementDto)
        {
            // Verify product exists
            var product = await _productRepository.GetByIdAsync(movementDto.ProductId);
            if (product == null)
                throw new ArgumentException($"Product with ID {movementDto.ProductId} not found");

            // Check if we have enough stock for outbound movements
            if (movementDto.MovementType == MovementType.Out && product.Quantity < movementDto.Quantity)
                throw new InvalidOperationException($"Insufficient stock. Available: {product.Quantity}, Requested: {movementDto.Quantity}");

            var movement = new Movement
            {
                ProductId = movementDto.ProductId,
                MovementType = movementDto.MovementType,
                Quantity = movementDto.Quantity,
                Date = DateTime.UtcNow
            };

            var createdMovement = await _movementRepository.CreateAsync(movement);
            return MapToMovementResponseDto(createdMovement);
        }

        private static MovementResponseDto MapToMovementResponseDto(Movement movement)
        {
            return new MovementResponseDto
            {
                Id = movement.Id,
                ProductId = movement.ProductId,
                ProductCode = movement.Product?.Code ?? "",
                ProductName = movement.Product?.Name ?? "",
                MovementType = movement.MovementType,
                Quantity = movement.Quantity,
                Date = movement.Date
            };
        }
    }
}
