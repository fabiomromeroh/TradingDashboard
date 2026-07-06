import { loginUserApi } from "@/features/users/api/usersApi";
import type { LoginUserCommand } from "@/features/users/types/user.type";
import { handleApiError } from "@/lib/utils";
import { useAppDispatch } from "@/store/hooks";
import { setAccessToken, setUser } from "@/store/store";
import type { ApiError, ApiErrorResponse } from "@/types/api.types";
import { useCallback, useState } from "react";

export function useLoginUser() {
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<ApiError[]>([]);
  const dispatch = useAppDispatch();

  const loginUser = useCallback(async (loginData: LoginUserCommand) => {
    setLoading(true);
    return await loginUserApi(loginData)
      .then((data) => {
        dispatch(setUser(data.user));

        dispatch(setAccessToken(data.accessToken));
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

  return { loginUser, errors, loading };
}
