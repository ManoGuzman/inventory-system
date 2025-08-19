using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Server.Data
{
    public class InventoryContextFactory : IDesignTimeDbContextFactory<InventoryContext>
    {
        public InventoryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<InventoryContext>();

            // Use SQL Server for migrations
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=InventorySystemDb;Trusted_Connection=true;MultipleActiveResultSets=true");

            return new InventoryContext(optionsBuilder.Options);
        }
    }
}
