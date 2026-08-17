import { useCallback, useEffect, useState } from "react";
import type { AccountDto, UseAccountsResult } from "../types/account.types";
import { getAccounts } from "../api/account.api";
import { useAppSelector } from "@/store/hooks";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

export function useAccountsQuery(): UseAccountsResult {
  const [accounts, setAccounts] = useState<AccountDto[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<ApiError[] | null>(null);
  const userId = useAppSelector((state) => state.auth.user?.id);

  const fetchAccounts = useCallback(async () => {
    if (userId)
      return await getAccounts(userId)
        .then((data) => {
          setAccounts(data);
          setError(null);
        })
        .catch((response) => {
          setError(handleApiError(response));
          return false;
        })
        .finally(() => setIsLoading(false));
  }, [userId]);

  useEffect(() => {
    if (userId) fetchAccounts();
  }, [userId, fetchAccounts]);

  return { accounts, isLoading, error, refetch: fetchAccounts };
}
