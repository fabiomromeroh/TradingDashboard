import { useCallback, useState } from "react";
import type { ConfirmImportCommand } from "../types/import.types";
import { confirmImport } from "../api/importApi";

export function useConfirmImport() {
  const [isUploading, setIsUploading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const confirm = useCallback(
    async (confirmImportCommand: ConfirmImportCommand) => {
      return await confirmImport(confirmImportCommand)
        .then(() => {
          return true;
        })
        .catch((error) => {
          setError(error.message);
          return false;
        })
        .finally(() => {
          setIsUploading(false);
        });
    },
    [],
  );

  return { confirm, isUploading, error };
}
