import { login } from "@/features/users/api/users.api";
import { useApiMutation } from "@/hooks/useApiMutation";

export function useLoginMutation() {
  return useApiMutation(login);
}
