import { useCallback, useState } from "react";
import { uploadImport } from "../api/importApi";
import type { UploadImport } from "../types/import.types";

export function useUploadImport() {
  const [importResult, setImportResult] = useState<UploadImport>();
  const [isUploading, setIsUploading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const uploadFile = useCallback(
    async (file: File, accountId?: string, brokerName?: string) => {
      return await uploadImport(file, accountId, brokerName)
        .then((result) => {
          setImportResult(result);
          setError(null);
          return true;
        })
        .catch((response) => {
          console.error("Error uploading import:", response.errors);
          setError(response.errors[0]);
          return false;
        })
        .finally(() => {
          setIsUploading(false);
        });
    },
    [],
  );

  return { uploadFile, importResult, isUploading, error };
}
