import { useApiMutation } from "@/hooks/useApiMutation";
import { updateConfigFilters } from "../api/users.api";

export function useConfigFiltersMutation() {
  return useApiMutation(updateConfigFilters);
}
