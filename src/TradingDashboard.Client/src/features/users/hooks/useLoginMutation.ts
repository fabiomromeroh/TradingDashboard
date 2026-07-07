import { login } from "@/features/users/api/users.api";
import type { LoginUserCommand } from "@/features/users/types/user.types";
import { handleApiError } from "@/lib/utils";
import { useAppDispatch } from "@/store/hooks";
import { setAccessToken, setUser } from "@/store/store";
import type { ApiError, ApiErrorResponse } from "@/types/api.types";
import { useCallback, useState } from "react";

export function useLoginMutation() {
  const [isPending, setIsPending] = useState(false);
  const [error, setError] = useState<ApiError[]>([]);
  const dispatch = useAppDispatch();

  const mutate = useCallback(
    async (loginData: LoginUserCommand) => {
      setIsPending(true);
      return await login(loginData)
        .then((data) => {
          dispatch(setUser(data.user));
          dispatch(setAccessToken(data.accessToken));
          setError([]);
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
    },
    [dispatch],
  );

  return { mutate, error, isPending };
}
