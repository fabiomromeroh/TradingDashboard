import { useState, useEffect, useCallback } from "react";
import { getTradesByAccountId } from "../api/trades.api";
import type { TradeDto, UseTradesResult } from "../types/trade.types";
import { useAppSelector } from "@/store/hooks";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

export function useTradesQuery(): UseTradesResult {
  const [trades, setTrades] = useState<TradeDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<ApiError[] | null>(null);
  const selectedAccounts = useAppSelector<string[]>(
    (x) => x.auth.configFilters.accountIds,
  );

  // useCallback ensures `fetchTrades` keeps a stable reference so it's safe
  // to pass as a prop or put in a dependency array without infinite loops.
  const fetchTrades = useCallback(async () => {
    return await getTradesByAccountId(selectedAccounts)
      .then((data) => {
        setTrades(data);
        setError(null);
      })
      .catch((response) => {
        setError(handleApiError(response));
        return false;
      })
      .finally(() => setIsLoading(false));
  }, [selectedAccounts]);

  useEffect(() => {
    if (!selectedAccounts) return;
    fetchTrades();
  }, [fetchTrades, selectedAccounts]);

  return { trades, isLoading, error, refetch: fetchTrades };
}
