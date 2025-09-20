# Inventory Management System - Client

A modern web application built with Blazor WebAssembly for managing inventory systems.

## Features

- Product Management
- Inventory Tracking
- User Authentication
- Movement History
- Responsive Design with Bootstrap

## Technology Stack

- **Frontend**: Blazor WebAssembly (.NET 9.0)
- **UI Framework**: Bootstrap 5
- **HTTP Client**: System.Net.Http
- **Architecture**: Component-based architecture

## Project Structure

```
client/
├── Layout/          # Layout components
├── Models/          # Data models and DTOs
├── Pages/           # Razor pages/components
├── Services/        # Business logic services
├── wwwroot/         # Static assets
└── Program.cs       # Application entry point
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code

### Running the Application

1. Navigate to the client directory
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Run the application:
   ```bash
   dotnet run
   ```

## Development Guidelines

- Follow C# coding conventions
- Use meaningful variable and method names
- Implement proper error handling
- Keep components focused and single-purpose
- Use dependency injection for services

## Architecture

The application follows a clean architecture pattern:

- **Components**: Handle UI rendering and user interactions
- **Services**: Contain business logic and API communication
- **Models**: Define data structures and validation
- **Layout**: Provide consistent UI structure

## API Integration

The client communicates with the backend API through HTTP services:

- `ProductService`: Manages product operations
- `AuthService`: Handles authentication and authorization
