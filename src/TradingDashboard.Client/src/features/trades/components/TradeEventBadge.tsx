// components/TradeEventBadge.tsx
import type { TradeEventType } from "../types/trade.types";

// A lookup table instead of if/else chains — easy to extend with new event types.
const eventTypeConfig: Record<
  TradeEventType,
  { label: string; className: string; dotClassName: string }
> = {
  Entry: {
    label: "Entry",
    className: "bg-primary/20 text-primary border border-primary/30",
    dotClassName: "bg-primary",
  },
  Add: {
    label: "Add",
    className: "bg-chart-2/20 text-chart-2 border border-chart-2/30",
    dotClassName: "bg-chart-2",
  },
  Trim: {
    label: "Trim",
    className: "bg-chart-3/20 text-chart-3 border border-chart-3/30",
    dotClassName: "bg-chart-3",
  },
  PartialExit: {
    label: "Partial Exit",
    className: "bg-chart-4/20 text-chart-4 border border-chart-4/30",
    dotClassName: "bg-chart-4",
  },
  FinalExit: {
    label: "Final Exit",
    className:
      "bg-destructive/15 text-destructive border border-destructive/30",
    dotClassName: "bg-destructive",
  },
  Note: {
    label: "Note",
    className: "bg-secondary text-secondary-foreground border border-border",
    dotClassName: "bg-muted-foreground",
  },
};

export function TradeEventBadge({ type }: { type: TradeEventType }) {
  const config = eventTypeConfig[type];
  return (
    <span
      className={`inline-flex items-center rounded px-2 py-0.5 text-xs font-medium ${config.className}`}
    >
      {config.label}
    </span>
  );
}

export function getTradeEventDotClassName(type: TradeEventType): string {
  return eventTypeConfig[type].dotClassName;
}

export function getTradeEventLabel(type: TradeEventType): string {
  return eventTypeConfig[type].label;
}
