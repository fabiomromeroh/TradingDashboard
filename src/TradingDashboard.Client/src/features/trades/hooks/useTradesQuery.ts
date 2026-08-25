import { useState, useEffect, useCallback, useRef } from "react";
import {
  getTrades,
  getTradesPaginated,
  type PaginatedTradesResponse,
} from "../api/trades.api";
import type { TradeDto, UseTradesResult } from "../types/trade.types";
import { useAppSelector } from "@/store/hooks";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

export interface UseTradesQueryResult extends UseTradesResult {
  hasMore: boolean;
  isLoadingMore: boolean;
  onLoadMore: () => void;
}

export function useTradesQuery(paginated?: boolean): UseTradesQueryResult {
  const [trades, setTrades] = useState<TradeDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isLoadingMore, setIsLoadingMore] = useState(false);
  const [error, setError] = useState<ApiError[] | null>(null);
  const [hasMore, setHasMore] = useState(true);
  const [cursor, setCursor] = useState<string>("");
  const [totalCount, setTotalCount] = useState(0);
  const loadingRef = useRef(false);
  const selectedAccounts = useAppSelector<string[]>(
    (x) => x.auth.filtersConfig.accountIds,
  );

  const fetchTrades = useCallback(
    async (isInitial = true) => {
      if (!paginated) {
        setIsLoading(true);
        return await getTrades()
          .then((data) => {
            setTrades(data as TradeDto[]);
            setError(null);
          })
          .catch((response: any) => {
            setError(handleApiError(response));
            return false;
          })
          .finally(() => setIsLoading(false));
      }

      // Paginated fetch
      if (isInitial) {
        setIsLoading(true);
        setCursor("");
      } else {
        // Prevent duplicate calls while loading
        if (loadingRef.current || !hasMore) return;
        loadingRef.current = true;
        setIsLoadingMore(true);
      }

      try {
        const response = (await getTradesPaginated({
          pageSize: 100,
          cursor: isInitial ? "" : cursor,
        })) as PaginatedTradesResponse;

        setTrades((prevTrades) =>
          isInitial ? response.items : [...prevTrades, ...response.items],
        );
        setTotalCount(response.totalCount);
        setCursor(response.nextCursor || "");
        setHasMore(response.nextCursor !== null);
        setError(null);
      } catch (response: any) {
        setError(handleApiError(response));
      } finally {
        if (isInitial) {
          setIsLoading(false);
        } else {
          loadingRef.current = false;
          setIsLoadingMore(false);
        }
      }
    },
    [paginated, cursor, hasMore],
  );

  useEffect(() => {
    if (!selectedAccounts) return;
    fetchTrades(true);
  }, [selectedAccounts]);

  const onLoadMore = useCallback(() => {
    if (paginated && hasMore && !isLoadingMore && !loadingRef.current) {
      fetchTrades(false);
    }
  }, [paginated, hasMore, isLoadingMore, fetchTrades]);

  return {
    trades,
    isLoading,
    isLoadingMore,
    error,
    hasMore,
    onLoadMore,
    totalCount,
    refetch: () => fetchTrades(true),
  };
}
