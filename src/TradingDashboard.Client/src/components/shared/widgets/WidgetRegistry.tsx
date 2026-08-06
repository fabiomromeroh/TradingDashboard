import type { ReactNode } from "react";
import { MetricCard } from "@/components/shared/widgets/MetricCard";
import { GaugeMetricCard } from "@/components/shared/widgets/GaugeMetricCard";
import { RingMetricCard } from "@/components/shared/widgets/RingMetricCard";
import { RangeMetricCard } from "@/components/shared/widgets/RangeMetricCard";
import { AreaChartWidget } from "@/components/shared/widgets/AreaChartWidget";
import { BarChartWidget } from "@/components/shared/widgets/BarChartWidget";
import { DonutChartWidget } from "@/components/shared/widgets/DonutChartWidget";
import type {
  MainWidgetType,
  WidgetCatalogItem,
  WidgetType,
  WidgetZone,
} from "../../../features/dashboard/types/dashboard.types";

// ─── Column span per main widget ─────────────────────────────────────────────
// Set colSpan: 2 to span both columns (full width); 1 for half-width.
// Set chartHeight to control the chart's pixel height inside the card.

export const MAIN_WIDGET_SIZES: Record<
  MainWidgetType,
  { colSpan: 1 | 2; chartHeight: number }
> = {
  "equity-curve": { colSpan: 2, chartHeight: 260 },
  "daily-pnl-bar": { colSpan: 2, chartHeight: 260 },
  "win-loss-donut": { colSpan: 1, chartHeight: 260 },
  "monthly-pnl": { colSpan: 1, chartHeight: 260 },
};

export function getWidgetColSpan(type: WidgetType): 1 | 2 {
  if (type in MAIN_WIDGET_SIZES) {
    return MAIN_WIDGET_SIZES[type as MainWidgetType].colSpan;
  }
  return 1;
}

// ─── Sample data (replace with API hooks when ready) ─────────────────────────

const equityCurveData = [
  { date: "Jan 2", equity: 1200 },
  { date: "Jan 3", equity: 3400 },
  { date: "Jan 6", equity: 5100 },
  { date: "Jan 7", equity: 4300 },
  { date: "Jan 8", equity: 7900 },
  { date: "Jan 9", equity: 10200 },
  { date: "Jan 10", equity: 8700 },
  { date: "Jan 13", equity: 12500 },
  { date: "Jan 14", equity: 15800 },
  { date: "Jan 15", equity: 14100 },
  { date: "Jan 16", equity: 18300 },
  { date: "Jan 17", equity: 22000 },
  { date: "Jan 20", equity: 19500 },
  { date: "Jan 21", equity: 25400 },
  { date: "Jan 22", equity: 30100 },
  { date: "Jan 23", equity: 28800 },
  { date: "Jan 24", equity: 35600 },
  { date: "Jan 27", equity: 41200 },
  { date: "Jan 28", equity: 38900 },
  { date: "Jan 29", equity: 47300 },
  { date: "Jan 30", equity: 55700 },
  { date: "Jan 31", equity: 52100 },
  { date: "Feb 3", equity: 61400 },
  { date: "Feb 4", equity: 69800 },
  { date: "Feb 5", equity: 67200 },
  { date: "Feb 6", equity: 76500 },
  { date: "Feb 7", equity: 83900 },
  { date: "Feb 10", equity: 80400 },
  { date: "Feb 11", equity: 88700 },
  { date: "Feb 12", equity: 93171 },
];

const dailyPnLData = [
  { date: "Jan 2", pnl: 1200 },
  { date: "Jan 3", pnl: 2200 },
  { date: "Jan 6", pnl: 1700 },
  { date: "Jan 7", pnl: -900 },
  { date: "Jan 8", pnl: 3600 },
  { date: "Jan 9", pnl: 2300 },
  { date: "Jan 10", pnl: -1500 },
  { date: "Jan 13", pnl: 3800 },
  { date: "Jan 14", pnl: 3300 },
  { date: "Jan 15", pnl: -1700 },
  { date: "Jan 16", pnl: 4200 },
  { date: "Jan 17", pnl: 3700 },
  { date: "Jan 20", pnl: -2500 },
  { date: "Jan 21", pnl: 5900 },
  { date: "Jan 22", pnl: 4700 },
  { date: "Jan 23", pnl: -1300 },
  { date: "Jan 24", pnl: 6800 },
  { date: "Jan 27", pnl: 5600 },
  { date: "Jan 28", pnl: -2300 },
  { date: "Jan 29", pnl: 8400 },
  { date: "Jan 30", pnl: 8400 },
  { date: "Jan 31", pnl: -3600 },
  { date: "Feb 3", pnl: 9300 },
  { date: "Feb 4", pnl: 8400 },
  { date: "Feb 5", pnl: -2600 },
  { date: "Feb 6", pnl: 9300 },
  { date: "Feb 7", pnl: 7400 },
  { date: "Feb 10", pnl: -3500 },
  { date: "Feb 11", pnl: 8300 },
  { date: "Feb 12", pnl: 4471 },
];

