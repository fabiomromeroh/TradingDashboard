import { useState, useCallback } from "react";
import type {
  DashboardLayout,
  DashboardWidget,
  WidgetType,
  WidgetZone,
} from "../types/dashboard.types";

const STORAGE_KEY = "td-dashboard-layout";

const defaultLayout: DashboardLayout = {
  overview: [
    { id: "default-net-pnl", type: "net-pnl", zone: "overview" },
    { id: "default-win-rate", type: "win-rate", zone: "overview" },
    { id: "default-profit-factor", type: "profit-factor", zone: "overview" },
    { id: "default-day-win-rate", type: "day-win-rate", zone: "overview" },
    { id: "default-avg-win-loss", type: "avg-win-loss", zone: "overview" },
  ],
  main: [
    { id: "default-equity-curve", type: "equity-curve", zone: "main" },
    { id: "default-daily-pnl-bar", type: "daily-pnl-bar", zone: "main" },
    { id: "default-win-loss-donut", type: "win-loss-donut", zone: "main" },
    { id: "default-monthly-pnl", type: "monthly-pnl", zone: "main" },
  ],
};

function loadLayout(): DashboardLayout {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (raw) return JSON.parse(raw) as DashboardLayout;
  } catch {
    // ignore parse errors
  }
  return defaultLayout;
}

function persist(layout: DashboardLayout) {
  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(layout));
  } catch {
    // ignore storage errors
  }
}

export function useDashboardLayout() {
  const [layout, setLayout] = useState<DashboardLayout>(loadLayout);

  const saveLayout = useCallback((next: DashboardLayout) => {
    setLayout(next);
    persist(next);
  }, []);

  const addWidget = useCallback((type: WidgetType, zone: WidgetZone) => {
    const widget: DashboardWidget = {
      id: `${type}-${crypto.randomUUID()}`,
      type,
      zone,
    };
    setLayout((prev) => {
      const next = { ...prev, [zone]: [...prev[zone], widget] };
      persist(next);
      return next;
    });
  }, []);

  const removeWidget = useCallback((id: string, zone: WidgetZone) => {
    setLayout((prev) => {
      const next = { ...prev, [zone]: prev[zone].filter((w) => w.id !== id) };
      persist(next);
      return next;
    });
  }, []);

  return { layout, saveLayout, addWidget, removeWidget };
}
