import { useCallback, useState } from "react";
import { createUser } from "../api/usersApi";
import type { CreateUserCommand } from "../types/user.type";

export function useCreateUser() {
  const [error, setError] = useState<string | null>(null);
  const [isCreating, setIsCreating] = useState(true);

  const create = useCallback(async (user: CreateUserCommand) => {
    return createUser(user)
      .then(() => {
        return true;
      })
      .catch((err: unknown) => {
        const message =
          err instanceof Error ? err.message : "Failed to create user";
        setError(message);
        return false;
      })
      .finally(() => {
        setIsCreating(false);
      });
  }, []);

  return { error, isCreating, execute: create };
}
