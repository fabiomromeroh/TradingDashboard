// Public API of the trades feature.
// Only export what other features or pages actually need.
export { TradeTable } from "./components/TradeTable";
export { TradeStatusBadge } from "./components/TradeStatusBadge";
export { useTrades } from "./hooks/useTrades";
export type { Trade, TradeDirection, TradeStatus } from "./types/trade.types";
