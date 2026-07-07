import { useCallback, useEffect, useState } from "react";
import { getImportHistory } from "../api/import.api";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";
import type { ImportHistory } from "../types/import.types";

export function useImportHistoryQuery(accountId?: string) {
  const [importHistory, setImportHistory] = useState<ImportHistory[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errors, setErrors] = useState<ApiError[]>([]);

  const fetchImportHistory = useCallback(async () => {
    if (!accountId) {
      setImportHistory([]);
      setErrors([]);
      setIsLoading(false);
      return;
    }

    setIsLoading(true);

    getImportHistory(accountId)
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
    fetchImportHistory();
  }, [fetchImportHistory]);

  return { importHistory, isLoading, errors, refetch: fetchImportHistory };
}
