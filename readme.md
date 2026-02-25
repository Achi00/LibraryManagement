# Library Management System API

RESTful API for managing a library system built with **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server**.  
The API supports managing books, authors, patrons, and borrow records.

---

# Features

- Full CRUD operations for books, authors, patrons, and borrow records
- Book search by title, author, or ISBN
- Pagination and filtering
- Borrow / return book workflow
- Overdue book tracking
- Book availability checks
- Structured error handling and logging
- Swagger API documentation

---

# Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- MS SQL Server
- Clean Architecture
- Dependency Injection
- Middleware pipeline
- Swagger / OpenAPI

---

# Domain Models

### Book

- Title, ISBN, Publication Year
- Description and Cover Image
- Quantity tracking
- Linked Author

### Author

- First Name / Last Name
- Biography
- Date of Birth
- List of authored books

### Patron

- Library member information
- Membership date
- Borrow history

### Borrow Record

- Borrow and return dates
- Due date tracking
- Status (Borrowed, Returned, Overdue)

---

# Architecture

The project follows **Clean Architecture** with clear separation of responsibilities:

```
API
Application / Services
Repositories
Data Access (EF Core)
Domain Models
```

Key practices used:

- Repository Pattern
- DTOs for request/response models
- Dependency Injection
- Custom Middleware (logging & exception handling)
- Entity Framework Core Code-First with migrations
- Database seeding for initial data

---

# Additional Capabilities

- Global exception handling
- Request/response logging
- Validation using FluentValidation
- Transaction support for multi-entity operations
- Pagination, filtering, and sorting support
