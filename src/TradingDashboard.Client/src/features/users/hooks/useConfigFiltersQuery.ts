import { useCallback, useEffect, useState } from "react";
import type { ApiError } from "@/types/api.types";
import { getConfigFilters } from "../api/users.api";
import type { ConfigDto, ConfigFiltersQueryResult } from "../types/user.types";

export function useConfigFiltersQuery(): ConfigFiltersQueryResult {
  const [config, setConfig] = useState<ConfigDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<ApiError[] | null>(null);

  const fetchConfig = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    return await getConfigFilters()
      .then((data) => {
        setConfig(data);
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
