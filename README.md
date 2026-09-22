# Order Management System

A backend **Order Management System** built with .NET 10 and ASP.NET Core.

The project is being developed as a more advanced backend application focused on modular architecture, clean separation of responsibilities and real-world order processing.

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
- GitHub Actions

## Architecture

The application is organized as a **Modular Monolith**.

Each business module owns its own domain, application and infrastructure layers while being hosted by a common ASP.NET Core API.

Current structure:

```text
src/
├── OrderManagement.Api
└── Modules/
    ├── Products/
    │   ├── OrderManagement.Modules.Products.Domain
    │   ├── OrderManagement.Modules.Products.Application
    │   └── OrderManagement.Modules.Products.Infrastructure
    └── Inventory/
        ├── OrderManagement.Modules.Inventory.Domain
        ├── OrderManagement.Modules.Inventory.Application
        └── OrderManagement.Modules.Inventory.Infrastructure
```

## Implemented Features

### Products Module

Currently implemented:

- Create product
- Get product by ID
- Paginated product listing
- Search, filtering and sorting
- Change product price
- Rename product
- Activate / deactivate product
- Duplicate SKU detection

### Inventory Module

Currently implemented:

- Create inventory item
- Get inventory by product ID
- Paginated inventory listing
- Filtering and sorting
- Add stock
- Reserve stock
- Release stock reservation
- Confirm stock removal
- Inventory validation and business conflict handling

### Infrastructure

- PostgreSQL persistence with EF Core migrations
- Docker Compose
- FluentValidation
- Global exception handling
- ProblemDetails responses
- GitHub Actions CI pipeline

## Docker

PostgreSQL runs in Docker using Docker Compose.

```bash
docker compose up -d
```

Sensitive configuration is kept outside the repository using environment variables and .NET User Secrets.

## Planned Development

Planned next steps include:

- Orders module
- Payments module
- Identity and authorization
- Unit and integration tests
- Testcontainers
- RabbitMQ
- .NET Worker Service
- Redis
- Outbox Pattern
- Health Checks
- Structured logging
- Azure deployment
- LLM API integration

## Goal

The goal of the project is to build a backend system that goes beyond a simple CRUD application and demonstrates practical backend engineering concepts such as modular architecture, CQRS, messaging, concurrency handling, testing, Docker, CI/CD and cloud deployment.
