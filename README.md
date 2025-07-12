# MagicVilla_VillaAPI

MagicVilla_VillaAPI is a robust RESTful Web API built with ASP.NET Core (.NET 9, C# 13.0) for managing villa properties and villa numbers. It provides secure endpoints for CRUD operations, user authentication, and role-based authorization.

## Features

- **Villa Management**: Create, read, update, delete, and search villas.
- **Villa Number Management**: Manage villa numbers associated with villas.
- **User Authentication**: Register, login, and manage users.
- **Role-Based Authorization**: Secure endpoints for admin and user roles.
- **DTOs & AutoMapper**: Clean separation of domain models and data transfer objects.
- **Response Caching**: Optimized performance with response caching.
- **Error Handling**: Consistent API responses with detailed error messages.
- **Swagger/OpenAPI**: Integrated API documentation (if enabled).

## Technologies

- ASP.NET Core (.NET 9)
- C# 13.0
- Entity Framework Core
- AutoMapper
- JWT Authentication
- Swagger
- SQL Server 

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (or update connection string in `appsettings.json`)

### Installation

1. **Clone the repository**

2. **Configure the database**
   - Update the connection string in `appsettings.json` as needed.

3. **Apply migrations**

4. **Run the application**

5. **Access Swagger UI**
   - Navigate to `https://localhost:5001/swagger` in your browser or you can use postman.

## Usage

### Authentication

- Register and login via `/api/Users/register` and `/api/Users/login` to obtain a JWT token.
- Include the token in the `Authorization` header as `Bearer <token>` for protected endpoints.


## Project Structure

- `Controllers/` - API controllers for villas, villa numbers, and users.
- `Models/` - Domain models and DTOs.
- `Repository/` - Data access layer with repository pattern.
- `Data/` - Entity Framework DbContext.
- `AutoMapperConfig.cs` - AutoMapper profile configuration.
