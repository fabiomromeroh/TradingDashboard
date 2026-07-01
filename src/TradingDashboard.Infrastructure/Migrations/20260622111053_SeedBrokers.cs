using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedBrokers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brokers",
                columns: new[] { "Id", "CreatedAt", "DisplayName", "Name", "SupportedImportFormat", "UpdatedAt", "Website" },
                values: new object[] { new Guid("c3a2b8d9-5f1a-4b6d-9f2e-1a2b3c4d5e6f"), new DateTime(2026, 6, 22, 9, 38, 56, 536, DateTimeKind.Utc).AddTicks(9367), "IBKR", "Interactive Brokers", null, null, "https://www.interactivebrokers.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brokers",
                keyColumn: "Id",
                keyValue: new Guid("c3a2b8d9-5f1a-4b6d-9f2e-1a2b3c4d5e6f"));
        }
    }
}
