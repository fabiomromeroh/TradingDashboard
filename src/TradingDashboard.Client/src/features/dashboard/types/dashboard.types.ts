import type { ApiError } from "@/types/api.types";

export type OverviewWidgetType =
  | "net-pnl"
  | "win-rate"
  | "profit-factor"
  | "day-win-rate"
  | "avg-win-loss"
  | "total-trades"
  | "max-drawdown";

export type MainWidgetType =
  | "net-pnl-curve"
  | "daily-pnl-bar"
  | "win-loss-donut"
  | "monthly-pnl";

export type WidgetType = OverviewWidgetType | MainWidgetType;
export type WidgetZone = "overview" | "main";

export interface DashboardWidget {
  id: string;
  type: WidgetType;
  zone: WidgetZone;
}

export interface DashboardLayout {
  overview: DashboardWidget[];
  main: DashboardWidget[];
}

export interface WidgetCatalogItem {
  type: WidgetType;
  zone: WidgetZone;
  label: string;
  description: string;
}

// ─── API request params ───────────────────────────────────────────────────────

export interface DashboardMetricParams {
  metricType: WidgetType;
}

// ─── Response DTO — discriminated union by renderType ─────────────────────────

export type MetricTone = "default" | "success" | "danger" | "warning" | "muted";

export type WidgetStat = { label: string; value: string; tone?: MetricTone };

export type Point = { x: string; y: number };

export type MetricPayload = {
  displayValue: string;
  description: string;
  tone?: MetricTone;
  points?: Point[];
};

export type GaugePayload = {
  displayValue: string;
  description: string;
  /** 0–100 */
  percent: number;
  stats: WidgetStat[];
};

export type RingPayload = {
  displayValue: string;
  description: string;
  numerator: number;
  denominator: number;
  stats?: WidgetStat[];
};

export type RangePayload = {
  displayValue: string;
  description: string;
  left: WidgetStat;
  right: WidgetStat;
  /** 0–1 fraction for the positive (left) side */
  ratio: number;
};

export type AreaChartPayload = {
  description: string;
  points: { x: string; y: number }[];
};

export type BarChartPayload = {
  description: string;
  points: Point[];

  /** Color bars green/red based on sign */
  colorByValue?: boolean;
};

export type DistributionPayload = {
  description: string;
  segments: { name: string; value: number }[];
};

export type WidgetDto =
  | { renderType: "metric"; widgetType: string; payload: MetricPayload }
  | { renderType: "gauge"; widgetType: string; payload: GaugePayload }
  | { renderType: "ring"; widgetType: string; payload: RingPayload }
  | { renderType: "range"; widgetType: string; payload: RangePayload }
  | { renderType: "area-chart"; widgetType: string; payload: AreaChartPayload }
  | { renderType: "bar-chart"; widgetType: string; payload: BarChartPayload }
  | {
      renderType: "distribution";
      widgetType: string;
      payload: DistributionPayload;
    };

export interface UseDashboardWidgetResult {
  data: WidgetDto | null;
  isLoading: boolean;
  error: ApiError[] | null;
  refetch: () => void;
}

export interface UseDashboardWidgetOptions {
  from?: string;
  to?: string;
  period?: "day" | "week" | "month";
}
