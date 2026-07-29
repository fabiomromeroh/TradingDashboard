import { useCallback, useEffect, useState } from "react";
import { getUsers } from "../api/users.api";
import type { UserDto } from "../types/user.types";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

interface UseUsersResult {
  data: UserDto[];
  error: ApiError[] | null;
  isLoading: boolean;
  refetch: () => void;
}

export function useUsersQuery(): UseUsersResult {
  const [users, setUsers] = useState<UserDto[]>([]);
  const [error, setErrors] = useState<ApiError[] | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const fetchUsers = useCallback(async () => {
    await getUsers()
      .then((data) => {
        setUsers(data);
      })
      .catch((response) => {
        const errors = handleApiError(response);
        setErrors(errors);
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  return { data: users, error, isLoading, refetch: fetchUsers };
}
