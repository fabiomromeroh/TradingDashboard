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
      <div className="trade-chart-empty">
        No price data available for this trade.
      </div>
    );
  }

  return (
    <div className="trade-chart-wrapper" style={{ position: "relative" }}>
      <div
        ref={legendRef}
        className="trade-chart-legend"
        style={{
          position: "absolute",
          top: 8,
          left: 8,
          zIndex: 1,
          opacity: 0,
          transition: "opacity 0.15s",
        }}
      />
      <div ref={containerRef} className="trade-chart-container" />
    </div>
  );
}
