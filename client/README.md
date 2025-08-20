# 🎨 Inventory System - Frontend Client

A modern **Blazor WebAssembly** frontend application for the Inventory Management System, featuring a responsive UI built with **MudBlazor** components and seamless API integration.

## 🌟 Overview

The frontend client provides an intuitive and responsive user interface for managing inventory operations. Built as a **Single Page Application (SPA)** using Blazor WebAssembly, it offers a native-like experience while running entirely in the browser.

### ✨ **Key Features**

- 🔐 **JWT Authentication** - Secure token-based authentication flow
- 📱 **Responsive Design** - Mobile-first approach with MudBlazor components
- ⚡ **Real-time Updates** - Live data synchronization with the backend API
- 🎨 **Material Design** - Beautiful UI following Material Design principles
- 🔄 **State Management** - Efficient client-side state handling
- 📊 **Dashboard Analytics** - Visual insights into inventory data
- 🌐 **Progressive Web App** - PWA capabilities for offline usage

## 🏗️ **Architecture**

```
┌─────────────────────────────────────────────────┐
│                 Browser                         │
├─────────────────────────────────────────────────┤
│  Blazor WebAssembly Client (.NET 9.0)          │
│  ┌─────────────┐  ┌─────────────┐               │
│  │    Pages    │  │   Layout    │               │
│  │             │  │             │               │
│  │ • Home      │  │ • MainLayout│               │
│  │ • Products  │  │ • NavMenu   │               │
│  │ • Login     │  │             │               │
│  └─────────────┘  └─────────────┘               │
│  ┌─────────────┐  ┌─────────────┐               │
│  │  Services   │  │   Models    │               │
│  │             │  │             │               │
│  │ • AuthSvc   │  │ • ProductDto│               │
│  │ • ProductSvc│  │ • UserDto   │               │
│  │ • HttpClient│  │ • AuthModels│               │
│  └─────────────┘  └─────────────┘               │
└─────────────────────────────────────────────────┘
             │ HTTP/HTTPS │
             ▼
┌─────────────────────────────────────────────────┐
│         Backend API Server                     │
│         (ASP.NET Core)                          │
└─────────────────────────────────────────────────┘
```

## 🚀 **Quick Start**

### **Prerequisites**

- **.NET 9.0 SDK** or later
- **Modern web browser** (Chrome, Firefox, Safari, Edge)
- **Backend API running** on `http://localhost:5184`

### **Installation & Setup**

```bash
# Clone the repository (if not already done)
git clone https://github.com/manoguzman/inventory-system.git
cd inventory-system/client

# Restore NuGet packages
dotnet restore

# Run in development mode
dotnet run

# Or run with hot reload
dotnet watch run
```

The application will be available at: **http://localhost:5103**

### **Build for Production**

```bash
# Build optimized release version
dotnet publish -c Release -o ./dist

# The published files in ./dist can be deployed to any static web server
```

## 📁 **Project Structure**

```
client/
├── 📄 Program.cs                 # Application entry point
├── 📄 App.razor                  # Root component
├── 📄 _Imports.razor             # Global using statements
├── 📄 GlobalUsings.cs            # Global C# usings
├── 📄 client.csproj              # Project configuration
│
├── 📁 Pages/                     # Application pages
│   ├── Home.razor                # Dashboard page
│   └── Products.razor            # Product management
│
├── 📁 Layout/                    # Layout components
│   ├── MainLayout.razor          # Main application layout
│   ├── MainLayout.razor.css      # Layout styles
│   ├── NavMenu.razor             # Navigation menu
│   └── NavMenu.razor.css         # Navigation styles
│
├── 📁 Services/                  # HTTP services
│   ├── AuthService.cs            # Authentication service
│   └── ProductService.cs         # Product API service
│
├── 📁 Models/                    # Data models
│   ├── AuthModels.cs             # Authentication DTOs
│   ├── ProductDto.cs             # Product data models
│   ├── MovementDto.cs            # Inventory movement models
│   └── UserDto.cs                # User data models
│
├── 📁 wwwroot/                   # Static assets
│   ├── index.html                # Main HTML file
│   ├── favicon.png               # Favicon
│   ├── icon-192.png              # PWA icon
│   └── css/                      # Custom styles
│       └── app.css               # Application styles
│
└── 📁 Properties/                # Configuration
    └── launchSettings.json       # Launch profiles
```

## 🎨 **Technology Stack**

### **Core Framework**

- **Blazor WebAssembly** - .NET 9.0 client-side framework
- **C#** - Primary programming language
- **Razor Pages** - Component markup syntax

### **UI Components**

- **MudBlazor 8.11.0** - Material Design component library
- **Material Design Icons** - Comprehensive icon set
- **CSS Grid & Flexbox** - Modern layout techniques

### **HTTP & State Management**

- **System.Net.Http.Json** - HTTP client with JSON support
- **HttpClient** - API communication
- **Dependency Injection** - Built-in DI container

### **Development Tools**

- **Hot Reload** - Real-time development experience
- **Browser Developer Tools** - Debugging support
- **Source Maps** - Debug original C# code in browser

## 🔧 **Configuration**

### **API Configuration**

The client is configured to communicate with the backend API. Update the base URL in `Program.cs`:

```csharp
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5184/") // Update this URL
});
```

### **Environment Configuration**

Configure different environments in `Properties/launchSettings.json`:

```json
{
  "profiles": {
    "Development": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "http://localhost:5103",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "Production": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "http://localhost:5103",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Production"
      }
    }
  }
}
```

## 🖥️ **Pages & Components**

### **🏠 Home Page (`/`)**

- Dashboard with key metrics
- Quick navigation to main features
- Recent activity overview
- Inventory status cards

### **📦 Products Page (`/products`)**

