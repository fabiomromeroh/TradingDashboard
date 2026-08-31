// components/TradeChart.tsx
import { useCandlestickChart, type Candle } from "./useCandlestickChart";

export interface TradeChartProps {
  candles: Candle[];
  height?: number;
}

export function TradeChart({ candles, height = 500 }: TradeChartProps) {
  const { containerRef, legendRef } = useCandlestickChart({
    data: candles,
    height,
  });

  if (candles.length === 0) {
    return (
      <div
        className="flex items-center justify-center text-sm text-muted-foreground"
        style={{ height }}
      >
        No price data available for this trade.
      </div>
    );
  }

  return (
    <div className="relative">
      <div
        ref={legendRef}
        className="pointer-events-none absolute top-2 left-2 z-10 rounded bg-popover/90 px-2 py-1 text-xs text-popover-foreground opacity-0 shadow-xs transition-opacity duration-150"
      />
      <div ref={containerRef} />
    </div>
  );
}
