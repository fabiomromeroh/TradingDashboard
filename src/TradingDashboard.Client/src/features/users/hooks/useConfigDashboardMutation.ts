import { useApiMutation } from "@/hooks/useApiMutation";
import { updateConfigDashboard } from "../api/users.api";

export function useConfigDashboardMutation() {
  return useApiMutation(updateConfigDashboard);
}
