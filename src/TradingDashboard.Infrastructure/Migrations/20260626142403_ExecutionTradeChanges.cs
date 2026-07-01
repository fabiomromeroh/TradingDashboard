using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExecutionTradeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Executions");

            migrationBuilder.AddColumn<decimal>(
                name: "PositionSize",
                table: "Trades",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionSize",
                table: "Trades");

            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "Executions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
