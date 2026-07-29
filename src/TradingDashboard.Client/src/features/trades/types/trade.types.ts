// Union types act as enums but are plain strings — easier to work with
export type TradeDirection = "Long" | "Short";
export type TradeStatus = "Open" | "Closed" | "Cancelled";

export interface TradeDto {
  id: string;
  symbol: string;
  direction: TradeDirection;
  status: TradeStatus;
  entryPrice: number;
  closePrice?: number; // optional: only set when the trade is closed
  quantity: number;
  openedAt: string; // ISO date string from the API
  closedAt?: string;
  netReturn?: number | null;
  percentageReturn?: number;
}

export interface ExecutionDto {
  id: string;
  tradeId: string;
  price: number;
  quantity: number;
  executedAt: string; // ISO date string from the API
  side: string; // "buy" or "sell"
  commission?: number | null; // optional: only set
  instrumentType: string; // "stock" or "future"
}

export interface UseTradesResult {
  trades: TradeDto[];
  isLoading: boolean;
  error: string | null;
  refetch: () => void; // lets the UI trigger a manual refresh
}
