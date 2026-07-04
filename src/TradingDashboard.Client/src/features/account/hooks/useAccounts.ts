import { useCallback, useEffect, useState } from "react";
import type { AccountQuery } from "../types/account.types";
import { getAccounts } from "../api/accountApi";
import { useAppSelector } from "@/store/hooks";

interface UseAccountsResult {
  accounts: AccountQuery[];
  error: string | null;
  isLoading: boolean;
  refetch: () => void;
}

export function useAccounts(): UseAccountsResult {
  const [accounts, setAccounts] = useState<AccountQuery[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const userId = useAppSelector((state) => state.user.id);

  const fetchAccounts = useCallback(async () => {
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
