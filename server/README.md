# 📦 Inventory Management System - Backend API

A modern, secure inventory management system built with **ASP.NET Core 9.0** featuring JWT authentication, Entity Framework Core, and a clean architecture pattern.

## 🚀 Features

### 🔐 **Authentication & Security**

- JWT-based authentication with BCrypt password hashing
- Role-based access control (Admin, Manager, User)
- Protected API endpoints with authorization middleware
- Secure token generation and validation

### 📊 **Inventory Management**

- **Products**: Complete CRUD operations with categories and locations
- **Movements**: Track inventory in/out movements with automatic quantity updates
- **Users**: User management with role-based permissions
- **Categories**: Product categorization system

### 🏗️ **Architecture**

- **Clean Architecture** with separation of concerns
- **Repository Pattern** for data access abstraction
- **Service Layer** for business logic
- **DTOs** for API data transfer
- **Entity Framework Core** with Code-First migrations

### 🛠️ **Technical Stack**

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server / In-Memory (configurable)
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer tokens
- **API Documentation**: OpenAPI/Swagger
- **Password Hashing**: BCrypt.Net

## 📋 Prerequisites

- **.NET 9.0 SDK** or later
- **SQL Server** (optional - can use In-Memory database)
- **Visual Studio 2022** or **VS Code** (recommended)

## ⚡ Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/inventory-system.git
cd inventory-system/server
```

### 2. Install Dependencies

```bash
dotnet restore
```

### 3. Configure Database

Choose your database option in `appsettings.Development.json`:

**Option A: In-Memory Database (Default)**

```json
{
  "UseInMemoryDatabase": true
}
```

**Option B: SQL Server**

```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventorySystemDb;Trusted_Connection=true;"
  }
}
```

### 4. Run Database Migrations (SQL Server only)

```bash
dotnet ef database update
```

### 5. Start the Application

```bash
dotnet run
```

The API will be available at:

- **HTTP**: `http://localhost:5184`
- **Swagger UI**: `http://localhost:5184/swagger` (Development only)

## 🔑 Authentication

### Default Admin User

The system creates a default admin user on startup:

- **Username**: `admin`
- **Password**: `admin123`
- **Role**: `Admin`

### Login Process

1. **POST** `/api/auth/login` with credentials
2. Receive JWT token in response
3. Include token in `Authorization: Bearer {token}` header for protected endpoints

### Example Login Request

```json
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

### Example Response

```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiration": "2025-08-19T07:56:00Z",
    "user": {
      "id": 1,
      "username": "admin",
      "role": "Admin"
    }
  }
}
```

## 📚 API Endpoints

### 🔐 Authentication

| Method | Endpoint           | Description           | Auth Required |
| ------ | ------------------ | --------------------- | ------------- |
| POST   | `/api/auth/login`  | User authentication   | ❌            |
| GET    | `/api/auth/me`     | Get current user info | ✅            |
| GET    | `/api/auth/verify` | Verify token validity | ✅            |
| POST   | `/api/auth/logout` | User logout           | ✅            |

### 📦 Products

| Method | Endpoint             | Description        | Auth Required |
| ------ | -------------------- | ------------------ | ------------- |
| GET    | `/api/products`      | Get all products   | ✅            |
| GET    | `/api/products/{id}` | Get product by ID  | ✅            |
| POST   | `/api/products`      | Create new product | ✅            |
| PUT    | `/api/products/{id}` | Update product     | ✅            |
| DELETE | `/api/products/{id}` | Delete product     | ✅            |

### 📊 Inventory

| Method | Endpoint                   | Description               | Auth Required |
| ------ | -------------------------- | ------------------------- | ------------- |
| GET    | `/api/inventory`           | Get inventory movements   | ✅            |
| POST   | `/api/inventory/movement`  | Record inventory movement | ✅            |
| GET    | `/api/inventory/low-stock` | Get low stock products    | ✅            |

## 🧪 Testing the API

### Using HTTP Files

The project includes test files:

- `test-auth.http` - Authentication flow testing
- `server.http` - General API testing

### Using cURL

```bash
# Login
curl -X POST http://localhost:5184/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin123"}'

