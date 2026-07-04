using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImportUploadNewApproach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Executions_Trades_TradeId",
                table: "Executions");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades");

            migrationBuilder.AlterColumn<Guid>(
                name: "TradeId",
                table: "Executions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Executions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Executions_AccountId",
                table: "Executions",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Executions_Accounts_AccountId",
                table: "Executions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Executions_Trades_TradeId",
                table: "Executions",
                column: "TradeId",
                principalTable: "Trades",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Executions_Accounts_AccountId",
                table: "Executions");

            migrationBuilder.DropForeignKey(
                name: "FK_Executions_Trades_TradeId",
                table: "Executions");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Executions_AccountId",
                table: "Executions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Executions");

            migrationBuilder.AlterColumn<Guid>(
                name: "TradeId",
                table: "Executions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Executions_Trades_TradeId",
                table: "Executions",
                column: "TradeId",
                principalTable: "Trades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
