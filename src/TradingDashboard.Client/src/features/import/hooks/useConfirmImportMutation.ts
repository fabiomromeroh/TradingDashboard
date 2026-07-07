import { useCallback, useState } from "react";
import type { ConfirmImportCommand } from "../types/import.types";
import { confirmImport } from "../api/import.api";

export function useConfirmImportMutation() {
  const [isPending, setIsPending] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const mutate = useCallback(
    async (confirmImportCommand: ConfirmImportCommand) => {
      setIsPending(true);
      return await confirmImport(confirmImportCommand)
        .then(() => {
          return true;
        })
        .catch((error) => {
          setError(error.message);
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
