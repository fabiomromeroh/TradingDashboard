import { DataTable } from "@/components/shared/DataTable";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import type { ImportHistory } from "../types/import.types";
import type { ColumnDef } from "@tanstack/react-table";
import { useImportHistoryQuery } from "../hooks/useImportHistoryQuery";
import { getDateFormat } from "@/lib/utils";
import { ConfirmDeleteButton } from "@/components/shared/ConfirmDeleteButton";
import { useRollbackImportMutation } from "../hooks/useRollbackImportMutation";

export function ImportHistoryTable({ accountId = "" }: { accountId?: string }) {
  const { importHistory, refetch: getImportHistory } =
    useImportHistoryQuery(accountId);
  const { mutate: rollbackImport } = useRollbackImportMutation();

  const handleRollbackImport = async (id: string) => {
    const success = await rollbackImport(id);

    if (success) {
      getImportHistory();
    }
  };

  const columns: ColumnDef<ImportHistory, unknown>[] = [
    { accessorKey: "sourceType", header: "Source Type" },

    {
      accessorKey: "brokerName",
      header: "Broker",
    },
    {
      accessorKey: "status",
      header: "Status",
      cell: ({ getValue }) => {
        const status = getValue() as string;
        const isCompleted = status === "Completed";

        return (
          <span
            className={`px-2.5 py-0.5 rounded-full text-xs font-semibold ${
              isCompleted
                ? "bg-primary/20 text-primary border border-primary/30"
                : "bg-destructive/20 text-destructive border border-destructive-foreground/30"
            }`}
          >
            {status}
          </span>
        );
      },
    },
    {
      accessorKey: "totalRows",
      header: "Total Rows",
    },
    {
      accessorKey: "processedRows",
      header: "Proccesed",
    },
    {
      accessorKey: "skippedRows",
      header: "Skipped",
    },
    {
      accessorKey: "periodStart",
      header: "Period Start",
      cell: ({ getValue }) => {
        const raw = getValue<string>();
        return getDateFormat(raw);
      },
    },
    {
      accessorKey: "periodEnd",
      header: "Period End",
      cell: ({ getValue }) => {
        const raw = getValue<string>();
        return getDateFormat(raw);
      },
    },
    {
      accessorKey: "completedAt",
      header: "Completed At",
      cell: ({ getValue }) => {
        const raw = getValue<string>();
        return getDateFormat(raw);
      },
    },

    {
      accessorKey: "fileName",
      header: "File Name",
    },
    {
      id: "actions",
      enableHiding: false,
      cell: ({ row }) => {
        const history = row.original;
        return (
          <ConfirmDeleteButton
            disabled={history.isRolledBack}
            label="Import History"
            handleOnClick={() => handleRollbackImport(history.id)}
          />
        );
      },
    },
  ];

  return (
    <Card>
      <CardContent>
        <CardHeader>
          <CardTitle>Import History</CardTitle>
        </CardHeader>
        <DataTable
          columns={columns}
          data={importHistory}
          filterPlaceholder="Filter trades..."
          withColumnVisibilityToggle
          withPagination
        />
      </CardContent>
    </Card>
  );
}
