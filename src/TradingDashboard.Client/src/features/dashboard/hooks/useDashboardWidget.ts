import { useCallback, useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { getDashboardMetric } from "../api/dashboard.api";
import type {
  WidgetDto,
  WidgetType,
  UseDashboardWidgetResult,
} from "../types/dashboard.types";
import { setMetrics } from "@/store/store";
import { handleApiError } from "@/lib/utils";
import type { ApiError } from "@/types/api.types";

export function useDashboardWidget(
  widgetType: WidgetType,
): UseDashboardWidgetResult {
  const [data, setData] = useState<WidgetDto | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<ApiError[] | null>(null);
  const dispatch = useAppDispatch();
  const filtersConfig = useAppSelector((x) => x.auth.filtersConfig);

  const fetchMetric = useCallback(async () => {
    setIsLoading(true);
    await getDashboardMetric({
      metricType: widgetType,
    })
      .then((dto) => {
        setData(dto);
        dispatch(setMetrics(dto));
        setError(null);
      })
      .catch((response) => {
        setError(handleApiError(response));
        return false;
      })
      .finally(() => setIsLoading(false));
  }, [widgetType, filtersConfig]);

  useEffect(() => {
    fetchMetric();
  }, [fetchMetric]);

  return { data, isLoading, error, refetch: fetchMetric };
}
