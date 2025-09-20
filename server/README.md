# Inventory Management System - Backend API

A robust ASP.NET Core Web API for inventory management with JWT authentication, Entity Framework Core, and clean architecture.

## Overview

This backend API provides comprehensive inventory management capabilities with secure authentication, role-based authorization, and real-time inventory tracking.

### Key Features

- **JWT Authentication** with BCrypt password hashing
- **RESTful API Design** with OpenAPI/Swagger documentation
- **Entity Framework Core** with Code-First migrations
- **Clean Architecture** with Repository and Service patterns
- **Role-based Authorization** (Admin, Manager, User)
- **Input Validation** and error handling
- **Inventory Tracking** with movement history
- **Security Best Practices** with CORS configuration

## Prerequisites

- .NET 9.0 SDK or later
- SQL Server 2019+ or SQL Server LocalDB
- Visual Studio 2022 or VS Code
- Entity Framework Core CLI tools

## Installation

### 1. Install EF Core CLI Tools

```bash
dotnet tool install --global dotnet-ef
dotnet ef --version
```

### 2. Clone and Navigate

```bash
git clone <repository-url>
cd inventory-system/server
```

### 3. Database Configuration

Update `appsettings.json` with your database connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventorySystemDb;Trusted_Connection=true;"
  },
  "UseInMemoryDatabase": false,
  "JwtSettings": {
    "SecretKey": "your-super-secret-jwt-key-here-minimum-256-bits",
    "Issuer": "InventorySystemAPI",
    "Audience": "InventorySystemClient",
    "ExpirationHours": 8
  }
}
```

#### Alternative Connection Strings

**SQL Server Express:**

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=InventorySystemDb;Trusted_Connection=true;"
```

**SQL Server with Authentication:**

```json
"DefaultConnection": "Server=localhost;Database=InventorySystemDb;User Id=sa;Password=YourPassword;"
```

**Azure SQL Database:**

```json
"DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=InventorySystemDb;Persist Security Info=False;User ID=yourusername;Password=yourpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

### 4. Database Setup

```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update

# Verify tables were created: Users, Products, Movements
```

### 5. Run the Application

```bash
# Development mode with hot reload
dotnet watch run

# Standard development run
dotnet run

# Production build
dotnet publish -c Release
```

The API will be available at:

- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger`

## Project Structure

```
server/
├── Controllers/           # API Controllers (Auth, Products, Inventory)
├── Services/             # Business Logic Layer
│   ├── IAuthenticationService.cs
│   ├── AuthenticationService.cs
│   ├── IProductService.cs
│   ├── ProductService.cs
│   ├── IMovementService.cs
│   ├── MovementService.cs
│   ├── IUserService.cs
│   └── UserService.cs
├── Repositories/         # Data Access Layer
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   ├── IProductRepository.cs
│   ├── ProductRepository.cs
│   ├── IMovementRepository.cs
│   ├── MovementRepository.cs
│   ├── ICategoryRepository.cs
│   └── CategoryRepository.cs
├── Models/              # Domain Entities
│   ├── User.cs
│   ├── Product.cs
│   └── Movement.cs
├── DTOs/                # Data Transfer Objects
│   ├── AuthenticationDto.cs
│   ├── UserDto.cs
│   ├── ProductDto.cs
│   ├── MovementDto.cs
│   └── ApiResponse.cs
├── Data/                # EF Core Context
│   ├── InventoryContext.cs
│   └── InventoryContextFactory.cs
├── Migrations/          # Database Migrations
├── Middleware/          # Custom Middleware
│   └── ValidationMiddleware.cs
├── Scripts/             # Database Scripts
│   ├── 01_QueryTests.sql
│   ├── 02_SampleData.sql
│   └── 03_Maintenance.sql
└── Properties/          # Launch Settings
    └── launchSettings.json
```

## API Endpoints

### Authentication

| Method | Endpoint           | Description           | Authorization |
| ------ | ------------------ | --------------------- | ------------- |
| `POST` | `/api/auth/login`  | User authentication   | None          |
| `GET`  | `/api/auth/me`     | Get current user info | JWT Required  |
| `GET`  | `/api/auth/verify` | Verify token validity | JWT Required  |
| `POST` | `/api/auth/logout` | User logout           | JWT Required  |

### Products

