using Npgsql;
using OrderManagement.Api.Exceptions;
using OrderManagement.Api.Infrastructure.Transactions;
using OrderManagement.Modules.Identity.Infrastructure;
using OrderManagement.Modules.Inventory.Infrastructure;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Infrastructure;
using OrderManagement.Modules.Products.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("Database")
    ?? throw new InvalidOperationException("Connection string 'Database' not found.");

builder.Services.AddScoped<NpgsqlConnection>(
    _ => new NpgsqlConnection(connectionString));

builder.Services.AddProductsModule(connectionString);
builder.Services.AddInventoryModule(connectionString);
builder.Services.AddOrdersModule(connectionString);
builder.Services.AddIdentityModule(connectionString);

builder.Services.AddScoped<IOrdersTransaction, OrdersTransaction>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
