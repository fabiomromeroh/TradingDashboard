// components/TradeDetail.tsx
import { Spinner } from "@/components/ui/spinner";
import { useTradeCandles } from "@/components/shared/charts/useTradeCandles";
import type { TradeDto } from "../types/trade.types";
import { TradeChart } from "@/components/shared/charts/TradeChart";

export function TradeDetail({ trade }: { trade: TradeDto }) {
  const { candles, isLoading } = useTradeCandles(trade.id, trade.entryPrice);

  return (
    <section className="trade-detail">
      <h2>{trade.symbol}</h2>
      {isLoading ? <Spinner /> : <TradeChart candles={candles} height={450} />}
    </section>
  );
}