| Method   | Endpoint             | Description        | Authorization |
| -------- | -------------------- | ------------------ | ------------- |
| `GET`    | `/api/products`      | Get all products   | JWT Required  |
| `GET`    | `/api/products/{id}` | Get product by ID  | JWT Required  |
| `POST`   | `/api/products`      | Create new product | JWT Required  |
| `PUT`    | `/api/products/{id}` | Update product     | JWT Required  |
| `DELETE` | `/api/products/{id}` | Delete product     | Admin/Manager |

### Inventory

| Method | Endpoint                   | Description               | Authorization |
| ------ | -------------------------- | ------------------------- | ------------- |
| `GET`  | `/api/inventory`           | Get inventory movements   | JWT Required  |
| `POST` | `/api/inventory/movement`  | Record inventory movement | JWT Required  |
| `GET`  | `/api/inventory/low-stock` | Get low stock products    | JWT Required  |

## Database Schema

### Users Table

| Column       | Type          | Constraints           | Description            |
| ------------ | ------------- | --------------------- | ---------------------- |
| Id           | int           | Primary Key, Identity | User identifier        |
| Username     | nvarchar(50)  | Unique, Required      | Login username         |
| PasswordHash | nvarchar(255) | Required              | BCrypt hashed password |
| Role         | nvarchar(20)  | Required              | Admin, Manager, User   |
| CreatedAt    | datetime2     | Required              | Account creation date  |
| LastLoginAt  | datetime2     | Nullable              | Last login timestamp   |
| IsActive     | bit           | Default: true         | Account status         |

### Products Table

| Column           | Type          | Constraints           | Description               |
| ---------------- | ------------- | --------------------- | ------------------------- |
| Id               | int           | Primary Key, Identity | Product identifier        |
| Code             | nvarchar(20)  | Unique, Required      | Product code              |
| Name             | nvarchar(100) | Required              | Product name              |
| Category         | nvarchar(50)  | Required              | Product category          |
| Quantity         | int           | Default: 0            | Current stock quantity    |
| Location         | nvarchar(100) | Required              | Storage location          |
| RegistrationDate | datetime2     | Required              | Product registration date |

### Movements Table

| Column       | Type          | Constraints           | Description              |
| ------------ | ------------- | --------------------- | ------------------------ |
| Id           | int           | Primary Key, Identity | Movement identifier      |
| ProductId    | int           | Foreign Key, Required | Reference to Products.Id |
| MovementType | nvarchar(10)  | Required              | 'In' or 'Out'            |
| Quantity     | int           | Required              | Movement quantity        |
| Date         | datetime2     | Required              | Movement timestamp       |
| Notes        | nvarchar(500) | Optional              | Movement notes           |

## Testing

### Test Files

Use the included HTTP test files for API testing:

- `test-auth.http` - Authentication flow testing
- `server.http` - General API testing
- `api-tests.http` - Comprehensive API tests

### Sample Test Workflow

1. **Login to get JWT token:**

   ```http
   POST {{hostAddress}}/api/auth/login
   Content-Type: application/json

   {
     "username": "admin",
     "password": "admin123"
   }
   ```

2. **Use token in subsequent requests:**
   ```http
   GET {{hostAddress}}/api/products
   Authorization: Bearer YOUR_JWT_TOKEN
   ```

### Default Test Accounts

| Username | Password   | Role    | Description          |
| -------- | ---------- | ------- | -------------------- |
| admin    | admin123   | Admin   | Full system access   |
| manager  | manager123 | Manager | Inventory management |
| user     | user123    | User    | Read-only access     |

## Database Management

### Sample Data Population

Execute the sample data script to populate test data:

```bash
# Using sqlcmd
sqlcmd -S (localdb)\mssqllocaldb -d InventorySystemDb -i Scripts/02_SampleData.sql

# Or run from SQL Server Management Studio
# Open and execute Scripts/02_SampleData.sql
```

### Database Maintenance

Regular maintenance tasks:

```bash
# Run query tests
sqlcmd -S (localdb)\mssqllocaldb -d InventorySystemDb -i Scripts/01_QueryTests.sql

# Run maintenance procedures
sqlcmd -S (localdb)\mssqllocaldb -d InventorySystemDb -i Scripts/03_Maintenance.sql
```

### Database Reset

To completely reset the database:

```bash
# Drop database
dotnet ef database drop

# Remove migrations
dotnet ef migrations remove

# Create new migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Troubleshooting

#### Migration Errors

```bash
# Reset migrations
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### Connection Issues

