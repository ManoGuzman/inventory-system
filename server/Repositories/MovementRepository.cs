using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Repositories
{
    public class MovementRepository : IMovementRepository
    {
        private readonly InventoryContext _context;

        public MovementRepository(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movement>> GetAllAsync()
        {
            return await _context.Movements
                .Include(m => m.Product)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }

        public async Task<Movement?> GetByIdAsync(int id)
        {
            return await _context.Movements
                .Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Movement>> GetByProductIdAsync(int productId)
        {
            return await _context.Movements
                .Include(m => m.Product)
                .Where(m => m.ProductId == productId)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }

        public async Task<Movement> CreateAsync(Movement movement)
        {
            // Update product quantity based on movement type
            var product = await _context.Products.FindAsync(movement.ProductId);
            if (product != null)
            {
                if (movement.MovementType == MovementType.In)
                {
                    product.Quantity += movement.Quantity;
                }
                else if (movement.MovementType == MovementType.Out)
                {
                    product.Quantity -= movement.Quantity;
                    // Ensure quantity doesn't go negative
                    if (product.Quantity < 0)
                        product.Quantity = 0;
                }
            }

            _context.Movements.Add(movement);
            await _context.SaveChangesAsync();

            // Reload with product information
            await _context.Entry(movement)
                .Reference(m => m.Product)
                .LoadAsync();

            return movement;
        }

        public async Task<IEnumerable<Movement>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Movements
                .Include(m => m.Product)
                .Where(m => m.Date >= startDate && m.Date <= endDate)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movement>> GetByMovementTypeAsync(MovementType movementType)
        {
            return await _context.Movements
                .Include(m => m.Product)
                .Where(m => m.MovementType == movementType)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }
    }
}
