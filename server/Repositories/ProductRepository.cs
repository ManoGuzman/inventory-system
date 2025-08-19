using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryContext _context;

        public ProductRepository(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Movements)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Movements)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetByCodeAsync(string code)
        {
            return await _context.Products
                .Include(p => p.Movements)
                .FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return null;

            existingProduct.Code = product.Code;
            existingProduct.Name = product.Name;
            existingProduct.Category = product.Category;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Location = product.Location;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByCodeAsync(string code, int? excludeId = null)
        {
            var query = _context.Products.Where(p => p.Code == code);

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            return await _context.Products
                .Include(p => p.Movements)
                .Where(p => p.Category == category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByLocationAsync(string location)
        {
            return await _context.Products
                .Include(p => p.Movements)
                .Where(p => p.Location == location)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}
