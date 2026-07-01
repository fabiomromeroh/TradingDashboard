import { useState, useEffect, useCallback } from "react";
import { getTradesByAccountId } from "../api/tradesApi";
import type { Trade, UseTradesResult } from "../types/trade.types";
import { useAppSelector } from "@/store/hooks";

export function useTrades(): UseTradesResult {
  const [trades, setTrades] = useState<Trade[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const selectedAccounts = useAppSelector<string[]>(
    (x) => x.account.selectedAccounts,
  );

  // useCallback ensures `fetchTrades` keeps a stable reference so it's safe
  // to pass as a prop or put in a dependency array without infinite loops.
  const fetchTrades = useCallback(async () => {
    console.log(selectedAccounts);

    await getTradesByAccountId(selectedAccounts)
      .then((data) => {
        setTrades(data);
        setError(null);
      })
      .catch((err: Error) => {
        setError(err.message ?? "Failed to fetch trades");
        setTrades([]);
      })
      .finally(() => setIsLoading(false));
  }, [selectedAccounts]);

  useEffect(() => {
    if (!selectedAccounts) return;
    fetchTrades();
  }, [fetchTrades, selectedAccounts]);

  return { trades, isLoading, error, refetch: fetchTrades };
}
