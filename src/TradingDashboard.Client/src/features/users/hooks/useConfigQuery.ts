import { useCallback, useEffect, useState } from "react";
import type { ApiError } from "@/types/api.types";
import type { ConfigQueryResult } from "../types/user.types";
import { useAppDispatch } from "@/store/hooks";
import { getUserConfig } from "../api/users.api";
import { loadUserConfig } from "@/store/store";

export function useConfigQuery(): ConfigQueryResult {
  const dispatch = useAppDispatch();
  const [config, setConfig] = useState<any | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<ApiError[] | null>(null);

  const fetchConfig = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    return await getUserConfig()
      .then((data) => {
        setConfig(data.configs);
        dispatch(loadUserConfig(data));

        setError(null);
        return data;
      })
      .catch((response) => {
        const apiErrorResponse = response as ApiError[];
        setError(apiErrorResponse);
        return null;
      })
      .finally(() => setIsLoading(false));
  }, []);

  useEffect(() => {
    fetchConfig();
  }, [fetchConfig]);

  return { config, isLoading, error, refetch: fetchConfig };
}
