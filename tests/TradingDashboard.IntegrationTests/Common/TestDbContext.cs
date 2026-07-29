using Microsoft.EntityFrameworkCore;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;
using TradingDashboard.Infrastructure.Persistence;

namespace TradingDashboard.IntegrationTests.Common;

public static class TestDbContext
{
    public static void ConfigureForTests(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("TradingDashboardTests");
    }

    public static void SeedTestData(AppDbContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        //----------------------------------------------------------------------
        // USERS
        //----------------------------------------------------------------------
        var user1 = User.Create(
            email: "alice@test.com",
            passwordHash: "hash-alice",
            firstName: "Alice",
            lastName: "Trader");

        var user2 = User.Create(
            email: "bob@test.com",
            passwordHash: "hash-bob",
            firstName: "Bob",
            lastName: "Investor");

        var user3 = User.Create(
            email: "charlie@test.com",
            passwordHash: "hash-charlie",
            firstName: "Charlie",
            lastName: "Scalper");

        db.Users.AddRange(user1, user2, user3);

        //----------------------------------------------------------------------
        // BROKERS
        //----------------------------------------------------------------------
        var broker1 = Broker.Create(
            name: "IBKR",
            displayName: "Interactive Brokers",
            website: "https://ibkr.test",
            supportedImportFormat: "CSV");

        var broker2 = Broker.Create(
            name: "TD",
            displayName: "TD Ameritrade",
            website: "https://td.test",
            supportedImportFormat: "CSV");

        db.Brokers.AddRange(broker1, broker2);
        db.SaveChanges();

        //----------------------------------------------------------------------
        // ACCOUNTS
        //----------------------------------------------------------------------
        var accountAliceIbkr = Account.Create(
            name: "Alice-IBKR-USD",
            userId: user1.Id,
            brokerId: broker1.Id,
            importSourceType: ImportSourceType.FileUpload);

        var accountAliceTd = Account.Create(
            name: "Alice-TD-USD",
            userId: user1.Id,
            brokerId: broker2.Id,
            importSourceType: ImportSourceType.FileUpload);

        var accountBobIbkr = Account.Create(
            name: "Bob-IBKR-EUR",
            userId: user2.Id,
            brokerId: broker1.Id,
            importSourceType: ImportSourceType.FileUpload);

        var accountCharlieIbkr = Account.Create(
            name: "Charlie-IBKR-USD",
            userId: user3.Id,
            brokerId: broker1.Id,
            importSourceType: ImportSourceType.FileUpload);

        db.Accounts.AddRange(accountAliceIbkr, accountAliceTd, accountBobIbkr, accountCharlieIbkr);
        db.SaveChanges();

        //----------------------------------------------------------------------
        // IMPORT SESSIONS
        //----------------------------------------------------------------------
        var sessionAliceIbkr1 = ImportSession.Create(
            accountId: accountAliceIbkr.Id,
            brokerName: broker1.Name,
            ImportSourceType.FileUpload,
            fileName: "alice-ibkr-2024-01-01.csv");

        var sessionAliceIbkr2 = ImportSession.Create(
            accountId: accountAliceIbkr.Id,
            brokerName: broker1.Name,
            ImportSourceType.FileUpload,
            fileName: "alice-ibkr-2024-01-02.csv");

        var sessionAliceTd1 = ImportSession.Create(
            accountId: accountAliceTd.Id,
            brokerName: broker2.Name,
            ImportSourceType.FileUpload,
            fileName: "alice-td-2024-01-03.csv");

        var sessionBobIbkr1 = ImportSession.Create(
            accountId: accountBobIbkr.Id,
            brokerName: broker1.Name,
            ImportSourceType.FileUpload,
            fileName: "bob-ibkr-2024-01-01.csv");

        var sessionCharlieIbkr1 = ImportSession.Create(
            accountId: accountCharlieIbkr.Id,
            brokerName: broker1.Name,
            ImportSourceType.FileUpload,
            fileName: "charlie-ibkr-2024-01-01.csv");

        db.ImportSessions.AddRange(sessionAliceIbkr1, sessionAliceIbkr2, sessionAliceTd1, sessionBobIbkr1, sessionCharlieIbkr1);
        db.SaveChanges();

        //----------------------------------------------------------------------
        // TRADES + EXECUTIONS (5 trades, each with multiple executions on same symbol)
        //----------------------------------------------------------------------

        var now = DateTimeOffset.UtcNow;

        // Trade 1: Alice, IBKR, AAPL long, 3 executions
        var trade1 = Trade.Create(
            symbol: "AAPL",
            entryPrice: 190.00m,
            quantity: 30m,
            direction: TradeDirection.Long,
            accountId: accountAliceIbkr.Id,
            openedAt: now.AddMinutes(-120));

        var exec1_1 = Execution.Create(
            accountId: accountAliceIbkr.Id,
            symbol: "AAPL",
            price: 189.50m,
            quantity: 10m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-119),
            commission: 1.25m,
            brokerExecutionId: "ALICE-IBKR-AAPL-1",
            brokerOrderId: "ALICE-IBKR-ORD-1",
            importSessionId: sessionAliceIbkr1.Id,
            exchange: "NASDAQ",
            orderType: "MKT",
            currency: CurrencyType.USD);

