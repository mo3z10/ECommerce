#  E-Commerce Backend API

A scalable E-Commerce Backend built with **ASP.NET Core Web API**, **Entity Framework Core**, **SQL Server**, following modern backend development practices and design patterns.

---

#  Features

## Authentication & Authorization

* User Registration
* User Login
* JWT Authentication
* Role-Based Authorization
* Admin & Customer Roles
* Protected Endpoints

---

# Product Management

* Create Product
* Update Product
* Delete Product (Soft Delete)
* Get Product By Id
* Get All Products
* Inventory Management
* Optimistic Concurrency Control using RowVersion

---

# Customer Management

* Customer Registration
* Customer Profile Retrieval
* Customer Order History

---

# Shopping Cart

* Add Item To Cart
* Remove Item From Cart
* Update Item Quantity
* Clear Cart
* Get Customer Cart
* Get All Carts (Admin)

---

# Order Management

* Checkout Process
* Create Orders From Cart
* Order Status Tracking
* Get Customer Orders
* Get All Orders
* Update Order Status

---

# Inventory Management

* Automatic Stock Validation
* Automatic Stock Reduction After Checkout
* Prevent Ordering More Than Available Stock
* Automatic InStock Status Updates
* Low Stock Detection System

---
# Global Exception Handling

Implemented a centralized exception handling middleware to provide consistent API error responses.

Features:

* Centralized exception handling
* Consistent JSON error responses
* Proper HTTP status codes
* Error logging using ILogger
* Prevents leaking internal server details

Example response:

```json
{
    "statusCode":404,
    "message":"Product Not Found"
}
```
---

#  Background Processing (Hangfire)

Implemented using **Hangfire** for reliable background and scheduled task execution.

Features:

* Background job processing
* Recurring scheduled jobs
* Email processing in background
* Automated maintenance tasks
---

## Implemented Hangfire Jobs

### Order Confirmation Email

After successful checkout:

```
Customer Checkout
        |
        |
Create Order
        |
        |
Hangfire Background Job
        |
        |
Send Confirmation Email
```

The API does not wait for email sending.
The email is processed asynchronously in the background.

---

### Order Status Notification

When admin updates order status:

```
Update Order Status
        |
        |
Create Background Job
        |
        |
Send Status Update Email
```

Customers receive notifications when their order status changes.

---

### Abandoned Cart Cleanup

Recurring job to remove unused carts.

Example:

```
Every Day
    |
    |
Find carts with old activity
    |
    |
Remove abandoned cart items
```

---

### Low Stock Inventory Notification

Recurring inventory monitoring:

```
Daily Job
    |
    |
Check Product Quantity
    |
    |
Find Low Stock Products
    |
    |
Send Admin Notification Email
```

---

### Hangfire Dashboard

Added Hangfire monitoring dashboard:

Features:

* View running jobs
* View failed jobs
* View scheduled jobs
* Retry failed jobs
* Monitor background processing

---

#  Advanced Query Features

## Pagination

Supports efficient retrieval of large datasets.

Example:

```http
GET /api/products?pageNumber=1&pageSize=10
```

---

## Searching

Supports searching products by name.

Example:

```http
GET /api/products?search=ball
```

---

## Filtering

Supports:

* Price Range
* Stock Availability
* Order Status

Example:

```http
GET /api/products?minPrice=50&maxPrice=500
```

---

## Sorting

Supports sorting by:

* Name
* Price
* Quantity
* Date

Example:

```http
GET /api/products?sortBy=price&descending=true
```

---

# Soft Delete System

Implemented using:

* IsDeleted Flag
* Global Query Filters
* Audit Tracking

Deleted records remain in database but are hidden from normal users.

Administrators can access soft-deleted records when needed.

---

# Rate Limiting

Implemented using **ASP.NET Core Rate Limiting Middleware** to protect the API from abuse attacks, and excessive traffic.

## Features

* Fixed Window Limiter
* Sliding Window Limiter
* Token Bucket Limiter
* Concurrency Limiter
* Automatic HTTP 429 (Too Many Requests) responses
---

## Fixed Window Limiter
## Sliding Window Limiter
## Concurrency Limiter


## Custom Rejection Response

When a client exceeds the configured limit, the API returns:

```http
HTTP/1.1 429 Too Many Requests
```

Example response:

```json
{
    "message": "Too many requests. Please try again later."
}
```

