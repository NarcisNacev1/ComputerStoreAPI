# Computer Store API

[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![API](https://img.shields.io/badge/Web%20API-REST-blue)](https://swagger.io/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean-green)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

A RESTful Web API for a computer store built with **ASP.NET Core 10** following **Clean Architecture** principles.

## 🚀 Features

| Feature | Description |
|---------|-------------|
| **Category Management** | Full CRUD operations for product categories |
| **Product Management** | Full CRUD operations for products with category associations |
| **Stock Import** | Import stock data from third-party systems (auto-creates missing categories/products) |
| **Basket Discount** | Smart discount calculation (5% when buying multiple items from same category) |
| **Error Handling** | Global exception handling with user-friendly messages |
| **API Documentation** | Swagger/OpenAPI for easy testing |

## 🛠️ Technology Stack

- **Framework:** .NET 10
- **API Style:** RESTful Web API
- **Database:** SQLite (Development) / SQL Server Ready
- **ORM:** Entity Framework Core 10
- **Mapping:** AutoMapper 12.0.1
- **Documentation:** Swagger/Swashbuckle
- **Testing:** xUnit, Moq

## 📁 Architecture (Clean Architecture)

```
┌─────────────────────────────────────────────┐
│           API Layer (Controllers)           │  ← HTTP requests/responses
├─────────────────────────────────────────────┤
│         Application Layer (Services)        │  ← Business logic & DTOs
├─────────────────────────────────────────────┤
│        Infrastructure Layer (Data)          │  ← Database & Repositories
├─────────────────────────────────────────────┤
│            Domain Layer (Core)              │  ← Entities & Interfaces
└─────────────────────────────────────────────┘
```

## 🏃‍♂️ Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or any C# IDE

### Run Locally

1. **Clone the repository**
   ```bash
   git clone https://github.com/NarcisNacev1/ComputerStoreAPI.git
   ```

2. **Open in Visual Studio**
   - Open `ComputerStore.sln`

3. **Set startup project**
   - Right-click `ComputerStore.API` → Set as Startup Project

4. **Run the application**
   - Press `F5` or click "Run"

5. **Open Swagger UI**
   - Navigate to `https://localhost:7168/swagger`

## 📡 API Endpoints

### Categories
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/categories` | Get all categories |
| GET | `/api/categories/{id}` | Get category by ID |
| POST | `/api/categories` | Create new category |
| PUT | `/api/categories/{id}` | Update category |
| DELETE | `/api/categories/{id}` | Delete category |

### Products
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create new product |
| PUT | `/api/products/{id}` | Update product |
| DELETE | `/api/products/{id}` | Delete product |

### Stock Management
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/stock/import` | Import stock data (auto-creates missing items) |

### Basket Discount
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/basket/calculate-discount` | Calculate discount for shopping basket |

## 📝 Sample API Calls

### Create a Category
```json
POST /api/categories
{
  "name": "Electronics",
  "description": "Electronic devices"
}
```

### Create a Product
```json
POST /api/products
{
  "name": "Gaming Laptop",
  "description": "High-performance laptop",
  "price": 1299.99,
  "categoryIds": [1]
}
```

### Import Stock
```json
POST /api/stock/import
[
  {
    "name": "Intel Core i9",
    "categories": ["CPU", "Electronics"],
    "price": 499.99,
    "quantity": 10
  }
]
```

### Calculate Basket Discount
```json
POST /api/basket/calculate-discount
{
  "items": [
    { "productId": 1, "quantity": 2 },
    { "productId": 2, "quantity": 1 }
  ]
}
```

## 💰 Discount Rules

| Scenario | Discount |
|----------|----------|
| Single product | 0% |
| Multiple products from same category | 5% on subsequent items |
| Different categories only | No discount |

**Example:** Buying 2 CPUs + 1 Keyboard → 5% discount on one CPU (second item in same category)

## 🧪 Running Tests

```bash
# Run unit tests
dotnet test ComputerStore.Tests.Unit

# Run integration tests  
dotnet test ComputerStore.Tests.Integration
```

## 📊 Database Schema

```
Categories ──┐
             │
             ├── ProductCategories (Junction Table)
             │
Products ────┘
```

- **Categories → Products:** Many-to-Many relationship
- Each product can belong to multiple categories
- Each category can contain multiple products

## 🔧 Environment Configuration

The application uses SQLite by default (no installation needed). 
To switch to SQL Server, update the connection string in `appsettings.json`.

## 📄 License

This project is for educational purposes.

## 👤 Author

**Narcis Nacev** - [GitHub](https://github.com/NarcisNacev1)
