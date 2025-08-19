using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly InventoryContext _context;

        public CategoryRepository(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
        {
            return await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<bool> CategoryExistsAsync(string category)
        {
            return await _context.Products
                .AnyAsync(p => p.Category == category);
        }

        public async Task<int> GetProductCountByCategoryAsync(string category)
        {
            return await _context.Products
                .CountAsync(p => p.Category == category);
        }
    }
}
