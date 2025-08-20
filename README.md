# 📦 Inventory Management System

A modern inventory management system built with **ASP.NET Core**, featuring JWT authentication, real-time inventory tracking, and comprehensive product management.

## 🌟 Overview

This inventory management system provides a complete solution for businesses to track products, manage stock levels, and monitor inventory movements with a secure, role-based access control system.

### 🎯 **Key Features**

- 🔐 **Secure Authentication** - JWT-based authentication with role management
- 📊 **Real-time Inventory** - Live stock tracking and movement history
- 🏷️ **Product Management** - Complete CRUD operations with categories and locations
- 👥 **User Management** - Role-based access control (Admin, Manager, User)
- **Advanced Search** - Filter and search products by multiple criteria
- 📈 **Analytics Dashboard** - Insights into inventory trends and patterns

## 🏗️ **System Architecture**

```
┌─────────────────┐    ┌─────────────────┐
│    Backend      │    │    Database     │
│  (ASP.NET Core) │◄──►│  (SQL Server)   │
│                 │    │                 │
│ • REST API      │    │ • Entity Data   │
│ • Authentication│    │ • Relationships │
│ • Business Logic│    │ • Constraints   │
└─────────────────┘    └─────────────────┘
```

## 📋 **Prerequisites**

- **.NET 9.0 SDK** or later
- **SQL Server** (or SQL Server LocalDB)
- **Visual Studio 2022** or **VS Code**

## 🚀 **Quick Start**

### **1. Clone the Repository**

```bash
git clone https://github.com/manoguzman/inventory-system.git
cd inventory-system
```

### **2. Setup**

```bash
cd server
dotnet restore
dotnet run
```

The API will be available at `http://localhost:5184`

## 🔧 **Project Structure**

```
inventory-system/
├── 📁 server/                 # Backend (ASP.NET Core)
│   ├── Controllers/           # API Controllers
│   ├── Services/             # Business Logic
│   ├── Repositories/         # Data Access Layer
│   ├── Models/               # Domain Entities
│   ├── DTOs/                 # Data Transfer Objects
│   ├── Data/                 # EF Core Context
│   ├── Migrations/           # Database Migrations
│   └── Scripts/              # Database Scripts
│
├── 📁 docs/                   # Documentation
├── 📄 README.md              # This file
└── 📄 .gitignore             # Git ignore rules
```

## 🖥️ **Backend (ASP.NET Core)**

### **🔥 Features**

- ✅ **JWT Authentication** with BCrypt password hashing
- ✅ **RESTful API** with OpenAPI/Swagger documentation
- ✅ **Entity Framework Core** with Code-First migrations
- ✅ **Repository Pattern** for clean architecture
- ✅ **Service Layer** for business logic separation
- ✅ **Role-based Authorization** (Admin, Manager, User)
- ✅ **In-Memory & SQL Server** database support
- ✅ **CORS Configuration** for cross-origin requests

### **📚 API Endpoints**

#### **🔐 Authentication**

| Method | Endpoint           | Description           |
| ------ | ------------------ | --------------------- |
| `POST` | `/api/auth/login`  | User authentication   |
| `GET`  | `/api/auth/me`     | Get current user info |
| `GET`  | `/api/auth/verify` | Verify token validity |
| `POST` | `/api/auth/logout` | User logout           |

#### **📦 Products**

| Method   | Endpoint             | Description        |
| -------- | -------------------- | ------------------ |
| `GET`    | `/api/products`      | Get all products   |
| `GET`    | `/api/products/{id}` | Get product by ID  |
| `POST`   | `/api/products`      | Create new product |
| `PUT`    | `/api/products/{id}` | Update product     |
| `DELETE` | `/api/products/{id}` | Delete product     |

#### **📊 Inventory**

| Method | Endpoint                   | Description               |
| ------ | -------------------------- | ------------------------- |
| `GET`  | `/api/inventory`           | Get inventory movements   |
| `POST` | `/api/inventory/movement`  | Record inventory movement |
| `GET`  | `/api/inventory/low-stock` | Get low stock products    |

### **🔧 Configuration**

**Database Configuration** (`appsettings.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventorySystemDb;Trusted_Connection=true;"
  },
  "UseInMemoryDatabase": false,
  "JwtSettings": {
    "SecretKey": "YourSecretKey",
    "Issuer": "InventorySystemAPI",
    "Audience": "InventorySystemClient",
    "ExpirationHours": 8
  }
}
```

### **🧪 Testing the Backend**

```bash
cd server

# Run the application
dotnet run

# Test authentication
curl -X POST http://localhost:5184/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin123"}'
```

**Default Admin User:**

- Username: `admin`
- Password: `admin123`
- Role: `Admin`

## 📊 **Database Schema**

### **Core Tables**

```sql
Users
├── Id (PK)
├── Username (Unique)
├── PasswordHash (BCrypt)
├── Role (Admin/Manager/User)
├── CreatedAt
├── LastLoginAt
└── IsActive

Products
├── Id (PK)
├── Code (Unique)
├── Name
├── Category
├── Quantity
├── Location
└── RegistrationDate

Movements
├── Id (PK)
├── ProductId (FK)
├── MovementType (In/Out)
├── Quantity
└── Date
```

## 🧪 **Testing**

### **Backend Testing**

```bash
cd server
dotnet test                    # Run unit tests
dotnet run                     # Start API server
```

**Test Files Included:**

- `test-auth.http` - Authentication flow testing
- `server.http` - General API testing

## 🚀 **Deployment**

### **Backend Deployment**

```bash
cd server
dotnet publish -c Release -o ./publish
```

### **Docker Support** _(Planned)_

```bash
docker-compose up -d           # Start all services
```

## 🔐 **Security Features**

- ✅ **JWT Authentication** with secure token generation
- ✅ **BCrypt Password Hashing** for user credentials
- ✅ **Role-based Authorization** for endpoint protection
- ✅ **CORS Configuration** for secure cross-origin requests
- ✅ **Input Validation** with data annotations
- 🔲 **Rate Limiting** (planned)
- 🔲 **API Key Authentication** (planned)
- 🔲 **Audit Logging** (planned)

## 📈 **Roadmap**

### **Phase 1: Core Backend** ✅ _COMPLETED_

- [x] Authentication system
- [x] Product management
- [x] Inventory tracking
- [x] User management
- [x] API documentation

### **Phase 2: Advanced Features** 📋 _PLANNED_

- [ ] Real-time notifications
- [ ] Advanced reporting
- [ ] Barcode scanning
- [ ] Multi-warehouse support
- [ ] API rate limiting
- [ ] Automated testing

### **Phase 3: Production Features** 🎯 _FUTURE_

- [ ] Docker containerization
- [ ] CI/CD pipeline
- [ ] Load balancing
- [ ] Monitoring & logging
- [ ] Performance optimization

## 🤝 **Contributing**

We welcome contributions! Please follow these steps:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** your changes (`git commit -m 'Add some amazing feature'`)
4. **Push** to the branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

### **Development Guidelines**

- Follow **C# coding conventions** for backend
- Write **unit tests** for new features
- Update **documentation** for API changes
- Follow **commit message conventions**

---

<div align="center">

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens)](https://jwt.io/)

</div>
