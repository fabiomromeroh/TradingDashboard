import type { TradeStatus } from '../types/trade.types';

interface TradeStatusBadgeProps {
  status: TradeStatus;
}

// A lookup table instead of if/else chains — easy to extend with new statuses.
const statusStyles: Record<TradeStatus, string> = {
  Open: 'bg-blue-100 text-blue-700',
  Closed: 'bg-green-100 text-green-700',
  Cancelled: 'bg-gray-100 text-gray-500',
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

