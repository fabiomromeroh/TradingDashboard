import { useCallback, useEffect, useState } from "react";
import { getImportHistoryApi } from "../api/importApi";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";
import type { ImportHistory } from "../types/import.types";

export function useImportHistory(accountId?: string) {
  const [importHistory, setImportHistory] = useState<ImportHistory[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errors, setErrors] = useState<ApiError[]>([]);

  const getImportHistory = useCallback(async () => {
    if (!accountId) {
      setImportHistory([]);
      setErrors([]);
      setIsLoading(false);
      return;
    }

    setIsLoading(true);

    getImportHistoryApi(accountId)
      .then((data) => {
        setImportHistory(data);
      })
      .catch((response) => {
        const errors = handleApiError(response);
        setErrors(errors);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, [accountId]);

  useEffect(() => {
    getImportHistory();
  }, [getImportHistory]);

  return { getImportHistory, importHistory, isLoading, errors };
}
