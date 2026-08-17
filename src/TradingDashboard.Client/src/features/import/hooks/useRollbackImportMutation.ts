import { useCallback, useState } from "react";
import { rollbackImport } from "../api/import.api";
import { handleApiError } from "@/lib/utils";
import { toast } from "sonner";
import type { ApiError } from "@/types/api.types";

export function useRollbackImportMutation() {
  const [isPending, setIsPending] = useState<boolean>(false);
  const [errors, setErrors] = useState<ApiError[] | null>(null);
  const mutate = useCallback(async (id: string | undefined) => {
    if (id) {
      setIsPending(true);

      return await rollbackImport(id)
        .then(() => {
          toast.success("Rollback successful");

          return true;
        })
        .catch((response) => {
          setErrors(handleApiError(response));
          return false;
        })
        .finally(() => {
          setIsPending(false);
        });
    }
  }, []);

  return { mutate, isPending, error: errors };
}
