export interface UploadImportRow {
  RowNumber: number;
  Symbol: string;
  Description: string;
  Side: string;
  Quantity: number;
  Price: number;
  Commission: number;
  Exchange: string;
  OrderType: string;
  ExecutedAt: Date;
  IsDuplicate: boolean;
  ParseError: string | null;
}

export interface UploadImport {
  brokerName: string;
  duplicateRows: number;
  fileName: string;
  invalidRows: number;
  totalRows: number;
  newRows: number;
  rows: UploadImportRow[];
  cancelUpload: () => void;
  showPreview: boolean;
  accountId: string;
}

export interface ConfirmImportCommand {
  accountId: string;
  fileName: string;
  totalRows: number;
  newRows: number;
  invalidRows: number;
  duplicateRows: number;
  rows: UploadImportRow[];
}

export interface ImportHistoryTableProps {
  accountId: string;
  fileName?: string;
  fileFormat?: string;
  sourceType: number;
  completedAt?: Date;
  status: number;
  periodStart: Date;
  periodEnd: Date;
  totalRows: number;
  newRows: number;
  skippedRows: number;
  brokerName: string;
}
