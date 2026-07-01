using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TradeDerivedCalculations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ClosePrice",
                table: "Trades",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,8)",
                oldPrecision: 18,
                oldScale: 8,
                oldDefaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageClosePrice",
                table: "Trades",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageEntryPrice",
                table: "Trades",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetReturn",
                table: "Trades",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageReturn",
                table: "Trades",
                type: "decimal(10,4)",
                precision: 10,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCommissions",
                table: "Trades",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageClosePrice",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "AverageEntryPrice",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "NetReturn",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "PercentageReturn",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TotalCommissions",
                table: "Trades");

            migrationBuilder.AlterColumn<decimal>(
                name: "ClosePrice",
                table: "Trades",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,8)",
                oldPrecision: 18,
                oldScale: 8,
                oldNullable: true);
        }
    }
}
