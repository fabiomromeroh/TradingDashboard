import { useCallback, useEffect, useState } from "react";
import { getBrokers } from "../api/broker.api";
import { handleApiError, toSelectOptions } from "@/lib/utils";
import type { ApiError, SelectOption } from "@/types/api.types";

export function useBrokersQuery() {
  const [brokers, setBrokers] = useState<SelectOption[]>([]);

  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<ApiError[] | null>(null);

  const fetchBrokers = useCallback(async () => {
    return await getBrokers()
      .then((data) => {
        const result = toSelectOptions(data);

        setBrokers(result);
      })
      .catch((response) => {
        setError(handleApiError(response));
        return false;
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  useEffect(() => {
    fetchBrokers();
  }, [fetchBrokers]);

  return { brokers, isLoading, error, refetch: fetchBrokers };
}
