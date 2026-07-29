using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BrokerAccountCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrokerAccountCredentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncryptedPayload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSyncDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerAccountCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrokerAccountCredentials_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerAccountCredentials_AccountId",
                table: "BrokerAccountCredentials",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerAccountCredentials");
        }
    }
}
