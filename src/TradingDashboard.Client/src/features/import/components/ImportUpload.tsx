import { FileUpload } from "@/components/shared/FileUpload";
import { useUploadImport } from "../hooks/useUploadImport";
import { PreviewImportModal } from "./PreviewImportModal";
import { useEffect, useState } from "react";
import { AppSelect } from "@/components/shared/AppSelect";
import { useAppSelector } from "@/store/hooks";
import { toSelectOptions } from "@/lib/utils";
import { Label } from "@/components/ui/label";
import { useBrokers } from "../hooks/useBrokers";
import { Card, CardContent } from "@/components/ui/card";

export function ImportUpload({
  // accountId,
  selectedAccount,
  onSelectedAccountChange,
}: {
  accountId?: string;
  selectedAccount: string;
  onSelectedAccountChange: (value: string) => void;
}) {
  const { uploadFile, importResult, isUploading } = useUploadImport();
  const [showPreview, setShowPreview] = useState<boolean>(false);
  const [uploadKey, setUploadKey] = useState(0);
  const [selectedBroker, setSelectedBroker] = useState<string>("");
  const [selectedBrokerLabel, setSelectedBrokerLabel] = useState<string>("");

  const { brokers: brokerOptions } = useBrokers();

  const accounts = useAppSelector((x) => x.account.accounts);
  const accountOptions = toSelectOptions(accounts);

  useEffect(() => {
    if (selectedAccount && selectedAccount !== "") {
      onSelectedAccountChange(selectedAccount);
      return;
    }

    if (accountOptions.length > 0) {
      onSelectedAccountChange(accountOptions[0].value);
    }
  }, [accountOptions, onSelectedAccountChange, selectedAccount]);

  const cancelUpload = () => {
    setShowPreview(false);
    setUploadKey((k) => k + 1);
  };

  const handleUploadFIle = async (file: File) => {
    const success = await uploadFile(
      file,
      selectedAccount,
      selectedBrokerLabel,
    );

    if (success) {
      setShowPreview(true);
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
              setShowPreview={setShowPreview}
            />
          )}
          <div className="grid gap-4 col-start-1">
            <Label htmlFor="accountSelect">Account </Label>
            <AppSelect
              name="accountSelect"
              options={accountOptions}
              value={selectedAccount}
              placeholder="Select an account.."
              onChange={(value) => onSelectedAccountChange(value)}
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
              isUploading={isUploading}
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
