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
import { useTrades } from "../hooks/useTrades";
import { TradeStatusBadge } from "./TradeStatusBadge";
import type { Trade } from "../types/trade.types";
import { DataTable } from "@/components/shared/DataTable";

const columns: ColumnDef<Trade, unknown>[] = [
  { accessorKey: "symbol", header: "Symbol" },
  {
    accessorKey: "direction",
    header: "Direction",
    cell: ({ getValue }) => {
      const val = getValue() as string;
      return (
        <span className={val === "Long" ? "text-green-600" : "text-red-600"}>
          {val}
        </span>
      );
    },
  },
  {
    accessorKey: "entryPrice",
    header: "Entry Price",
    cell: ({ getValue }) => `$${(getValue() as number).toFixed(2)}`,
  },
  {
    accessorKey: "closePrice",
    header: "Close Price",
    cell: ({ getValue }) => {
      const val = getValue() as number | undefined;
      return val !== undefined ? `$${val.toFixed(2)}` : "—";
    },
  },
  { accessorKey: "quantity", header: "Qty" },
  { accessorKey: "positionSize", header: "Position" },
  {
    accessorKey: "netReturn",
    header: "Net Return",
    cell: ({ getValue }) => {
      const val = getValue() as number | undefined | null;
      if (val === undefined || val === null) return "—";
      return (
        <span
          className={
            val >= 0 ? "text-green-600 font-medium" : "text-red-600 font-medium"
          }
        >
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
        <span
          className={
            val >= 0 ? "text-green-600 font-medium" : "text-red-600 font-medium"
          }
        >
          {val >= 0 ? "+" : ""}
          {val.toFixed(2)}%
        </span>
      );
    },
  },
  {
    accessorKey: "status",
    header: "Status",
    cell: ({ getValue }) => (
      <TradeStatusBadge status={getValue() as Trade["status"]} />
    ),
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

export function TradeTable() {
  const { trades, isLoading, error, refetch } = useTrades();

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
    />
  );
}
