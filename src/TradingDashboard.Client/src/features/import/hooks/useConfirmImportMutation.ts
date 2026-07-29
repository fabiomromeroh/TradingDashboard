import { useCallback, useState } from "react";
import type { ConfirmImportCommand } from "../types/import.types";
import { confirmImport } from "../api/import.api";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

export function useConfirmImportMutation() {
  const [isPending, setIsPending] = useState<boolean>(false);
  const [error, setError] = useState<ApiError[] | null>(null);

  const mutate = useCallback(
    async (confirmImportCommand: ConfirmImportCommand) => {
      setIsPending(true);
      return await confirmImport(confirmImportCommand)
        .then(() => {
          return true;
        })
        .catch((error) => {
          const errors = handleApiError(error);
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
