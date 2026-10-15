using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;

namespace OrderManagement.IntegrationTests.Infrastructure;

public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    private static readonly string JwtSecretKey =
        Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64));

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var testSettings = new KeyValuePair<string, string?>[]
        {
            new("ConnectionStrings:Database", _connectionString),

            new("Jwt:Issuer", "OrderManagement.Api.Tests"),

            new("Jwt:Audience", "OrderManagement.Client.Tests"),

            new("Jwt:SecretKey", JwtSecretKey),

            new("Jwt:ExpirationMinutes", "60")
        };

        builder.ConfigureHostConfiguration(builder =>
        {
            builder.AddInMemoryCollection(testSettings);
        });

        return base.CreateHost(builder);
    }
}