- Product listing with search and filtering
- Add/Edit/Delete product operations
- Category and location management
- Bulk operations support

### **🔧 Layout Components**

#### **MainLayout**

- Application shell with responsive design
- Sidebar navigation for desktop
- Mobile-friendly hamburger menu
- MudBlazor theme integration

#### **NavMenu**

- Hierarchical navigation structure
- Role-based menu items
- Active page highlighting
- Responsive collapse/expand

## 🔐 **Authentication Flow**

### **Login Process**

1. User enters credentials on login form
2. `AuthService.LoginAsync()` sends request to API
3. API returns JWT token on successful authentication
4. Token stored in browser's local storage
5. Subsequent API calls include Bearer token
6. Automatic token refresh handling

### **Authorization**

```csharp
// Example of protected API call
public async Task<List<ProductDto>> GetProductsAsync()
{
    var response = await _httpClient.GetAsync("api/products");
    // Token automatically included via DI configuration
    return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
}
```

## 🎛️ **Services**

### **AuthService**

- User authentication and logout
- Token management
- User session state
- Permission validation

### **ProductService**

- Product CRUD operations
- Search and filtering
- Category management
- API error handling

## 🎨 **Styling & Theming**

### **MudBlazor Theme**

The application uses MudBlazor's Material Design theming:

```razor
<MudThemeProvider />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />
```

### **Custom Styles**

Custom CSS is located in `wwwroot/css/app.css`:

```css
/* Custom application styles */
.dashboard-card {
  transition: transform 0.2s;
}

.dashboard-card:hover {
  transform: translateY(-2px);
}
```

## 🚀 **Development**

### **Running in Development**

```bash
# Start with hot reload
dotnet watch run

# Start without hot reload
dotnet run

# Build only
dotnet build
```

### **Debugging**

1. **Browser DevTools**: Press F12 to access browser debugging
2. **Blazor DevTools**: Install Blazor DevTools browser extension
3. **Source Maps**: Debug C# code directly in browser
4. **Console Logging**: Use `Console.WriteLine()` for debugging

### **Hot Reload**

The application supports hot reload for:

- Razor markup changes
- CSS modifications
- C# code updates (limited)

## 📱 **Progressive Web App (PWA)**

### **PWA Features**

- **App Manifest**: Defined in `wwwroot/manifest.json`
- **Service Worker**: For offline capabilities
- **Install Prompt**: "Add to Home Screen" functionality
- **Offline Support**: Cached resources for offline usage

### **PWA Configuration**

```json
{
  "name": "Inventory Management System",
  "short_name": "Inventory",
  "description": "Modern inventory management solution",
  "start_url": "/",
  "display": "standalone",
  "theme_color": "#594ae2",
  "background_color": "#ffffff",
  "icons": [
    {
      "src": "icon-192.png",
      "sizes": "192x192",
      "type": "image/png"
    }
  ]
}
```

## 🧪 **Testing**

### **Manual Testing**

```bash
# Start the application
dotnet run

# Test scenarios:
# 1. Navigate to http://localhost:5103
# 2. Test login with: admin/admin123
# 3. Verify dashboard loads correctly
# 4. Test product management features
# 5. Test responsive design on mobile
```

### **Browser Compatibility**

✅ **Supported Browsers:**

- Chrome 90+
- Firefox 90+
- Safari 14+
- Edge 90+

## 📦 **Deployment**

### **Static Hosting**

```bash
# Build for production
dotnet publish -c Release -o ./dist

# Deploy to static hosting providers:
# - Netlify
# - Vercel
# - GitHub Pages
# - Azure Static Web Apps
# - AWS S3 + CloudFront
```

### **Docker Deployment**

```dockerfile
# Dockerfile for containerized deployment
FROM nginx:alpine
COPY dist/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### **Configuration for Production**

Update API base URL for production:

```csharp
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://your-api-domain.com/")
});
```

## 🔧 **Troubleshooting**

### **Common Issues**

#### **API Connection Failed**

```
Error: Failed to fetch from 'http://localhost:5184/api/...'
```

**Solution**: Ensure backend API is running and CORS is configured

#### **Authentication Failed**

```
Error: 401 Unauthorized
```

**Solution**: Check JWT token validity and API endpoint authentication

#### **Hot Reload Not Working**

```
Error: Hot reload session ended
```

**Solution**: Restart with `dotnet watch run`

### **Performance Optimization**

1. **Lazy Loading**: Implement lazy loading for large components
2. **Virtual Scrolling**: Use for large data lists
3. **Image Optimization**: Optimize static assets
4. **Bundle Analysis**: Analyze bundle size with `dotnet publish`

## 🤝 **Contributing**

### **Development Guidelines**

1. **Component Structure**: Follow Blazor component conventions
2. **Styling**: Use MudBlazor components first, custom CSS sparingly
3. **State Management**: Keep state management simple and predictable
4. **API Integration**: Use services for all API communications
5. **Error Handling**: Implement proper error boundaries

### **Code Style**

```csharp
// Use meaningful component names
@page "/products"
@using client.Services
@inject ProductService ProductService

// Follow async/await patterns
private async Task LoadProductsAsync()
{
    try
    {
        products = await ProductService.GetProductsAsync();
    }
    catch (Exception ex)
    {
        // Handle error appropriately
        errorMessage = "Failed to load products";
    }
}
```

---

<div align="center">

[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?style=for-the-badge&logo=blazor)](https://blazor.net/)
[![MudBlazor](https://img.shields.io/badge/MudBlazor-8.11.0-594AE2?style=for-the-badge)](https://mudblazor.com/)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Material Design](https://img.shields.io/badge/Material-Design-757575?style=for-the-badge&logo=material-design)](https://material.io/)

</div>
