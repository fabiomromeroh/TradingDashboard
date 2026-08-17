import { useCallback, useState } from "react";
import { uploadImport } from "../api/import.api";
import type { UploadImportDto } from "../types/import.types";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

export function useUploadImportMutation() {
  const [importResult, setImportResult] = useState<UploadImportDto>();
  const [isPending, setIsPending] = useState<boolean>(false);
  const [errors, setErrors] = useState<ApiError[] | null>(null);

  const mutate = useCallback(
    async (file: File, accountId?: string, brokerName?: string) => {
      setIsPending(true);
      return await uploadImport(file, accountId, brokerName)
        .then((result) => {
          setImportResult(result);
          setErrors(null);
          return true;
        })
        .catch((response) => {
          setErrors(handleApiError(response));
          return false;
        })
        .finally(() => {
          setIsPending(false);
        });
    },
    [],
  );

  return { mutate, importResult, isPending, error: errors };
}
