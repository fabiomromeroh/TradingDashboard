import { useCallback, useState } from "react";
import { uploadImport } from "../api/importApi";
import type { UploadImport } from "../types/import.types";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

export function useUploadImport() {
  const [importResult, setImportResult] = useState<UploadImport>();
  const [isUploading, setIsUploading] = useState<boolean>(false);
  const [errors, setErrors] = useState<ApiError[] | null>(null);

  const uploadFile = useCallback(
    async (file: File, accountId?: string, brokerName?: string) => {
      setIsUploading(true);
      return await uploadImport(file, accountId, brokerName)
        .then((result) => {
          setImportResult(result);
          setErrors(null);
          return true;
        })
        .catch((response) => {
          const errors = handleApiError(response);
          setErrors(errors);
          return false;
        })
        .finally(() => {
          setIsUploading(false);
        });
    },
    [],
  );

  return { uploadFile, importResult, isUploading, errors };
}
