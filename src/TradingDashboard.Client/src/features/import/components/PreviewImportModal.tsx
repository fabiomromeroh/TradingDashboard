import { DataTable } from "@/components/shared/DataTable";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import type {
  ConfirmImportCommand,
  UploadImport,
  UploadImportRow,
} from "../types/import.types";
import type { ColumnDef } from "@tanstack/react-table";
import { Label } from "@/components/ui/label";
import { useConfirmImport } from "../hooks/useConfirmImport";
import { toast } from "sonner";

const columns: ColumnDef<UploadImportRow>[] = [
  { accessorKey: "symbol", header: "Symbol" },
  { accessorKey: "side", header: "Side" },
  { accessorKey: "quantity", header: "Quantity" },
  { accessorKey: "price", header: "Price" },
  { accessorKey: "orderType", header: "Order Type" },
  {
    accessorKey: "executedAt",
    header: "Executed At",
    cell: ({ getValue }) => {
      const raw = getValue<string>();
      if (!raw) return "—";
      return new Date(raw).toLocaleString("en-IE", {
        year: "numeric",
        month: "short",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
        timeZone: "UTC",
      });
    },
  },
  { accessorKey: "commission", header: "Commission" },
];

export function PreviewImportModal(importPreview: UploadImport) {
  const { confirm, error } = useConfirmImport();

  const handleConfirm = async () => {
    const confirmImportCommand: ConfirmImportCommand = {
      fileName: importPreview.fileName,
      accountId: importPreview.accountId,
      totalRows: importPreview.totalRows,
      newRows: importPreview.newRows,
      duplicateRows: importPreview.duplicateRows,
      invalidRows: importPreview.invalidRows,
      rows: importPreview.rows,
    };
    const success = await confirm(confirmImportCommand);

    if (success) {
      toast.success("Trades imported to account successfully");
    } else {
      toast.error(error ?? "Failed to import trades to account");
    }
  };

  return (
    <div>
      <Dialog
        open={importPreview.showPreview}
        onOpenChange={importPreview.cancelUpload}
      >
        <DialogContent className="sm:max-w-6xl">
          <DialogHeader>
            <DialogTitle>Import Preview</DialogTitle>
          </DialogHeader>
          <div className="max-h-[80vh] overflow-auto">
            <div className="grid grid-cols-1 mb-3 gap-3">
              <Label>New: {importPreview.newRows}</Label>
              <Label>Duplicated: {importPreview.duplicateRows}</Label>
              <Label>Invalid: {importPreview.invalidRows}</Label>
            </div>

            <DataTable
              columns={columns}
              data={importPreview.rows}
              showFilter={true}
              withPagination
            />
          </div>
          <DialogFooter>
            <DialogClose asChild>
              <Button
                onClick={importPreview.cancelUpload}
                variant="outline"
                type="button"
              >
                Cancel
              </Button>
            </DialogClose>
            <Button onClick={handleConfirm} type="submit">
              Confirm
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
