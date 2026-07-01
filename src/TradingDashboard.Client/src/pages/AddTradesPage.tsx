import { useLocation } from "react-router-dom";
import { Separator } from "@/components/ui/separator";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ImportHistoryTable } from "@/features/import/components/ImportHistoryTable";
import { ImportUpload } from "@/features/import";

type ImportPageState = {
  accountId?: string;
};

export function AddTradesPage() {
  const location = useLocation();
  const importState = (location.state as ImportPageState | null) ?? {};

  return (
    <Tabs defaultValue="importFile">
      <TabsList>
        <TabsTrigger value="importFile">File Upload</TabsTrigger>
        <TabsTrigger value="brokerSync">Broker Sync</TabsTrigger>
        <TabsTrigger value="manualAdd">Manual</TabsTrigger>
      </TabsList>
      <TabsContent value="importFile">
        <ImportUpload accountId={importState.accountId} />
      </TabsContent>

      <Separator className="my-4" />
      <ImportHistoryTable />
    </Tabs>
  );
}