---
# Real-Time Notifications (SignalR)
Features
Implemeneted Using SingleR integration to Add Real Time Notification and Connection
* Real-time order status updates
* Low stock alerts for administrators
* New customer registration notifications
* New order creation notifications
* User-specific notifications using SignalR User Connections
* Group-based notifications for administrators

---
# Payment Integration (Stripe)

Implemented Stripe payment integration 

Current Features:

* Create Payment Intent
* Retrieve Payment Status
* Cancel Payment Intent
* Stripe Webhook Endpoint
* Secure API Key Configuration using Environment Variables
* Automatic Payment Methods Support
Current Payment Flow

```
Client
    |
    |
Create Payment Intent
    |
    |
Stripe
    |
    |
Return Client Secret
    |
    |
Frontend Confirms Payment
    |
    |
Stripe Sends Webhook
    |
    |
Backend Verifies Payment
```
---

#  Redis Distributed Caching

Implemented using **Redis Distributed Cache** to improve performance and reduce database load.

Features:

* Redis Integration with ASP.NET Core
* Distributed caching using IDistributedCache
* Cache Products
* Cache Orders
* Cache Paginated Results
* Cache expiration policies
* Cache invalidation using versioning strategy

Caching flow:

```
Request
    |
    |
Check Redis Cache
    |
    |
Cache Exists?
    |
    |
Return Cached Data


Cache Miss
    |
    |
Load From Database
    |
    |
Store In Redis
    |
    |
Return Data
```

---
# Docker Support

The project is fully containerized using Docker Compose.

Containers:

- ASP.NET Core API
- SQL Server
- Redis

Run:

docker compose up --build




# Transaction Management

Checkout process uses database transactions.

Workflow:

```
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

```
Rollback Transaction
```

---

# Concurrency Handling

Implemented using EF Core RowVersion.

Prevents users from overwriting data modified by another user.

---

#  Design Patterns Used

## Generic Repository Pattern

Reusable database operations across entities.

## Unit Of Work Pattern

Coordinates repositories and manages transactions.

## Dependency Injection

Used throughout the application.

## DTO Pattern

Separates API models from database entities.

---

#  Security

* JWT Authentication
* Role-Based Authorization
* ASP.NET Identity
* Protected Endpoints
* Secure Password Hashing

---
# Automatic Seeding

The application automatically creates

- Admin Role
- Customer Role
- Administrator Account

---
# 🛠 Technologies Used

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* ASP.NET Identity
* JWT Bearer Authentication
* Redis Distributed Cache
* Hangfire Background Processing
* MailKit Email Service
* Stripe .Net
* LINQ
* Swagger / OpenAPI
* C#

---

#  Project Structure

```
ECommerce.Api
│
├── Controllers
├── Middleware
│
ECommerce.BIL
│
├── Services
├── DTOs
├── Background Jobs
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

#  Implemented Features
* Global Exception Handling Middleware
* ASP.NET Core Rate Limiting
* Fixed Window Limiter
* Sliding Window Limiter
* Token Bucket Limiter
* Concurrency Limiter
* JWT Authentication
* Role-Based Authorization
* ASP.NET Identity
* Generic Repository Pattern
* Unit Of Work Pattern
* DTO Pattern
* Soft Delete
* Global Query Filters
* Audit Logging
* Optimistic Concurrency (RowVersion)
* Transactions
* Product Management
* Customer Management
* Cart Management
* Order Management
* Inventory Validation
* Checkout Workflow
* Pagination
* Searching
* Filtering
* Sorting
* Redis Distributed Caching
* Hangfire Background Jobs
* Email Notifications
* Scheduled Maintenance Tasks
* Real Time Notifications Using SingleR
* Stripe Payment Integration
* Payment Intent Creation
* Payment Status Tracking
* Payment Cancellation
* Stripe Webhook Verification
* Docker
* Docker Compose

---

#  Future Improvements
## Cloud Deployment
* Azure App Service
* Azure SQL Database
* Azure Storage
* Production Hosting

## DevOps
* CI/CD Pipelines
* GitHub Actions

---
## To Run the Project

To run this project locally, follow the setup guide:

 [Setup Guide](HowToUse.md)
 ---
# Author

## Moaz Yasser

Backend .NET Developer

Skills:

* C#
* ASP.NET Core
* Entity Framework Core
* SQL Server
* Redis
* Hangfire
* Stripe
* SingleR
* Rate Limiting
* REST APIs
* Authentication & Authorization
* Design Patterns
* Database Design

---

This project is developed as a practical learning journey for building ready backend systems using the .NET ecosystem.
