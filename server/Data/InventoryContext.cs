using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Code).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Category).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Location).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Quantity).IsRequired();
                entity.Property(p => p.RegistrationDate).IsRequired();

                // Create unique index on Code
                entity.HasIndex(p => p.Code).IsUnique();

                // Configure relationship with movements
                entity.HasMany(p => p.Movements)
                      .WithOne(m => m.Product)
                      .HasForeignKey(m => m.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Movement entity
            modelBuilder.Entity<Movement>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.ProductId).IsRequired();
                entity.Property(m => m.MovementType).IsRequired();
                entity.Property(m => m.Quantity).IsRequired();
                entity.Property(m => m.Date).IsRequired();

                // Convert enum to string in database
                entity.Property(m => m.MovementType)
                      .HasConversion<string>();
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Role).IsRequired();
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();

                // Create unique index on Username
                entity.HasIndex(u => u.Username).IsUnique();

                // Convert enum to string in database
                entity.Property(u => u.Role)
                      .HasConversion<string>();
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // Password: "admin123"
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow,
                    IsActive = true
                }
            );

            // Seed sample categories and products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Code = "ELEC001",
                    Name = "Laptop Dell",
                    Category = "Electronics",
                    Quantity = 10,
                    Location = "Warehouse A - Shelf 1",
                    RegistrationDate = DateTime.UtcNow
                },
                new Product
                {
                    Id = 2,
                    Code = "FURN001",
                    Name = "Office Chair",
                    Category = "Furniture",
                    Quantity = 25,
                    Location = "Warehouse B - Section 2",
                    RegistrationDate = DateTime.UtcNow
                },
                new Product
                {
                    Id = 3,
                    Code = "STAT001",
                    Name = "A4 Paper Pack",
                    Category = "Stationery",
                    Quantity = 100,
                    Location = "Storage Room - Cabinet 3",
                    RegistrationDate = DateTime.UtcNow
                }
            );

            // Seed sample movements
            modelBuilder.Entity<Movement>().HasData(
                new Movement
                {
                    Id = 1,
                    ProductId = 1,
                    MovementType = MovementType.In,
                    Quantity = 10,
                    Date = DateTime.UtcNow.AddDays(-30)
                },
                new Movement
                {
                    Id = 2,
                    ProductId = 2,
                    MovementType = MovementType.In,
                    Quantity = 30,
                    Date = DateTime.UtcNow.AddDays(-25)
                },
                new Movement
                {
                    Id = 3,
                    ProductId = 2,
                    MovementType = MovementType.Out,
                    Quantity = 5,
                    Date = DateTime.UtcNow.AddDays(-20)
                },
                new Movement
                {
                    Id = 4,
                    ProductId = 3,
                    MovementType = MovementType.In,
                    Quantity = 100,
                    Date = DateTime.UtcNow.AddDays(-15)
                }
            );
        }
    }
}
