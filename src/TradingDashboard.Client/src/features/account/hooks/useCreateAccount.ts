import { useCallback, useState } from "react";
import { createAccount } from "../api/accountApi";
import type { CreateAccountCommand } from "../types/account.types";

export function useCreateAccount() {
  const [loading, setLoading] = useState(true as boolean);
  const [error, setError] = useState<string | null>(null);

  const create = useCallback(async (accountData: CreateAccountCommand) => {
    return await createAccount(accountData)
      .then(() => {
        return true;
      })
      .catch((err: unknown) => {
        const message =
          err instanceof Error ? err.message : "Failed to create account";
        setError(message);
        console.error(message);
        return false;
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  return { create, loading, error };
}