        var exec1_2 = Execution.Create(
            accountId: accountAliceIbkr.Id,
            symbol: "AAPL",
            price: 190.20m,
            quantity: 10m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-118),
            commission: 1.25m,
            brokerExecutionId: "ALICE-IBKR-AAPL-2",
            brokerOrderId: "ALICE-IBKR-ORD-2",
            importSessionId: sessionAliceIbkr1.Id,
            exchange: "NASDAQ",
            orderType: "LMT",
            currency: CurrencyType.USD);

        var exec1_3 = Execution.Create(
            accountId: accountAliceIbkr.Id,
            symbol: "AAPL",
            price: 190.80m,
            quantity: 10m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-117),
            commission: 1.25m,
            brokerExecutionId: "ALICE-IBKR-AAPL-3",
            brokerOrderId: "ALICE-IBKR-ORD-3",
            importSessionId: sessionAliceIbkr2.Id,
            exchange: "NASDAQ",
            orderType: "MKT",
            currency: CurrencyType.USD);

        // Trade 2: Alice, TD, MSFT long, 2 executions
        var trade2 = Trade.Create(
            symbol: "MSFT",
            entryPrice: 300.00m,
            quantity: 20m,
            direction: TradeDirection.Long,
            accountId: accountAliceTd.Id,
            openedAt: now.AddMinutes(-90));

        var exec2_1 = Execution.Create(
            accountId: accountAliceTd.Id,
            symbol: "MSFT",
            price: 299.50m,
            quantity: 10m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-89),
            commission: 1.00m,
            brokerExecutionId: "ALICE-TD-MSFT-1",
            brokerOrderId: "ALICE-TD-ORD-1",
            importSessionId: sessionAliceTd1.Id,
            exchange: "NASDAQ",
            orderType: "MKT",
            currency: CurrencyType.USD);

        var exec2_2 = Execution.Create(
            accountId: accountAliceTd.Id,
            symbol: "MSFT",
            price: 300.50m,
            quantity: 10m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-88),
            commission: 1.00m,
            brokerExecutionId: "ALICE-TD-MSFT-2",
            brokerOrderId: "ALICE-TD-ORD-2",
            importSessionId: sessionAliceTd1.Id,
            exchange: "NASDAQ",
            orderType: "LMT",
            currency: CurrencyType.USD);

        // Trade 3: Bob, IBKR, TSLA short, 3 executions
        var trade3 = Trade.Create(
            symbol: "TSLA",
            entryPrice: 250.00m,
            quantity: 15m,
            direction: TradeDirection.Short,
            accountId: accountBobIbkr.Id,
            openedAt: now.AddMinutes(-60));

        var exec3_1 = Execution.Create(
            accountId: accountBobIbkr.Id,
            symbol: "TSLA",
            price: 249.50m,
            quantity: 5m,
            side: Side.Sell,
            executedAt: now.AddMinutes(-59),
            commission: 0.75m,
            brokerExecutionId: "BOB-IBKR-TSLA-1",
            brokerOrderId: "BOB-IBKR-ORD-1",
            importSessionId: sessionBobIbkr1.Id,
            exchange: "NASDAQ",
            orderType: "MKT",
            currency: CurrencyType.USD);

        var exec3_2 = Execution.Create(
            accountId: accountBobIbkr.Id,
            symbol: "TSLA",
            price: 250.50m,
            quantity: 5m,
            side: Side.Sell,
            executedAt: now.AddMinutes(-58),
            commission: 0.75m,
            brokerExecutionId: "BOB-IBKR-TSLA-2",
            brokerOrderId: "BOB-IBKR-ORD-2",
            importSessionId: sessionBobIbkr1.Id,
            exchange: "NASDAQ",
            orderType: "LMT",
            currency: CurrencyType.USD);

        var exec3_3 = Execution.Create(
            accountId: accountBobIbkr.Id,
            symbol: "TSLA",
            price: 251.00m,
            quantity: 5m,
            side: Side.Sell,
            executedAt: now.AddMinutes(-57),
            commission: 0.75m,
            brokerExecutionId: "BOB-IBKR-TSLA-3",
            brokerOrderId: "BOB-IBKR-ORD-3",
            importSessionId: sessionBobIbkr1.Id,
            exchange: "NASDAQ",
            orderType: "MKT",
            currency: CurrencyType.USD);

        // Trade 4: Charlie, IBKR, NVDA long, 2 executions
        var trade4 = Trade.Create(
            symbol: "NVDA",
            entryPrice: 600.00m,
            quantity: 12m,
            direction: TradeDirection.Long,
            accountId: accountCharlieIbkr.Id,
            openedAt: now.AddMinutes(-45));

        var exec4_1 = Execution.Create(
            accountId: accountCharlieIbkr.Id,
            symbol: "NVDA",
            price: 599.50m,
            quantity: 6m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-44),
            commission: 1.50m,
            brokerExecutionId: "CHARLIE-IBKR-NVDA-1",
            brokerOrderId: "CHARLIE-IBKR-ORD-1",
            importSessionId: sessionCharlieIbkr1.Id,
            exchange: "NASDAQ",
            orderType: "MKT",
            currency: CurrencyType.USD);

        var exec4_2 = Execution.Create(
            accountId: accountCharlieIbkr.Id,
            symbol: "NVDA",
            price: 600.50m,
            quantity: 6m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-43),
            commission: 1.50m,
            brokerExecutionId: "CHARLIE-IBKR-NVDA-2",
            brokerOrderId: "CHARLIE-IBKR-ORD-2",
            importSessionId: sessionCharlieIbkr1.Id,
            exchange: "NASDAQ",
            orderType: "LMT",
            currency: CurrencyType.USD);

        // Trade 5: Alice, IBKR, META long, 4 executions
        var trade5 = Trade.Create(
            symbol: "META",
            entryPrice: 350.00m,
            quantity: 40m,
            direction: TradeDirection.Long,
            accountId: accountAliceIbkr.Id,
            openedAt: now.AddMinutes(-30));

        var exec5_1 = Execution.Create(
            accountId: accountAliceIbkr.Id,
            symbol: "META",
            price: 349.00m,
            quantity: 10m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-29),
            commission: 1.10m,
            brokerExecutionId: "ALICE-IBKR-META-1",
            brokerOrderId: "ALICE-IBKR-META-ORD-1",
            importSessionId: sessionAliceIbkr2.Id,
            exchange: "NASDAQ",
            orderType: "MKT",
            currency: CurrencyType.USD);

        var exec5_2 = Execution.Create(
            accountId: accountAliceIbkr.Id,
            symbol: "META",
            price: 350.25m,
            quantity: 10m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-28),
            commission: 1.10m,
            brokerExecutionId: "ALICE-IBKR-META-2",
            brokerOrderId: "ALICE-IBKR-META-ORD-2",
            importSessionId: sessionAliceIbkr2.Id,
            exchange: "NASDAQ",
            orderType: "LMT",
            currency: CurrencyType.USD);

        var exec5_3 = Execution.Create(
            accountId: accountAliceIbkr.Id,
            symbol: "META",
            price: 351.00m,
            quantity: 10m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-27),
            commission: 1.10m,
            brokerExecutionId: "ALICE-IBKR-META-3",
            brokerOrderId: "ALICE-IBKR-META-ORD-3",
            importSessionId: sessionAliceIbkr2.Id,
            exchange: "NASDAQ",
            orderType: "MKT",
            currency: CurrencyType.USD);

        var exec5_4 = Execution.Create(
            accountId: accountAliceIbkr.Id,
            symbol: "META",
            price: 350.75m,
            quantity: 10m,
            side: Side.Buy,
            executedAt: now.AddMinutes(-26),
            commission: 1.10m,
            brokerExecutionId: "ALICE-IBKR-META-4",
            brokerOrderId: "ALICE-IBKR-META-ORD-4",
            importSessionId: sessionAliceIbkr2.Id,
            exchange: "NASDAQ",
            orderType: "LMT",
            currency: CurrencyType.USD);

        db.Trades.AddRange(trade1, trade2, trade3, trade4, trade5);
        db.Executions.AddRange(
            exec1_1, exec1_2, exec1_3,
            exec2_1, exec2_2,
            exec3_1, exec3_2, exec3_3,
            exec4_1, exec4_2,
            exec5_1, exec5_2, exec5_3, exec5_4);

        db.SaveChanges();
    }
}
