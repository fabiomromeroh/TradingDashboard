using TradingDashboard.Domain.Common;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Domain.Entities;

public class Trade : BaseEntity
{
    public string Symbol { get; private set; } = string.Empty;
    public decimal EntryPrice { get; private set; }
    public decimal? ClosePrice { get; private set; }
    public decimal Quantity { get; private set; }
    public TradeDirection Direction { get; private set; }
    public DateTimeOffset OpenedAt { get; private set; }

    //Derived calculations---------------------------------
    public TradeStatus Status { get; private set; } = TradeStatus.Open;
    public DateTimeOffset? ClosedAt { get; private set; }

    public decimal PositionSize { get; private set; }
    public decimal TotalCommissions { get; private set; }
    public decimal AverageEntryPrice { get; private set; }
    public decimal? AverageClosePrice { get; private set; }
    public decimal? NetReturn { get; private set; }
    public decimal? PercentageReturn { get; set; }

    //--------------------------------------------------------

    public Guid AccountId { get; private set; }
    public Account Account { get; set; } = null!;

    public IReadOnlyCollection<Execution> Executions => _executions.AsReadOnly();
    private readonly List<Execution> _executions = [];

    // EF Core constructor
    private Trade() { }


    public static Trade Create(string symbol, decimal entryPrice, decimal quantity, TradeDirection direction, Guid accountId, DateTimeOffset openedAt)
    {
        return new()
        {
            Symbol = symbol,
            EntryPrice = entryPrice,
            Quantity = quantity,
            Direction = direction,
            AccountId = accountId,
            Status = TradeStatus.Open,
            OpenedAt = openedAt
        };
    }

    public static Trade CreatePlaceholder(string symbol, Guid accountId)
    {
        return new()
        {
            Symbol = symbol,
            AccountId = accountId,
            Status = TradeStatus.Open,

        };
    }

    public void RebuildFromExecutions(IEnumerable<Execution> executions)
    {
        _executions.Clear();
        _executions.AddRange(executions.OrderBy(e => e.ExecutedAt).ThenBy(e => e.Id));
        RecalculatePosition();
    }

    public void AddExecution(Execution execution)
    {
        _executions.Add(execution);
        RecalculatePosition();
    }

    private void RecalculatePosition()
    {
        if (_executions.Count == 0)
            return;

        //First execution
        if (_executions.Count == 1)
        {
            EntryPrice = _executions[0].Price;
            Quantity = _executions[0].Quantity;
            Direction = _executions[0].Side == Side.Buy ? TradeDirection.Long : TradeDirection.Short;
            OpenedAt = _executions[0].ExecutedAt;
        }


        var buys = _executions.Where(e => e.Side == Side.Buy).ToList();
        var sells = _executions.Where(e => e.Side == Side.Sell).ToList();

        var totalBuyQty = buys.Sum(e => e.Quantity);
        var totalSellQty = sells.Sum(e => e.Quantity);

        PositionSize = totalBuyQty - totalSellQty; // negative = short, 0 = closed, positive = long

        TotalCommissions = _executions.Sum(e => e.Commission);

        //// Detect direction from first execution
        //var first = _executions.MinBy(e => e.ExecutedAt);
        //Direction = first?.Side == Side.Sell ? TradeDirection.Short : TradeDirection.Long;

        // VWAP entry = opening side, VWAP exit = closing side
        var openingSide = Direction == TradeDirection.Long ? buys : sells;
        var closingSide = Direction == TradeDirection.Long ? sells : buys;
        var openingQty = Direction == TradeDirection.Long ? totalBuyQty : totalSellQty;
        var closingQty = Direction == TradeDirection.Long ? totalSellQty : totalBuyQty;

        AverageEntryPrice = openingQty > 0
            ? openingSide.Sum(e => e.Price * e.Quantity) / openingQty
            : 0;

        if (PositionSize == 0)
        {
            var last = _executions.MaxBy(e => e.ExecutedAt);
            Status = TradeStatus.Closed;
            ClosePrice = last!.Price;
            ClosedAt = last.ExecutedAt;

            AverageClosePrice = closingQty > 0
                ? closingSide.Sum(e => e.Price * e.Quantity) / closingQty
                : 0;

            var filledQty = Math.Min(openingQty, closingQty);

            // Long: profit when exit > entry. Short: profit when exit < entry.
            var priceDelta = Direction == TradeDirection.Long
                ? AverageClosePrice - AverageEntryPrice   //  positive = profit
                : AverageEntryPrice - AverageClosePrice;  //  positive = profit (sold high, bought low)

            var grossPnL = priceDelta * filledQty;
            NetReturn = grossPnL.GetValueOrDefault() - TotalCommissions;

            var capitalDeployed = AverageEntryPrice * filledQty;
            PercentageReturn = capitalDeployed != 0
                ? (NetReturn / capitalDeployed) * 100m
                : 0;
        }

    }

    public bool IsClosed => Status == TradeStatus.Closed;
    // Safe zero-fallback versions for display/aggregation contexts
    public decimal ClosePriceOrZero => ClosePrice ?? 0m;
    public decimal NetReturnOrZero => NetReturn ?? 0m;
    public decimal PercentageReturnOrZero => PercentageReturn ?? 0m;
    public decimal AverageClosePriceOrZero => AverageClosePrice ?? 0m;
}
