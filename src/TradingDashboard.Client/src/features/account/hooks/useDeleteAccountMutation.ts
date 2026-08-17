import { useCallback, useState } from "react";
import { deleteAccount } from "../api/account.api";
import type { ApiError } from "@/types/api.types";
import { toast } from "sonner";
import { handleApiError } from "@/lib/utils";

export function useDeleteAccountMutation() {
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState<ApiError[] | null>(null);

  const mutate = useCallback(async (accountId: string) => {
    setIsPending(true);
    return await deleteAccount(accountId)
      .then(() => {
        toast.success("Account deleted successfully");
        return true;
      })
      .catch((response) => {
        setError(handleApiError(response));
        return false;
      })
      .finally(() => {
        setIsPending(false);
      });
  }, []);

  return { mutate, isPending, error };
}
