import FileUploadZone from "./FileUploadZone";
import ImportHistoryTable from "./ImportHistoryTable";

export function ImportTrades() {
  return (
    <div className="space-y-4">
      <FileUploadZone />
      <ImportHistoryTable />
    </div>
  );
}