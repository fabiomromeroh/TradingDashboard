interface TradeStatusBadgeProps {
  status: string;
}

// A lookup table instead of if/else chains — easy to extend with new statuses.
const statusStyles: Record<string, string> = {
  Open: "bg-warning/30 text-warning border border-warning/30",
  Win: "bg-primary/20 text-primary border border-primary/30",
  Loss: "bg-destructive/20 text-destructive border border-destructive-foreground/30",
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
