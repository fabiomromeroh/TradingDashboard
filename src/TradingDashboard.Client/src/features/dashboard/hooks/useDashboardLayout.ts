import { useCallback } from "react";
import { useAppSelector, useAppDispatch } from "@/store/hooks";
import type {
  DashboardWidget,
  WidgetType,
  WidgetZone,
} from "../types/dashboard.types";
import type { DashboardConfig } from "@/features/users/types/user.types";
import { useConfigDashboardMutation } from "@/features/users/hooks/useConfigDashboardMutation";
import { setConfigDashboard } from "@/store/store";

const defaultLayout: DashboardWidget[] = [
  { type: "net-pnl", zone: "overview" },
  { type: "win-rate", zone: "overview" },
  { type: "profit-factor", zone: "overview" },
  { type: "avg-win-loss", zone: "overview" },

  { type: "net-pnl-curve", zone: "main" },
  { type: "daily-pnl-bar", zone: "main" },
  // { type: "win-loss-donut", zone: "main" },
  { type: "monthly-pnl", zone: "main" },
];

function loadLayout(dashboardConfig: DashboardConfig[]): DashboardConfig[] {
  if (dashboardConfig && dashboardConfig.length > 0) {
    return dashboardConfig;
  }

  return defaultLayout as DashboardConfig[];
}

export function useDashboardLayout() {
  const dispatch = useAppDispatch();

  const dashboardConfig = useAppSelector((x) => x.auth.dashboardConfig);
  const layout = loadLayout(dashboardConfig);

  const { mutate: updateConfigDashboard } = useConfigDashboardMutation();

  const saveLayout = useCallback((next: DashboardConfig[]) => {
    updateConfigDashboard(next, {
      onSuccess: () => {
        dispatch(setConfigDashboard(next));
      },
    });
  }, []);

  const addWidget = useCallback(
    (type: WidgetType, zone: WidgetZone) => {
      const widget: DashboardConfig = {
        type,
        zone,
      };
      saveLayout([...layout, widget]);
    },
    [layout, saveLayout],
  );

  const removeWidget = useCallback(
    (type: WidgetType, zone: WidgetZone) => {
      const next = layout.filter((w) => !(w.type === type && w.zone === zone));

      saveLayout(next);
    },
    [layout, saveLayout],
  );

  return { layout, saveLayout, addWidget, removeWidget };
}
