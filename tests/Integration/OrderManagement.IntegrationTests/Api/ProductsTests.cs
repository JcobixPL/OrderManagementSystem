using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.IntegrationTests.Infrastructure;
using OrderManagement.Modules.Identity.Infrastructure.Persistence;
using OrderManagement.Modules.Products.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace OrderManagement.IntegrationTests.Api;

public sealed class ProductsTests
    : IClassFixture<PostgreSqlContainerFixture>
{
    private readonly PostgreSqlContainerFixture _postgreSqlFixture;

    public ProductsTests(PostgreSqlContainerFixture postgreSqlContainerFixture)
    {
        _postgreSqlFixture = postgreSqlContainerFixture;
    }

    [Fact]
    public async Task CreateProduct_WithValidData_ReturnsCreated()
    {
        var connectionString = _postgreSqlFixture.Container.GetConnectionString();

        using var factory = new CustomWebApplicationFactory(connectionString);

        using var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<ProductsDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        var request = new
        {
            Sku = $"SKU-{Guid.NewGuid():N}",
            Name = "Integration Test Product",
            Description = "Product created by integration test",
            Price = 99.99m,
            Currency = "PLN"
        };

        var response = await client.PostAsJsonAsync("/api/products", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);


    }
}
