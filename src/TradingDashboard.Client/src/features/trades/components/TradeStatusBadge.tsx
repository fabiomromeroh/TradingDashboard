import type { TradeStatus } from "../types/trade.types";

interface TradeStatusBadgeProps {
  status: TradeStatus;
}

// A lookup table instead of if/else chains — easy to extend with new statuses.
const statusStyles: Record<TradeStatus, string> = {
  Open: "bg-primary/20 text-primary-foreground",
  Closed: "bg-accent/20 text-accent-foreground",
  Cancelled: "bg-muted/50 text-muted-foreground",
};

export function TradeStatusBadge({ status }: TradeStatusBadgeProps) {
  return (
    <span
      className={`inline-flex items-center rounded px-2 py-0.5 text-xs font-medium ${statusStyles[status]}`}
    >
      {status}
    </span>
  );
}
