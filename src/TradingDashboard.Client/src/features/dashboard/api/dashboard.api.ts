import apiClient from "@/lib/apiClient";
import type {
  DashboardMetricParams,
  WidgetDto,
} from "../types/dashboard.types";

export async function getDashboardMetric(
  params: DashboardMetricParams,
): Promise<WidgetDto> {
  return apiClient.get("/dashboard/metric", {
    params: {
      metricType: params.metricType,
    },
  });
}
