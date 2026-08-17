import { useCallback, useState } from "react";
import type { ApiError } from "@/types/api.types";
import { syncBroker } from "../api/import.api";
import type { SyncBrokerCommand } from "../types/import.types";
import { toast } from "sonner";
import { handleApiError } from "@/lib/utils";

export function useSyncBrokerMutation() {
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState<ApiError[] | null>(null);

  const mutate = useCallback(async (syncBrokerCommand: SyncBrokerCommand) => {
    setIsPending(true);
    return await syncBroker(syncBrokerCommand)
      .then((data) => {
        toast.success(`Broker sync completed (${data.newTrades} new trades)`, {
          duration: 10000,
        });
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
