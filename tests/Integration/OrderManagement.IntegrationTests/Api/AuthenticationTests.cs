using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.IntegrationTests.Infrastructure;
using OrderManagement.Modules.Identity.Infrastructure.Persistence;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace OrderManagement.IntegrationTests.Api;

public sealed class AuthenticationTests
    : IClassFixture<PostgreSqlContainerFixture>
{
    private readonly PostgreSqlContainerFixture _postgreSqlFixture;

    public AuthenticationTests(PostgreSqlContainerFixture postgreSqlFixture)
    {
        _postgreSqlFixture = postgreSqlFixture;
    }

    [Fact]
    public async Task GetMe_WithoutToken_ReturnsUnauthorized()
    {
        var connectionString = _postgreSqlFixture.Container.GetConnectionString();

        using var factory = new CustomWebApplicationFactory(connectionString);

        using var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });

        var response = await client.GetAsync("/api/auth/me");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsCreated()
    {
        var connectionString =
            _postgreSqlFixture.Container.GetConnectionString();

        using var factory =
            new CustomWebApplicationFactory(connectionString);

        using var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<IdentityDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        var request = new
        {
            Email = $"integration-{Guid.NewGuid():N}@example.com",
            Password = "Password123"
        };

        var response =
            await client.PostAsJsonAsync(
                "/api/auth/register",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        using var verificationScope =
    factory.Services.CreateScope();

        var verificationDbContext =
            verificationScope.ServiceProvider
                .GetRequiredService<IdentityDbContext>();

        var savedUser =
            await verificationDbContext.Users
                .AsNoTracking()
                .SingleAsync(x => x.Email == request.Email.ToLowerInvariant());

        Assert.Equal(request.Email.ToLowerInvariant(), savedUser.Email);
        Assert.NotEqual(request.Password, savedUser.PasswordHash);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsConflict()
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
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        var email = $"{Guid.NewGuid():N}@example.com";

        var request = new
        {
            Email = email,
            Password = "Password123"
        };

        var response1 = await client.PostAsJsonAsync("/api/auth/register", request);

        Assert.Equal(HttpStatusCode.Created, response1.StatusCode);

        var response2 = await client.PostAsJsonAsync("/api/auth/register", request);

        Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
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
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        var email = $"{Guid.NewGuid():N}@example.com";

        var registerRequest = new
        {
            Email = email,
            Password = "Password123"
        };

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", registerRequest);

        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);

        var loginRequest = new
        {
            Email = email,
            Password = "Password1234"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.Unauthorized, loginResponse.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
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
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        var email = $"{Guid.NewGuid():N}@example.com";

        var credentials = new
        {
            Email = email,
            Password = "Password123"
        };

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", credentials);

        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", credentials);

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginResult);
        Assert.False(string.IsNullOrWhiteSpace(loginResult.AccessToken));
    }

    [Fact]
    public async Task GetMe_WithValidCredentialsAndToken_ReturnsOk()
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
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        var email = $"{Guid.NewGuid()}@email.com";

        var credentials = new
        {
            Email = email,
            Password = "Password123"
        };

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", credentials);

        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", credentials);

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(loginResult);
        Assert.False(string.IsNullOrWhiteSpace(loginResult.AccessToken));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult.AccessToken);

        var getMeResponse = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.OK, getMeResponse.StatusCode);

        var getMeResult = await getMeResponse.Content.ReadFromJsonAsync<GetMeResponse>();

        Assert.NotNull(getMeResult);
        Assert.Equal(email, getMeResult.Email);
        Assert.Equal("Customer", getMeResult.Role);
    }

    private sealed record LoginResponse(string AccessToken);

    private sealed record GetMeResponse(
        Guid Id,
        string Email,
        string Role);
}