# Get products (replace {token} with actual token)
curl -X GET http://localhost:5184/api/products \
  -H "Authorization: Bearer {token}"
```

### Using Swagger UI

Visit `http://localhost:5184/swagger` in development mode for interactive API documentation.

## 📁 Project Structure

```
server/
├── Controllers/           # API Controllers
│   ├── AuthController.cs     # Authentication endpoints
│   ├── ProductsController.cs # Product management
│   └── InventoryController.cs # Inventory operations
├── Services/             # Business Logic Layer
│   ├── IAuthenticationService.cs
│   ├── AuthenticationService.cs
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IProductService.cs
│   ├── ProductService.cs
│   ├── IMovementService.cs
│   └── MovementService.cs
├── Repositories/         # Data Access Layer
│   ├── IProductRepository.cs
│   ├── ProductRepository.cs
│   ├── IMovementRepository.cs
│   ├── MovementRepository.cs
│   ├── IUserRepository.cs
│   └── UserRepository.cs
├── Models/              # Domain Entities
│   ├── Product.cs
│   ├── Movement.cs
│   └── User.cs
├── DTOs/               # Data Transfer Objects
│   ├── ProductDto.cs
│   ├── MovementDto.cs
│   ├── UserDto.cs
│   ├── AuthenticationDto.cs
│   └── ApiResponse.cs
├── Data/               # Database Context
│   ├── InventoryContext.cs
│   └── InventoryContextFactory.cs
├── Migrations/         # EF Core Migrations
├── Scripts/           # Database Scripts
└── Properties/        # Launch Settings
```

## ⚙️ Configuration

### JWT Settings

Configure JWT authentication in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YourSecretKey",
    "Issuer": "InventorySystemAPI",
    "Audience": "InventorySystemClient",
    "ExpirationHours": 8
  }
}
```

### Database Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventorySystemDb;Trusted_Connection=true;"
  },
  "UseInMemoryDatabase": false
}
```

## 🗄️ Database Schema

### Products Table

- `Id` (Primary Key)
- `Code` (Unique identifier)
- `Name` (Product name)
- `Category` (Product category)
- `Quantity` (Current stock)
- `Location` (Storage location)
- `RegistrationDate` (Creation date)

### Movements Table

- `Id` (Primary Key)
- `ProductId` (Foreign Key)
- `MovementType` (In/Out)
- `Quantity` (Movement amount)
- `Date` (Movement date)

### Users Table

- `Id` (Primary Key)
- `Username` (Unique)
- `PasswordHash` (BCrypt hashed)
- `Role` (Admin/Manager/User)
- `CreatedAt` (Account creation)
- `LastLoginAt` (Last login timestamp)
- `IsActive` (Account status)

## 🔧 Development

### Adding New Migrations

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Running Tests

```bash
dotnet test
```

### Code Formatting

The project follows standard C# coding conventions with:

- PascalCase for public members
- camelCase for private fields
- Meaningful naming conventions
- Proper async/await patterns

## 🚀 Deployment

### Building for Production

```bash
dotnet publish -c Release -o ./publish
```

### Environment Variables

Set these environment variables in production:

- `ASPNETCORE_ENVIRONMENT=Production`
- `ConnectionStrings__DefaultConnection=<your-connection-string>`
- `JwtSettings__SecretKey=<your-secret-key>`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/yourusername/inventory-system/issues) page
2. Create a new issue with detailed information
3. Include error messages and steps to reproduce

## 🎯 Roadmap

### Upcoming Features

- [ ] Advanced reporting and analytics
- [ ] Barcode scanning support
- [ ] Email notifications for low stock
- [ ] Multi-warehouse support
- [ ] Product images and attachments
- [ ] Audit logging
- [ ] Data export (Excel/CSV)
- [ ] Mobile app integration

---
