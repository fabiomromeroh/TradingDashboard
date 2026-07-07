import { useCallback, useState } from "react";
import { createAccount } from "../api/account.api";
import type { CreateAccountCommand } from "../types/account.types";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

export function useCreateAccountMutation() {
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState<ApiError[] | null>(null);

  const mutate = useCallback(async (accountData: CreateAccountCommand) => {
    return await createAccount(accountData)
      .then(() => {
        return true;
      })
      .catch((response) => {
        const errors = handleApiError(response);
        setError(errors);
        return false;
      })
      .finally(() => {
        setIsPending(false);
      });
  }, []);

  return { mutate, isPending, error };
}
