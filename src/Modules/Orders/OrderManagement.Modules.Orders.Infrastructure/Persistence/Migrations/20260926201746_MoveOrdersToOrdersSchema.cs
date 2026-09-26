using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.Modules.Orders.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveOrdersToOrdersSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "products",
                newName: "Orders",
                newSchema: "orders");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                schema: "products",
                newName: "OrderItems",
                newSchema: "orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "products");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "orders",
                newName: "Orders",
                newSchema: "products");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                schema: "orders",
                newName: "OrderItems",
                newSchema: "products");
        }
    }
}
