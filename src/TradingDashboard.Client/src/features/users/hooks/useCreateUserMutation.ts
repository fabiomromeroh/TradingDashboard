import { createUser } from "../api/users.api";
import { useApiMutation } from "@/hooks/useApiMutation";

export function useCreateUserMutation() {
  return useApiMutation(createUser);
}
