// Union types act as enums but are plain strings — easier to work with
export type TradeDirection = 'Long' | 'Short';
export type TradeStatus = 'Open' | 'Closed' | 'Cancelled';

export interface Trade {
  id: string;
  symbol: string;
  direction: TradeDirection;
  status: TradeStatus;
  entryPrice: number;
  exitPrice?: number;   // optional: only set when the trade is closed
  quantity: number;
  openedAt: string;     // ISO date string from the API
  closedAt?: string;
  pnl?: number;         // optional: only available on closed trades
}

export interface UseTradesResult {
  trades: Trade[];
  isLoading: boolean;
  error: string | null;
  refetch: () => void; // lets the UI trigger a manual refresh
}