import { DataTable } from "@/components/shared/DataTable";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import type { ImportHistoryTableProps } from "../types/import.types";
import type { ColumnDef } from "@tanstack/react-table";

const columns: ColumnDef<ImportHistoryTableProps, unknown>[] = [
  {
    accessorKey: "brokerName",
    header: "Broker",
  },
  {
    accessorKey: "totalRows",
    header: "Total Rows",
  },
  {
    accessorKey: "newRows",
    header: "New Rows",
  },
  {
    accessorKey: "skippedRows",
    header: "Skipped Rows",
  },
  {
    accessorKey: "periodStart",
    header: "Period Start",
  },
  {
    accessorKey: "periodEnd",
    header: "Period End",
  },
  {
    accessorKey: "completedAt",
    header: "Completed At",
  },
  {
    accessorKey: "status",
    header: "Status",
  },
  { accessorKey: "sourceType", header: "Source Type" },
  { accessorKey: "fileFormat", header: "File Format" },
];

export function ImportHistoryTable() {
  return (
    <Card>
      <CardContent>
        <CardHeader>
          <CardTitle>Import History</CardTitle>
        </CardHeader>
        <DataTable
          columns={columns}
          data={[]}
          filterPlaceholder="Filter trades..."
          withColumnVisibilityToggle
          withPagination
        />
      </CardContent>
    </Card>
  );
}
