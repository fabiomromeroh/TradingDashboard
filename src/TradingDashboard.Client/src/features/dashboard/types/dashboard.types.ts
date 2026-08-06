export type OverviewWidgetType =
  | "net-pnl"
  | "win-rate"
  | "profit-factor"
  | "day-win-rate"
  | "avg-win-loss"
  | "total-trades"
  | "max-drawdown";

export type MainWidgetType =
  | "equity-curve"
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
