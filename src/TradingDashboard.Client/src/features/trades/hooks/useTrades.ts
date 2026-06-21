import { useState, useEffect, useCallback } from 'react';
import { getTrades } from '../api/tradesApi';
import type { Trade, UseTradesResult } from '../types/trade.types';



export function useTrades(): UseTradesResult {
  const [trades, setTrades] = useState<Trade[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // useCallback ensures `fetchTrades` keeps a stable reference so it's safe
  // to pass as a prop or put in a dependency array without infinite loops.
  const fetchTrades = useCallback(async () => {
    setIsLoading(true);
    await getTrades()
      .then((data) => {
        setTrades(data);
        setError(null);
      })
      .catch((err: Error) => {
        setError(err.message ?? 'Failed to fetch trades');
        setTrades([]);
      })
      .finally(() => setIsLoading(false));
  }, []);

  useEffect(() => {
    fetchTrades();
  }, [fetchTrades]);

  return { trades, isLoading, error, refetch: fetchTrades };
}

