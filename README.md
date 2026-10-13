# Order Management System

A backend **Order Management System** built with .NET 10 and ASP.NET Core.

The project is developed as a modular backend application focused on clean architecture, domain logic, transactional consistency, security and real-world order processing.

> **Status:** Work in progress

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker
- MediatR
- FluentValidation
- CQRS
- Modular Monolith
- JWT Authentication
- Swagger / OpenAPI
- xUnit
- Moq
- GitHub Actions

## Architecture

The application is organized as a **Modular Monolith**.

Each business module owns its own Domain, Application and Infrastructure layers while being hosted by a common ASP.NET Core API.

Current structure:

```text
src/
├── OrderManagement.Api
└── Modules/
    ├── Products/
    │   ├── OrderManagement.Modules.Products.Domain
    │   ├── OrderManagement.Modules.Products.Application
    │   └── OrderManagement.Modules.Products.Infrastructure
    │
    ├── Inventory/
    │   ├── OrderManagement.Modules.Inventory.Domain
    │   ├── OrderManagement.Modules.Inventory.Application
    │   └── OrderManagement.Modules.Inventory.Infrastructure
    │
    ├── Orders/
    │   ├── OrderManagement.Modules.Orders.Domain
    │   ├── OrderManagement.Modules.Orders.Application
    │   └── OrderManagement.Modules.Orders.Infrastructure
    │
    ├── Identity/
    │   ├── OrderManagement.Modules.Identity.Domain
    │   ├── OrderManagement.Modules.Identity.Application
    │   └── OrderManagement.Modules.Identity.Infrastructure
    │
    └── Payments/
        ├── OrderManagement.Modules.Payments.Domain
        ├── OrderManagement.Modules.Payments.Application
        └── OrderManagement.Modules.Payments.Infrastructure
```

## Implemented Features

### Products Module

- Create product
- Get product by ID
- Paginated product listing
- Search, filtering and sorting
- Change product price
- Rename product
- Activate / deactivate product
- Duplicate SKU detection

### Inventory Module

- Create inventory item
- Get inventory by product ID
- Paginated inventory listing
- Filtering and sorting
- Add stock
- Reserve stock
- Release stock reservation
- Confirm stock removal
- Inventory validation and business conflict handling

### Orders Module

- Create order for the authenticated customer
- Get order by ID
- Order ownership validation
- Product data snapshot at order creation
- Stock reservation during order creation
- Transactional consistency between Orders and Inventory
- Order lifecycle:
  - AwaitingPayment
  - Paid
  - Processing
  - ReadyForShipment
  - Shipped
  - Delivered
  - Cancelled
  - Expired
- Order cancellation
- Automatic stock reservation release on cancellation
- Stock removal confirmation on shipment
- Invalid status transition handling
- Role-based authorization for operational order actions

### Identity Module

- User registration
- Password hashing
- Login
- JWT authentication
- Role-based authorization
- Current user context
- Authenticated user profile endpoint
- Integration with Orders and Payments
- Customer ownership validation

### Payments Module

- Create payment for an order
- Payment amount and currency resolved from Orders
- Payment ownership validation
- Payment status lifecycle
- Payment attempts
- Retry support after failed payment attempts
- Prevent multiple pending attempts
- Fail payment attempt with failure reason
- Complete payment attempt with external payment ID
- Automatic payment completion
- Automatic transition of Order to `Paid`
- Get payment details by ID
- Payment attempt history
- Invalid payment state transition handling
- Payment access control

## Testing

- Unit tests for Products domain and application logic
- Unit tests for Inventory domain and application logic
- Unit tests for Orders domain and application handlers
- Unit tests for Identity registration and login flows
- Unit tests for Payments domain
- Unit tests for Payments application handlers
- Ownership and authorization behavior tests
- FluentValidation tests
- Automated test execution in GitHub Actions

## Infrastructure

- PostgreSQL persistence with EF Core migrations
- Separate database schemas per module
- Docker Compose
- FluentValidation
- Global exception handling
- ProblemDetails responses
- JWT Bearer authentication
- Role-based authorization
- GitHub Actions CI pipeline
- Transaction coordination across multiple EF Core DbContexts
- Cross-module communication through application abstractions and MediatR

## Payment Flow

```text
Order
  ↓
AwaitingPayment
  ↓
Payment
  ↓
PaymentAttempt
  ├── Failed
  │     ↓
  │   Retry
  │     ↓
  │   New PaymentAttempt
  │
  └── Completed
        ↓
      Payment Completed
        ↓
      Order Paid
```

Each order has one payment while individual payment retries are represented as separate payment attempts.

## Security

The API uses JWT authentication and role-based authorization.

Current security rules include:

- Customers can create orders only for themselves
- Customers can access only their own orders
- Customers can create payments only for their own orders
- Customers can operate only on their own payments and payment attempts
- Employee and Admin roles are used for operational order actions
- Passwords are securely hashed
- Authentication secrets are stored outside the repository

## Docker

PostgreSQL runs in Docker using Docker Compose.

```bash
docker compose up -d
```

Sensitive configuration is kept outside the repository using environment variables and .NET User Secrets.

## CI

GitHub Actions automatically runs:

```text
Restore
↓
Build
↓
Unit Tests
```

for changes pushed to the repository and pull requests.

## Planned Development

- Integration tests
- Testcontainers
- RabbitMQ
- .NET Worker Service
- Outbox Pattern
- Health Checks
- Structured logging
- Redis where justified
- Dockerization of the full application
- Azure deployment
- CI/CD deployment pipeline
- .NET Aspire
- LLM API integration

## Goal

The goal of the project is to build a backend system that goes beyond a simple CRUD application and demonstrates practical backend engineering concepts such as modular architecture, domain-driven design, CQRS, transactional consistency, authentication, authorization, payment processing, retry mechanisms, testing, asynchronous messaging, Docker, CI/CD and cloud deployment.