const winLossData = [
  { name: "Wins", value: 25, fill: "#10b981" },
  { name: "Losses", value: 10, fill: "#f43f5e" },
  { name: "Breakeven", value: 0, fill: "#94a3b8" },
];

const monthlyPnLData = [
  { month: "Jan", pnl: 32400 },
  { month: "Feb", pnl: 28100 },
  { month: "Mar", pnl: -8500 },
  { month: "Apr", pnl: 14200 },
  { month: "May", pnl: 19700 },
  { month: "Jun", pnl: -4300 },
  { month: "Jul", pnl: 22800 },
  { month: "Aug", pnl: 31500 },
  { month: "Sep", pnl: -11200 },
  { month: "Oct", pnl: 26400 },
  { month: "Nov", pnl: 18900 },
  { month: "Dec", pnl: 9700 },
];

const fmtUsd = (v: number) => `$${(v / 1000).toFixed(0)}k`;
const fmtUsdFull = (v: number) => {
  const abs = `$${Math.abs(v).toLocaleString("en-US")}`;
  return v < 0 ? `-${abs}` : abs;
};

// ─── Catalog metadata ─────────────────────────────────────────────────────────

export const WIDGET_CATALOG: WidgetCatalogItem[] = [
  {
    type: "net-pnl",
    zone: "overview",
    label: "Net P&L",
    description: "Total net profit after losses and fees.",
  },
  {
    type: "win-rate",
    zone: "overview",
    label: "Win Rate",
    description: "Winning trades as a percentage of all closed trades.",
  },
  {
    type: "profit-factor",
    zone: "overview",
    label: "Profit Factor",
    description: "Gross profit divided by gross loss.",
  },
  {
    type: "day-win-rate",
    zone: "overview",
    label: "Day Win %",
    description: "Percentage of trading days ending in profit.",
  },
  {
    type: "avg-win-loss",
    zone: "overview",
    label: "Avg Win / Loss",
    description: "Average winning trade vs average losing trade.",
  },
  {
    type: "total-trades",
    zone: "overview",
    label: "Total Trades",
    description: "Total number of closed trades.",
  },
  {
    type: "max-drawdown",
    zone: "overview",
    label: "Max Drawdown",
    description: "Largest peak-to-trough decline in equity.",
  },
  {
    type: "equity-curve",
    zone: "main",
    label: "Equity Curve",
    description: "Cumulative P&L over time as an area chart.",
  },
  {
    type: "daily-pnl-bar",
    zone: "main",
    label: "Daily P&L",
    description: "Green/red bar chart of profit or loss per trading day.",
  },
  {
    type: "win-loss-donut",
    zone: "main",
    label: "Win / Loss Distribution",
    description: "Donut chart showing the ratio of wins to losses.",
  },
  {
    type: "monthly-pnl",
    zone: "main",
    label: "Monthly P&L",
    description: "Bar chart of monthly profit and loss for the year.",
  },
];

export const OVERVIEW_CATALOG = WIDGET_CATALOG.filter(
  (w) => w.zone === "overview",
);
export const MAIN_CATALOG = WIDGET_CATALOG.filter((w) => w.zone === "main");

// ─── Widget renderer ──────────────────────────────────────────────────────────