- Verify SQL Server service is running
- Check connection string format
- Ensure database permissions
- Test connection with SQL Server Management Studio

## Security

### Authentication & Authorization

- **JWT tokens** with configurable expiration
- **BCrypt password hashing** with salt
- **Role-based authorization** (Admin, Manager, User)
- **CORS configuration** for cross-origin requests

### Security Best Practices

- Strong JWT secret keys (minimum 256 bits)
- Secure connection strings
- Input validation middleware
- SQL injection prevention through EF Core
- Password complexity requirements

## Development

### Adding New Features

1. **Domain Model** - Create entity in `/Models`
2. **DTO** - Add data transfer object in `/DTOs`
3. **Repository** - Implement data access in `/Repositories`
4. **Service** - Add business logic in `/Services`
5. **Controller** - Create API endpoints in `/Controllers`
6. **Migration** - Update database schema

### Code Standards

- Follow C# naming conventions
- Use async/await patterns consistently
- Implement comprehensive error handling
- Add XML documentation comments
- Write unit tests for new features
- Follow SOLID principles

### Environment Configuration

**Development:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventorySystemDb_Dev;Trusted_Connection=true;"
  },
  "UseInMemoryDatabase": false
}
```

**Testing:**

```json
{
  "UseInMemoryDatabase": true
}
```

**Production:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=InventorySystemDb;Integrated Security=true;Encrypt=true;"
  }
}
```

## Deployment

### Production Configuration

1. **Update connection strings** for production database
2. **Configure JWT settings** with strong secret keys
3. **Set up environment variables** for sensitive data
4. **Enable HTTPS** and SSL certificates
5. **Configure logging** and monitoring

### Docker Support

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY . .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "server.dll"]
```

### Performance Considerations

- Configure connection pooling
- Set up database indexes
- Monitor query performance
- Implement caching strategies
- Use compression middleware

## Monitoring & Maintenance

### Backup Strategy

- Daily automated backups
- Transaction log backups
- Test restore procedures
- Document recovery processes

### Performance Monitoring

- Database index usage analysis
- Query execution plan reviews
- Connection pool monitoring
- Memory usage tracking

## Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/amazing-feature`)
3. Make changes following code standards
4. Add comprehensive tests
5. Update documentation
6. Submit pull request

## License

This project is licensed under the MIT License.

## Project Structure

```
server/
├── Controllers/        # API Controllers
├── Services/          # Business Logic Layer
├── Repositories/      # Data Access Layer
├── Models/           # Domain Entities
├── DTOs/             # Data Transfer Objects
├── Data/             # EF Core Context
├── Migrations/       # Database Migrations
├── Middleware/       # Custom Middleware
└── Scripts/          # Database Scripts
```

## API Endpoints

### Authentication

- `POST /api/auth/login` - User authentication
- `GET /api/auth/me` - Get current user info
- `GET /api/auth/verify` - Verify token validity
- `POST /api/auth/logout` - User logout

### Products

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### Inventory

- `GET /api/inventory` - Get inventory movements
- `POST /api/inventory/movement` - Record inventory movement
- `GET /api/inventory/low-stock` - Get low stock products

## Testing

Use the included HTTP test files:

- `test-auth.http` - Authentication flow testing
- `server.http` - General API testing
- `api-tests.http` - Comprehensive API tests

## Database

### Default Admin User

- Username: `admin`
- Password: `admin123`
- Role: `Admin`

### Core Tables

- **Users** - User accounts and roles
- **Products** - Product information and inventory
- **Movements** - Inventory movement history

## Security

- JWT token-based authentication
- BCrypt password hashing
- Role-based authorization
- CORS configuration
- Input validation middleware

## Development

### Adding New Features

1. Create domain model in `/Models`
2. Add DTOs in `/DTOs`
3. Implement repository in `/Repositories`
4. Add service layer in `/Services`
5. Create controller in `/Controllers`
6. Update database with migrations

### Code Standards

- Follow C# naming conventions
- Use async/await patterns
- Implement proper error handling
- Add XML documentation
- Write unit tests

## Deployment

### Production Configuration

1. Update connection strings
2. Configure JWT settings
3. Set up environment variables
4. Enable HTTPS
5. Configure logging

### Docker Support

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "server.dll"]
```

## Contributing

1. Fork the repository
2. Create feature branch
3. Make changes
4. Add tests
5. Submit pull request

## License

This project is licensed under the MIT License.
