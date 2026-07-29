import { useCallback, useState } from "react";
import { setBrokerCredentials } from "../api/account.api";
import type { SetCredentialsCommand } from "../types/account.types";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";
import { toast } from "sonner";

export function useSetBrokerCredentialsMutation() {
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState<ApiError[] | null>(null);

  const mutate = useCallback(
    async (accountId: string, credentials: SetCredentialsCommand) => {
      setIsPending(true);
      return await setBrokerCredentials(accountId, credentials)
        .then(() => {
          toast.success("Broker credentials saved successfully");
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
    },
    [],
  );

  return { mutate, isPending, error };
}
