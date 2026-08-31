import type { ApiError } from "@/types/api.types";

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
  error: ApiError[] | null;
  refetch: () => void; // lets the UI trigger a manual refresh
  totalCount: number; // total number of trades available (for pagination)
}

// Position-lifecycle events: Entry/Add/Trim/PartialExit/FinalExit change the
// position itself; Note is a freeform annotation (e.g. earnings, thesis update).
export type TradeEventType =
  | "Entry"
  | "Add"
  | "Trim"
  | "PartialExit"
  | "FinalExit"
  | "Note";

// Shown by default on the trade detail page; "Note" events are opt-in.
export const CORE_TRADE_EVENT_TYPES: TradeEventType[] = [
  "Entry",
  "Add",
  "Trim",
  "PartialExit",
  "FinalExit",
];

export interface TradeEventDto {
  id: string;
  tradeId: string;
  type: TradeEventType;
  occurredAt: string; // ISO date string
  price?: number | null;
  note?: string | null;
}

export interface CreateTradeEventCommand {
  type: TradeEventType;
  occurredAt: string; // ISO date string
  price?: number;
  note?: string;
}

// Trade-level metadata: thesis, checkpoints, outcome, notes.
export interface TradeMetadata {
  thesis?: string | null;
  additionalNotes?: string | null;
  outcome?: string | null;
  checkpoints?: TradeCheckpoint[];
}

export interface TradeCheckpoint {
  id: string;
  description: string;
  status: "pending" | "intact" | "broken";
  createdAt?: string; // ISO date string
}
