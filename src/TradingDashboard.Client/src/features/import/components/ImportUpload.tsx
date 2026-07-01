import { FileUpload } from "@/components/shared/FileUpload";
import { useUploadImport } from "../hooks/useUploadImport";
import { PreviewImportModal } from "./PreviewImportModal";
import { useState } from "react";
import { toast } from "sonner";
import { AppSelect } from "@/components/shared/AppSelect";
import { useAppSelector } from "@/store/hooks";
import { toSelectOptions } from "@/lib/utils";
import { Label } from "@/components/ui/label";
import { useBrokers } from "../hooks/useBrokers";
import { Card, CardContent } from "@/components/ui/card";

export function ImportUpload({ accountId }: { accountId?: string }) {
  const { uploadFile, importResult, error } = useUploadImport();
  const [showPreview, setShowPreview] = useState<boolean>(false);
  const [uploadKey, setUploadKey] = useState(0);
  const [selectedAccount, setSelectedAccount] = useState<string>(
    accountId || "",
  );
  const [selectedBroker, setSelectedBroker] = useState<string>("");
  const [selectedBrokerLabel, setSelectedBrokerLabel] = useState<string>("");
  const [isImporting, setIsImporting] = useState<boolean>(false);

  const { brokers: brokerOptions } = useBrokers();

  const accounts = useAppSelector((x) => x.account.accounts);
  const accountOptions = toSelectOptions(accounts);

  if (!selectedAccount && accountOptions.length > 0)
    setSelectedAccount(accountOptions[0].value);

  const cancelUpload = () => {
    setShowPreview(false);
    setUploadKey((k) => k + 1);
  };

  const handleUploadFIle = async (file: File) => {
    setIsImporting(true);

    const success = await uploadFile(
      file,
      selectedAccount,
      selectedBrokerLabel,
    );

    if (success) {
      setShowPreview(true);
      setIsImporting(false);
    } else {
      toast.error(
        error ?? "Failed to import file, please check file formatting.",
      );
      setIsImporting(false);
    }
  };

  return (
    <Card>
      <CardContent>
        <div className="grid grid-cols-2 gap-4">
          {importResult && (
            <PreviewImportModal
              {...importResult}
              cancelUpload={cancelUpload}
              showPreview={showPreview}
            />
          )}
          <div className="grid gap-4 col-start-1">
            <Label htmlFor="accountSelect">Account </Label>
            <AppSelect
              name="accountSelect"
              options={accountOptions}
              value={selectedAccount}
              placeholder="Select an account.."
              onChange={(value) => setSelectedAccount(value)}
              className="w-full"
              groupLabel="Accounts"
            />

            <Label htmlFor="brokerSelect">Broker </Label>
            <AppSelect
              name="brokerSelect"
              options={brokerOptions}
              value={selectedBroker}
              placeholder="Select a broker.."
              onChange={(value) => {
                setSelectedBroker(value);
                const brokerLabel =
                  brokerOptions.find((x) => x.value === value)?.label || "";
                setSelectedBroker(value);
                setSelectedBrokerLabel(brokerLabel);
              }}
              className="w-full"
              groupLabel="Brokers"
            />

            <FileUpload
              isImporting={isImporting}
              disabled={!selectedAccount || !selectedBroker}
              key={uploadKey}
              handleUpload={(file) => handleUploadFIle(file)}
            />
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
