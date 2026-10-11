# Order Management System

A backend **Order Management System** built with .NET 10 and ASP.NET Core.

The project is developed as a modular backend application focused on clean architecture, domain logic, transactional consistency and real-world order processing.

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
    └── Identity/
        ├── OrderManagement.Modules.Identity.Domain
        ├── OrderManagement.Modules.Identity.Application
        └── OrderManagement.Modules.Identity.Infrastructure
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

- Create order
- Get order by ID
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
- Order cancellation
- Automatic stock reservation release on cancellation
- Stock removal confirmation on shipment
- Invalid status transition handling

### Testing

- Unit tests for Products domain and application logic
- Unit tests for Inventory domain and application logic
- Unit tests for Orders domain
- Unit tests for key Orders application handlers
- FluentValidation tests

### Infrastructure

- PostgreSQL persistence with EF Core migrations
- Separate database schemas per module
- Docker Compose
- FluentValidation
- Global exception handling
- ProblemDetails responses
- GitHub Actions CI pipeline
- Transaction coordination across multiple EF Core DbContexts

## Identity Module

Currently in development.

Planned scope:

- User registration
- Password hashing
- Login
- JWT authentication
- Role-based authorization
- Current user context
- Integration with Orders

## Docker

PostgreSQL runs in Docker using Docker Compose.

```bash
docker compose up -d
```

Sensitive configuration is kept outside the repository using environment variables and .NET User Secrets.

## Planned Development

- Complete Identity and JWT authentication
- Payments module
- Integration tests
- Testcontainers
- RabbitMQ
- .NET Worker Service
- Outbox Pattern
- Health Checks
- Structured logging
- Redis where justified
- Azure deployment
- CI/CD deployment pipeline
- .NET Aspire
- LLM API integration

## Goal

The goal of the project is to build a backend system that goes beyond a simple CRUD application and demonstrates practical backend engineering concepts such as modular architecture, domain-driven design, CQRS, transactional consistency, validation, testing, asynchronous messaging, Docker, CI/CD and cloud deployment.
