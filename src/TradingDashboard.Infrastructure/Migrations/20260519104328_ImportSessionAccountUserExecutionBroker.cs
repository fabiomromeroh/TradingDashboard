using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImportSessionAccountUserExecutionBroker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OpenedAt",
                table: "Trades",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Trades",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ClosePrice",
                table: "Trades",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ClosedAt",
                table: "Trades",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateOnly>(
                name: "ClosedAtDate",
                table: "Trades",
                type: "date",
                nullable: false,
                computedColumnSql: "CAST([ClosedAt] AS DATE)",
                stored: true);

            migrationBuilder.AddColumn<int>(
                name: "ClosedAtHour",
                table: "Trades",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEPART(HOUR, [ClosedAt])",
                stored: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "OpenedAtDate",
                table: "Trades",
                type: "date",
                nullable: false,
                computedColumnSql: "CAST([OpenedAt] AS DATE)",
                stored: true);

            migrationBuilder.AddColumn<int>(
                name: "OpenedAtHour",
                table: "Trades",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEPART(HOUR, [OpenedAt])",
                stored: true);

            migrationBuilder.CreateTable(
                name: "Brokers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SupportedImportFormat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    InitialBalance = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrokerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Brokers_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "Brokers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrokerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TotalRows = table.Column<int>(type: "int", nullable: false),
                    ProcessedRows = table.Column<int>(type: "int", nullable: false),
                    FailedRows = table.Column<int>(type: "int", nullable: false),
                    ErrorSummary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PeriodStart = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PeriodEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportSessions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Executions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    ExecutedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImportSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Executions_ImportSessions_ImportSessionId",
                        column: x => x.ImportSessionId,
                        principalTable: "ImportSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Executions_Trades_TradeId",
                        column: x => x.TradeId,
                        principalTable: "Trades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trades_AccountId",
                table: "Trades",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_ClosedAtDate",
                table: "Trades",
                column: "ClosedAtDate");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_ClosedAtHour",
                table: "Trades",
                column: "ClosedAtHour");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_OpenedAtDate",
                table: "Trades",
                column: "OpenedAtDate");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_OpenedAtHour",
                table: "Trades",
                column: "OpenedAtHour");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BrokerId",
                table: "Accounts",
                column: "BrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Brokers_Name",
                table: "Brokers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Executions_ExecutedAt",
                table: "Executions",
                column: "ExecutedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Executions_ImportSessionId",
                table: "Executions",
                column: "ImportSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Executions_TradeId",
                table: "Executions",
                column: "TradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportSessions_AccountId",
                table: "ImportSessions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            // Delete orphaned trades that don't have a valid AccountId
            migrationBuilder.Sql("DELETE FROM [Trades] WHERE [AccountId] IS NULL OR [AccountId] = '00000000-0000-0000-0000-000000000000';");

            // Make AccountId non-nullable
            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Trades",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Accounts_AccountId",
                table: "Trades");

            migrationBuilder.DropTable(
                name: "Executions");

            migrationBuilder.DropTable(
                name: "ImportSessions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Brokers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Trades_AccountId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_ClosedAtDate",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_ClosedAtHour",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_OpenedAtDate",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_OpenedAtHour",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ClosedAtDate",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ClosedAtHour",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "OpenedAtDate",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "OpenedAtHour",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ClosePrice",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "Trades");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenedAt",
                table: "Trades",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }
    }
}
