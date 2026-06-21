import { useCallback, useEffect, useState } from "react";
import type { Account } from "../types/account.types";
import { getAccounts } from "../api/accountApi";

export function useAccounts() {
  const [accounts, setAccounts] = useState<Account[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const fetchAccounts = useCallback(async () => {
    setIsLoading(true);
    await getAccounts()
      .then((data) => {
        console.log("Fetched accounts:", data); // Debugging log
        setAccounts(data);
        setError(null);
      })
      .catch((err: Error) => {
        setError(err.message ?? "Failed to fetch accounts");
        setAccounts([]);
      })
      .finally(() => setIsLoading(false));

    }, []);

    useEffect(() => {
      fetchAccounts();
    }, [fetchAccounts]);

  return { accounts, isLoading, error, refetch: fetchAccounts };
}