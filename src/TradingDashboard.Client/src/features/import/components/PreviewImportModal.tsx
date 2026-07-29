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
  UploadImportRow,
  PreviewImportModalProps,
} from "../types/import.types";
import type { ColumnDef } from "@tanstack/react-table";
import { Label } from "@/components/ui/label";
import { toast } from "sonner";
import { getDateFormat } from "@/lib/utils";
import { useConfirmImportMutation } from "../hooks/useConfirmImportMutation";

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
      return getDateFormat(raw);
    },
  },
  { accessorKey: "commission", header: "Commission" },
];

export function PreviewImportModal(importPreview: PreviewImportModalProps) {
  const { mutate: confirmImport } = useConfirmImportMutation();

  const handleConfirm = async () => {
    const confirmImportCommand: ConfirmImportCommand = {
      fileName: importPreview.fileName,
      brokerName: importPreview.brokerName,
      accountId: importPreview.accountId,
      totalRows: importPreview.totalRows,
      newRows: importPreview.newRows,
      duplicateRows: importPreview.duplicateRows,
      invalidRows: importPreview.invalidRows,
      rows: importPreview.rows,
    };
    const success = await confirmImport(confirmImportCommand);

    if (success) {
      importPreview.setShowPreview(false);
      importPreview.onImportCompleted?.();
      toast.success("Trades imported to account successfully");
    }
  };

  return (
    <div>
      <Dialog
        open={importPreview.showPreview}
        onOpenChange={importPreview.cancelUpload}
      >
        <DialogContent
          onInteractOutside={(e) => e.preventDefault()}
          onEscapeKeyDown={(e) => e.preventDefault()}
          className="sm:max-w-6xl"
        >
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
              withFilter
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
