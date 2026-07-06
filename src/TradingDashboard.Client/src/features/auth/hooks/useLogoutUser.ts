import { logoutUserApi } from "@/features/users/api/usersApi";
import { handleApiError } from "@/lib/utils";
import { useAppDispatch } from "@/store/hooks";
import { logout } from "@/store/store";
import type { ApiError, ApiErrorResponse } from "@/types/api.types";
import { useCallback, useState } from "react";

export function useLogoutUser() {
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<ApiError[]>([]);
  const dispatch = useAppDispatch();

  const logoutUser = useCallback(async () => {
    setLoading(true);
    return await logoutUserApi()
      .then(() => {
        dispatch(logout());
        return true;
      })
      .catch((response: ApiErrorResponse) => {
        const errors = handleApiError(response);
        setErrors(errors);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  return { logoutUser, errors, loading };
}