/** Returns the rendered JSX for a widget type. Sample data is used until API hooks are wired in. */
export function renderWidget(type: WidgetType): ReactNode {
  switch (type) {
    case "net-pnl":
      return (
        <MetricCard
          title="Net P&L"
          value="$93,171.27"
          description="Across 35 closed trades"
          info="Net profit after subtracting all realized losses and fees."
          valueClassName="text-emerald-600 dark:text-emerald-400"
        />
      );
    case "win-rate":
      return (
        <GaugeMetricCard
          title="Trade Win %"
          value="71.43%"
          valueNumber={71.43}
          info="Winning trades divided by total closed trades."
          footerStats={[
            { label: "Wins", value: "25", tone: "success" },
            { label: "BE", value: "0", tone: "muted" },
            { label: "Losses", value: "10", tone: "danger" },
          ]}
        />
      );
    case "profit-factor":
      return (
        <RingMetricCard
          title="Profit Factor"
          value="10.86"
          valueNumber={86}
          total={100}
          color="var(--chart-2)"
          trackColor="var(--chart-1)"
          info="Gross profit divided by gross loss."
          footerStats={[{ label: "Healthy", value: "> 2.0", tone: "success" }]}
        />
      );
    case "day-win-rate":
      return (
        <GaugeMetricCard
          title="Day Win %"
          value="91.67%"
          valueNumber={91.67}
          info="Winning trading days divided by all active trading days."
          footerStats={[
            { label: "Green", value: "11", tone: "success" },
            { label: "Flat", value: "0", tone: "muted" },
            { label: "Red", value: "1", tone: "danger" },
          ]}
        />
      );
    case "avg-win-loss":
      return (
        <RangeMetricCard
          title="Avg Win / Loss"
          value="4.34"
          leftLabel="Avg win"
          leftValue="$4.1K"
          rightLabel="Avg loss"
          rightValue="-$945"
          ratio={0.82}
        />
      );
    case "total-trades":
      return (
        <MetricCard
          title="Total Trades"
          value="35"
          description="Closed positions"
          info="Total number of fully closed trades."
        />
      );
    case "max-drawdown":
      return (
        <MetricCard
          title="Max Drawdown"
          value="-$8,720"
          description="Peak to trough"
          info="Largest peak-to-trough decline in account equity."
          valueClassName="text-rose-600 dark:text-rose-400"
        />
      );
    case "equity-curve":
      return (
        <AreaChartWidget
          title="Equity Curve"
          info="Cumulative P&L plotted over each trading day."
          data={equityCurveData}
          dataKey="equity"
          config={{ equity: { label: "Equity", color: "var(--chart-2)" } }}
          chartHeight={MAIN_WIDGET_SIZES["equity-curve"].chartHeight}
          showGradient
          showZeroLine
          yTickFormatter={fmtUsd}
          tooltipValueFormatter={(v) => `$${v.toLocaleString("en-US")}`}
        />
      );
    case "daily-pnl-bar":
      return (
        <BarChartWidget
          title="Daily P&L"
          info="Profit or loss for each trading day."
          data={dailyPnLData}
          dataKey="pnl"
          config={{ pnl: { label: "Daily P&L" } }}
          chartHeight={MAIN_WIDGET_SIZES["daily-pnl-bar"].chartHeight}
          colorByValue
          yTickFormatter={fmtUsd}
          tooltipValueFormatter={fmtUsdFull}
        />
      );
    case "win-loss-donut": {
      const total = winLossData.reduce((a, d) => a + d.value, 0);
      return (
        <DonutChartWidget
          title="Win / Loss Distribution"
          info="Breakdown of winning, losing, and breakeven trades."
          data={winLossData}
          chartHeight={MAIN_WIDGET_SIZES["win-loss-donut"].chartHeight}
          innerRadius={52}
          outerRadius={76}
          centerValue={total}
          centerLabel="trades"
          tooltipFormatter={(v, t) =>
            `${v} trades (${t > 0 ? ((v / t) * 100).toFixed(1) : 0}%)`
          }
        />
      );
    }
    case "monthly-pnl":
      return (
        <BarChartWidget
          title="Monthly P&L"
          info="Total profit or loss aggregated by calendar month."
          data={monthlyPnLData}
          dataKey="pnl"
          xAxisKey="month"
          config={{ pnl: { label: "Monthly P&L" } }}
          chartHeight={MAIN_WIDGET_SIZES["monthly-pnl"].chartHeight}
          colorByValue
          yTickFormatter={fmtUsd}
          tooltipValueFormatter={fmtUsdFull}
        />
      );
    default:
      return null;
  }
}

export function getWidgetZone(type: WidgetType): WidgetZone {
  return WIDGET_CATALOG.find((w) => w.type === type)?.zone ?? "main";
}
