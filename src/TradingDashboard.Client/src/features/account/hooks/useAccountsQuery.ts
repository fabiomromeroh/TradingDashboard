import { useCallback, useEffect, useState } from "react";
import type { AccountDto, UseAccountsResult } from "../types/account.types";
import { getAccounts } from "../api/account.api";
import { useAppSelector } from "@/store/hooks";

export function useAccountsQuery(): UseAccountsResult {
  const [accounts, setAccounts] = useState<AccountDto[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const userId = useAppSelector((state) => state.auth.user?.id);

  const fetchAccounts = useCallback(async () => {
    if (userId)
      await getAccounts(userId)
        .then((data) => {
          setAccounts(data);
          setError(null);
        })
        .catch((err: Error) => {
          setError(err.message ?? "Failed to fetch accounts");
          setAccounts([]);
        })
        .finally(() => setIsLoading(false));
  }, [userId]);

  useEffect(() => {
    if (userId) fetchAccounts();
  }, [userId, fetchAccounts]);

  return { accounts, isLoading, error, refetch: fetchAccounts };
}
