import { useCallback } from "react";
import { rollbackImportApi } from "../api/importApi";
import { handleApiError } from "@/lib/utils";
import { toast } from "sonner";

export function useRollbackImport() {
  const rollbackImport = useCallback(async (id: string | undefined) => {
    if (id)
      return await rollbackImportApi(id)
        .then(() => {
          toast.success("Rollback successful");

          return true;
        })
        .catch((response) => {
          handleApiError(response);
          return false;
        });
  }, []);

  return { rollbackImport };
}
