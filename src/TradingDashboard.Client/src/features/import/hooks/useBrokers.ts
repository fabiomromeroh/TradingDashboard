import { useCallback, useEffect, useState } from "react";
import { getBrokers } from "../api/brokerApi";
import { toSelectOptions } from "@/lib/utils";
import type { SelectOption } from "@/types/api.types";

export function useBrokers() {
  const [brokers, setBrokers] = useState<SelectOption[]>([]);

  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const fetchBrokers = useCallback(() => {
    getBrokers()
      .then((data) => {
        const result = toSelectOptions(data);

        setBrokers(result);
      })
      .catch((err: Error) => {
        console.error("Error fetching brokers:", err);
        setBrokers([]);
        setError("Failed to fetch brokers.");
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
