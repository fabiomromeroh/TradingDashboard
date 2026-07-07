import { useCallback, useState } from "react";
import { createUser } from "../api/users.api";
import type { CreateUserCommand } from "../types/user.types";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

export function useCreateUserMutation() {
  const [error, setError] = useState<ApiError[] | null>(null);
  const [isPending, setIsPending] = useState(false);

  const mutate = useCallback(async (user: CreateUserCommand) => {
    setIsPending(true);
    return createUser(user)
      .then((data) => {
        return data;
      })
      .catch((response) => {
        const errors = handleApiError(response);
        setError(errors);
        return false;
      })
      .finally(() => {
        setIsPending(false);
      });
  }, []);

  return { mutate, isPending, error };
}
