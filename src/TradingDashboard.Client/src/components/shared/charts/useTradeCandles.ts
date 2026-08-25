// hooks/useTradeCandles.ts
import { useMemo } from "react";
import type { CandlestickData, Time } from "lightweight-charts";

interface UseTradeCandlesResult {
  candles: CandlestickData<Time>[];
  isLoading: boolean;
}

/**
 * TEMPORARY: generates fake daily OHLC data around a base price so the
 * chart can be built and styled before the real market-data API exists.
 * Replace the body with an HTTP call to `/trades/{tradeId}/candles` later —
 * the return shape (candles + isLoading) is what TradeChart/TradeDetail expect,
 * so no consumer code needs to change.
 */
export function useTradeCandles(
  tradeId: string,
  basePrice = 150,
  days = 30,
): UseTradeCandlesResult {
  const candles = useMemo(
    () => generateDummyCandles(basePrice, days),
    [tradeId, basePrice, days],
  );

  return { candles, isLoading: false };
}

function generateDummyCandles(
  basePrice: number,
  days: number,
): CandlestickData<Time>[] {
  const candles: CandlestickData<Time>[] = [];
  let lastClose = basePrice;

  const today = new Date();
  today.setUTCHours(0, 0, 0, 0);

  for (let i = days; i >= 0; i--) {
    const date = new Date(today);
    date.setUTCDate(date.getUTCDate() - i);

    const open = lastClose;
    const changePercent = (Math.random() - 0.5) * 0.04; // +/-2% daily swing
    const close = open * (1 + changePercent);
    const high = Math.max(open, close) * (1 + Math.random() * 0.01);
    const low = Math.min(open, close) * (1 - Math.random() * 0.01);

    candles.push({
      time: (date.getTime() / 1000) as Time,
      open: round2(open),
      high: round2(high),
      low: round2(low),
      close: round2(close),
    });

    lastClose = close;
  }

  return candles;
}

function round2(value: number): number {
  return Math.round(value * 100) / 100;
}
