import { logout } from "@/features/users/api/users.api";
import { handleApiError } from "@/lib/utils";
import { useAppDispatch } from "@/store/hooks";
import { logout as clearAuthState } from "@/store/store";
import type { ApiError, ApiErrorResponse } from "@/types/api.types";
import { useCallback, useState } from "react";

export function useLogoutMutation() {
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState<ApiError[]>([]);
  const dispatch = useAppDispatch();

  const mutate = useCallback(async () => {
    setIsPending(true);
    return await logout()
      .then(() => {
        dispatch(clearAuthState());
        return true;
      })
      .catch((response: ApiErrorResponse) => {
        const errors = handleApiError(response);
        setError(errors);
        return false;
      })
      .finally(() => {
        setIsPending(false);
      });
  }, [dispatch]);

  return { mutate, error, isPending };
}
