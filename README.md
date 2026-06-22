# 🛒 E-Commerce Backend API

A scalable E-Commerce Backend built with **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server** following modern backend development practices and design patterns.

---

## 🚀 Features

### Authentication & Authorization

- User Registration
- User Login
- JWT Authentication
- Role-Based Authorization
- Admin & Customer Roles
- Protected Endpoints

---

### Product Management

- Create Product
- Update Product
- Delete Product (Soft Delete)
- Get Product By Id
- Get All Products
- Inventory Management
- Optimistic Concurrency Control using RowVersion

---

### Customer Management

- Customer Registration
- Customer Profile Retrieval
- Customer Order History

---

### Shopping Cart

- Add Item To Cart
- Remove Item From Cart
- Update Item Quantity
- Clear Cart
- Get Customer Cart
- Get All Carts (Admin)

---

### Order Management

- Checkout Process
- Create Orders From Cart
- Order Status Tracking
- Get Customer Orders
- Get All Orders
- Update Order Status

---

### Inventory Management

- Automatic Stock Validation
- Automatic Stock Reduction After Checkout
- Prevent Ordering More Than Available Stock
- Automatic InStock Status Updates

---

## 🔎 Advanced Query Features

### Pagination

Supports efficient retrieval of large datasets.

Example:

```http
GET /api/products?pageNumber=1&pageSize=10
```

### Searching

Supports searching products by name.

Example:

```http
GET /api/products?search=ball
```

### Filtering

Supports filtering by:

- Price Range
- Stock Availability
- Order Status

Example:

```http
GET /api/products?minPrice=50&maxPrice=500
```

### Sorting

Supports sorting by:

- Name
- Price
- Quantity
- Date

Example:

```http
GET /api/products?sortBy=price&descending=true
```

---

## 🗑 Soft Delete System

Implemented using:

- IsDeleted Flag
- Global Query Filters
- Audit Tracking

Deleted records remain in the database but are hidden from normal users.

Administrators can access soft-deleted records when needed.

---

## 📝 Audit Logging

Every entity inherits from BaseEntity and stores:

```csharp
CreatedAt
CreatedBy

UpdatedAt
UpdatedBy

DeletedAt
DeletedBy
```

This provides complete tracking of data changes.

---

## 🔄 Transaction Management

The checkout process uses database transactions to ensure data consistency.

Workflow:

```text
Create Order
↓
Validate Inventory
↓
Reduce Product Stock
↓
Clear Cart
↓
Commit Transaction
```

If any step fails:

```text
Rollback Transaction
```

All changes are reverted automatically.

---

## ⚡ Concurrency Handling

Implemented using EF Core RowVersion.

Prevents users from overwriting data that has already been modified by another user.

---

## 🏗 Design Patterns Used

### Generic Repository Pattern

Provides reusable data access operations across entities.

### Unit Of Work Pattern

Coordinates repositories and manages transactions.

### Dependency Injection

Used throughout the application for loose coupling and maintainability.

### DTO Pattern

Separates API contracts from database entities.

---

## 🔐 Security

- JWT Authentication
- Role-Based Authorization
- ASP.NET Identity
- Protected Endpoints
- Secure Password Hashing

---

## 🛠 Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- JWT Bearer Authentication
- LINQ
- Swagger / OpenAPI
- C#

---

## 📂 Project Structure

```text
ECommerce.Api
│
├── Controllers
├── Middleware
│
ECommerce.BIL
│
├── Services
├── DTOs
│
ECommerce.DAL
│
├── Repositories
├── UnitOfWork
├── Configurations
├── Database
├── Models
```

---

## ✅ Implemented Features

- JWT Authentication
- Role-Based Authorization
- ASP.NET Identity
- Generic Repository Pattern
- Unit Of Work Pattern
- DTO Pattern
- Soft Delete
- Global Query Filters
- Audit Logging
- Optimistic Concurrency (RowVersion)
- Transactions
- Product Management
- Customer Management
- Cart Management
- Order Management
- Inventory Validation
- Checkout Workflow
- Pagination
- Searching
- Filtering
- Sorting

---

## 🎯 Future Improvements

### Real-Time Notifications

Using SignalR for:

- Order Status Updates
- Stock Updates
- Real-Time User Notifications

### Payment Integration

- Stripe Integration
- Secure Checkout Process
- Payment Verification

### Caching

- Redis Caching
- Faster Product Retrieval
- Reduced Database Load

### Cloud Deployment

- Azure App Service
- Azure SQL Database
- Azure Storage
- Production Hosting

### DevOps

- Docker
- CI/CD Pipelines
- GitHub Actions

### Additional Features

- Wishlist System
- Product Reviews & Ratings
- Email Notifications
- Refresh Tokens
- Advanced Reporting Dashboard

---

## 📈 Learning Goals

This project is continuously evolving to explore:

- Advanced ASP.NET Core Features
- Distributed Systems Concepts
- Cloud Computing
- Real-Time Applications
- Scalable Backend Architectures
- Production Deployment Strategies

---

##  Author

### Moaz Yasser

Backend .NET Developer

**Skills**

- C#
- ASP.NET Core
- Entity Framework Core
- SQL Server
- Operating System
- REST APIs
- Authentication & Authorization
- Design Patterns
- Database Design

---

 This project is being developed as a practical learning journey toward building production-ready backend systems using the .NET ecosystem.
