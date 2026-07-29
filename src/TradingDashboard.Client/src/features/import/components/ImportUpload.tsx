import { FileUpload } from "@/components/shared/FileUpload";
import { useUploadImportMutation } from "../hooks/useUploadImportMutation";
import { PreviewImportModal } from "./PreviewImportModal";
import { useState } from "react";
import { Card, CardContent } from "@/components/ui/card";
import type { ImportUploadProps } from "../types/import.types";

export function ImportUpload({
  selectedAccount,
  brokerName,
  onImportCompleted,
}: ImportUploadProps) {
  const {
    mutate: uploadFile,
    importResult,
    isPending: isUploading,
  } = useUploadImportMutation();
  const [showPreview, setShowPreview] = useState<boolean>(false);
  const [uploadKey, setUploadKey] = useState(0);

  const cancelUpload = () => {
    setShowPreview(false);
    setUploadKey((k) => k + 1);
  };

  const handleFileUpload = async (file: File) => {
    const success = await uploadFile(file, selectedAccount, brokerName);

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
              onImportCompleted={onImportCompleted}
            />
          )}
          <div className="grid gap-4 col-start-1">
            <FileUpload
              isUploading={isUploading}
              disabled={!selectedAccount || !brokerName}
              key={uploadKey}
              handleUpload={(file) => handleFileUpload(file)}
            />
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
