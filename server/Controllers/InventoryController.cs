using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.Repositories;
using Server.Models;
using Server.DTOs;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IMovementRepository _movementRepository;
        private readonly IProductRepository _productRepository;

        public InventoryController(IMovementRepository movementRepository, IProductRepository productRepository)
        {
            _movementRepository = movementRepository;
            _productRepository = productRepository;
        }

        [HttpPost("movement")]
        public async Task<ActionResult<MovementResponseDto>> AddMovement(MovementCreateDto movementDto)
        {
            var product = await _productRepository.GetByIdAsync(movementDto.ProductId);
            if (product == null)
                return NotFound("Product not found");

            // Validate stock won't go negative for outbound movements
            if (movementDto.MovementType == MovementType.Out && product.Quantity < movementDto.Quantity)
                return BadRequest("Insufficient stock. Cannot reduce stock below zero.");

            var movement = new Movement
            {
                ProductId = movementDto.ProductId,
                MovementType = movementDto.MovementType,
                Quantity = movementDto.Quantity,
                Date = DateTime.UtcNow
            };

            var createdMovement = await _movementRepository.CreateAsync(movement);

            var result = new MovementResponseDto
            {
                Id = createdMovement.Id,
                ProductId = createdMovement.ProductId,
                ProductCode = createdMovement.Product.Code,
                ProductName = createdMovement.Product.Name,
                MovementType = createdMovement.MovementType,
                Quantity = createdMovement.Quantity,
                Date = createdMovement.Date
            };

            return CreatedAtAction(nameof(GetMovement), new { id = createdMovement.Id }, result);
        }

        [HttpGet("movement/{id}")]
        public async Task<ActionResult<MovementResponseDto>> GetMovement(int id)
        {
            var movement = await _movementRepository.GetByIdAsync(id);
            if (movement == null)
                return NotFound();

            var result = new MovementResponseDto
            {
                Id = movement.Id,
                ProductId = movement.ProductId,
                ProductCode = movement.Product.Code,
                ProductName = movement.Product.Name,
                MovementType = movement.MovementType,
                Quantity = movement.Quantity,
                Date = movement.Date
            };

            return result;
        }

        [HttpGet("movements/{productId}")]
        public async Task<ActionResult<IEnumerable<MovementResponseDto>>> GetProductMovements(int productId)
        {
            var movements = await _movementRepository.GetByProductIdAsync(productId);
            var result = movements.Select(m => new MovementResponseDto
            {
                Id = m.Id,
                ProductId = m.ProductId,
                ProductCode = m.Product.Code,
                ProductName = m.Product.Name,
                MovementType = m.MovementType,
                Quantity = m.Quantity,
                Date = m.Date
            });

            return Ok(result);
        }

        [HttpGet("movements")]
        public async Task<ActionResult<IEnumerable<MovementResponseDto>>> GetAllMovements()
        {
            var movements = await _movementRepository.GetAllAsync();
            var result = movements.Select(m => new MovementResponseDto
            {
                Id = m.Id,
                ProductId = m.ProductId,
                ProductCode = m.Product.Code,
                ProductName = m.Product.Name,
                MovementType = m.MovementType,
                Quantity = m.Quantity,
                Date = m.Date
            });

            return Ok(result);
        }

        [HttpGet("movements/date-range")]
        public async Task<ActionResult<IEnumerable<MovementResponseDto>>> GetMovementsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var movements = await _movementRepository.GetByDateRangeAsync(startDate, endDate);
            var result = movements.Select(m => new MovementResponseDto
            {
                Id = m.Id,
                ProductId = m.ProductId,
                ProductCode = m.Product.Code,
                ProductName = m.Product.Name,
                MovementType = m.MovementType,
                Quantity = m.Quantity,
                Date = m.Date
            });

            return Ok(result);
        }

        [HttpGet("movements/type/{movementType}")]
        public async Task<ActionResult<IEnumerable<MovementResponseDto>>> GetMovementsByType(MovementType movementType)
        {
            var movements = await _movementRepository.GetByMovementTypeAsync(movementType);
            var result = movements.Select(m => new MovementResponseDto
            {
                Id = m.Id,
                ProductId = m.ProductId,
                ProductCode = m.Product.Code,
                ProductName = m.Product.Name,
                MovementType = m.MovementType,
                Quantity = m.Quantity,
                Date = m.Date
            });

            return Ok(result);
        }
    }
}
