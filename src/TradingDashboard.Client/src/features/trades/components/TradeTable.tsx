import type { ColumnDef } from "@tanstack/react-table";
import { MoreHorizontal } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { useTradesQuery } from "../hooks/useTradesQuery";
import { TradeStatusBadge } from "./TradeStatusBadge";
import type { ExecutionDto, TradeDto } from "../types/trade.types";
import { DataTable } from "@/components/shared/DataTable";
import { getTradeExecutions } from "../api/trades.api";
import { getDateFormat } from "@/lib/utils";

const columns: ColumnDef<TradeDto, unknown>[] = [
  { accessorKey: "symbol", header: "Symbol" },
  {
    accessorKey: "direction",
    header: "Direction",
    cell: ({ getValue }) => {
      const val = getValue() as string;
      return (
        <span
          className={val === "Long" ? "text-primary " : " text-destructive "}
        >
          {val}
        </span>
      );
    },
  },
  { accessorKey: "quantity", header: "Qty" },
  { accessorKey: "positionSize", header: "Position" },
  {
    accessorKey: "entryPrice",
    header: "Entry Price",
    cell: ({ getValue }) => `$${(getValue() as number).toFixed(2)}`,
  },
  {
    accessorKey: "openedAt",
    header: "Open Date",
    cell: ({ getValue }) => getDateFormat(getValue() as string),
  },
  {
    accessorKey: "closePrice",
    header: "Close Price",
    cell: ({ getValue }) => {
      const val = (getValue() as number | null) || undefined;
      return val !== undefined && val !== null ? `$${val.toFixed(2)}` : "—";
    },
  },
  {
    accessorKey: "closedAt",
    header: "Close Date",
    cell: ({ getValue }) => getDateFormat(getValue() as string),
  },

  { accessorKey: "totalCommissions", header: "Commissions" },
  { accessorKey: "averageEntryPrice", header: "Avg Entry Price" },
  { accessorKey: "averageClosePrice", header: "Avg Close Price" },
  {
    accessorKey: "netReturn",
    header: "Net Return",
    cell: ({ getValue }) => {
      const val = getValue() as number | undefined | null;
      if (val === undefined || val === null) return "—";
      return (
        <span className={val >= 0 ? "text-primary" : "text-destructive"}>
          {val >= 0 ? "+" : ""}
          {val.toFixed(2)}
        </span>
      );
    },
  },
  {
    accessorKey: "percentageReturn",
    header: "% Return",
    cell: ({ getValue }) => {
      const val = getValue() as number | undefined | null;
      if (val === undefined || val === null) return "—";
      return (
        <span className={val >= 0 ? "text-primary" : "text-destructive"}>
          {val >= 0 ? "+" : ""}
          {val.toFixed(2)}%
        </span>
      );
    },
  },
  {
    accessorKey: "status",
    accessorFn: (row) =>
      row.netReturn == null ? "Open" : row.netReturn > 0 ? "Win" : "Loss",
    header: "Status",
    cell: ({ getValue }) => <TradeStatusBadge status={getValue() as string} />,
  },
  {
    id: "actions",
    enableHiding: false,
    cell: ({ row }) => {
      const trade = row.original;
      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" size="sm" className="h-8 w-8 p-0">
              <span className="sr-only">Open menu</span>
              <MoreHorizontal className="h-4 w-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Actions</DropdownMenuLabel>
            <DropdownMenuItem
              onClick={() => navigator.clipboard.writeText(trade.id)}
            >
              Copy ID
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem>View details</DropdownMenuItem>
            <DropdownMenuItem className="text-destructive">
              Delete trade
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      );
    },
  },
];

const executionColumns: ColumnDef<ExecutionDto, unknown>[] = [
  { accessorKey: "side", header: "Side" },
  { accessorKey: "instrumentType", header: "Type" },
  {
    accessorKey: "executedAt",
    header: "Executed At",
    cell: ({ getValue }) => {
      const raw = getValue<string>();
      return getDateFormat(raw);
    },
  },
  { accessorKey: "quantity", header: "Quantity" },
  { accessorKey: "price", header: "Price" },
];

export function TradeTable() {
  const { trades, isLoading, error, refetch } = useTradesQuery();

  if (isLoading) {
    return (
      <p className="py-8 text-center text-muted-foreground">Loading trades…</p>
    );
  }

  if (error) {
    return (
      <div className="py-8 text-center">
        <p className="text-red-600 mb-2">{error}</p>
        <button onClick={refetch} className="text-sm underline">
          Try again
        </button>
      </div>
    );
  }

  return (
    <DataTable
      columns={columns}
      data={trades}
      filterPlaceholder="Filter trades..."
      withColumnVisibilityToggle
      withPagination
      withViewDetails
      withFilter
      detailColumns={executionColumns}
      detailsFetcher={(trade) => getTradeExecutions(trade.id)}
      detailTitle="Executions"
    />
  );
}
