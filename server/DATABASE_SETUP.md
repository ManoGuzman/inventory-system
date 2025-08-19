# Database Setup and Testing Guide

## Database Configuration

The application supports two database modes:

### 1. In-Memory Database (Default for Development)

- **Use case**: Quick testing, development, debugging
- **Configuration**: Set `"UseInMemoryDatabase": true` in `appsettings.Development.json`
- **Data**: Temporary, lost when application stops
- **Setup**: No setup required, automatically creates seed data

### 2. SQL Server Database (Production/Migration Testing)

- **Use case**: Production, persistent testing, migration development
- **Configuration**: Set `"UseInMemoryDatabase": false` or remove the setting
- **Data**: Persistent in SQL Server database
- **Setup**: Requires SQL Server LocalDB or full SQL Server

## Quick Start

### For In-Memory Testing (Default)

```bash
# Just run the application
dotnet run
```

### For SQL Server Testing

1. **Update configuration**:

   ```json
   // In appsettings.Development.json
   {
     "UseInMemoryDatabase": false, // or remove this line
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventorySystemDb_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

2. **Apply migrations**:

   ```bash
   dotnet ef database update
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

## Database Scripts

### Location: `/Scripts/`

1. **`01_QueryTests.sql`** - Test queries for data verification

   - View all products and stock levels
   - Movement history and summaries
   - Low stock alerts
   - Category analysis

2. **`02_SampleData.sql`** - Additional test data insertion

   - More products across categories
   - Additional users with different roles
   - Sample movements with realistic dates

3. **`03_Maintenance.sql`** - Database maintenance utilities
   - Data integrity checks
   - Cleanup scripts
   - Performance indexes
   - Inventory reports

## Entity Framework Commands

### Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations to database
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove

# Generate SQL script from migrations
dotnet ef migrations script
```

### Database Management

```bash
# Drop database (removes all data!)
dotnet ef database drop

# View migration history
dotnet ef migrations list

# Update to specific migration
dotnet ef database update MigrationName
```

## Default Seed Data

The application automatically creates:

### Products

- **ELEC001**: Laptop Dell (Electronics, Qty: 10)
- **FURN001**: Office Chair (Furniture, Qty: 25)
- **STAT001**: A4 Paper Pack (Stationery, Qty: 100)

### Users

- **admin**: Admin user (Password: "admin123")

### Movements

- Sample stock movements for each product

## Connection Strings

### Development (LocalDB)

```
Server=(localdb)\\mssqllocaldb;Database=InventorySystemDb_Dev;Trusted_Connection=true;MultipleActiveResultSets=true
```

### Production Example

```
Server=your-server;Database=InventorySystemDb;User Id=your-user;Password=your-password;Encrypt=true;TrustServerCertificate=false
```

## Testing the Database

1. **Start the application**:

   ```bash
   dotnet run
   ```

2. **Test API endpoints** using `server.http`:

   - GET `/api/products` - View all products
   - POST `/api/products` - Create new products
   - GET `/api/inventory/movements` - View movements
   - POST `/api/inventory/movement` - Create movements

3. **Query the database directly**:
   - Use SQL Server Management Studio (SSMS)
   - Use Azure Data Studio
   - Use VS Code with SQL Server extension
   - Run the scripts in `/Scripts/` folder

## Troubleshooting

### LocalDB Issues

```bash
# Check LocalDB instances
sqllocaldb info

# Start LocalDB
sqllocaldb start mssqllocaldb

# Create instance if missing
sqllocaldb create mssqllocaldb
```

### Migration Issues

```bash
# Reset migrations (WARNING: loses data)
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Switching Database Modes

- **To In-Memory**: Set `"UseInMemoryDatabase": true`
- **To SQL Server**: Set `"UseInMemoryDatabase": false` or remove setting
- Restart the application after changing modes
