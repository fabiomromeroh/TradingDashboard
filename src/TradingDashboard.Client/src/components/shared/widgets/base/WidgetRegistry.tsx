import type { ReactNode } from "react";
import { MetricCard } from "@/components/shared/widgets/MetricCard";
import { GaugeMetricCard } from "@/components/shared/widgets/GaugeMetricCard";
import { RingMetricCard } from "@/components/shared/widgets/RingMetricCard";
import { RangeMetricCard } from "@/components/shared/widgets/RangeMetricCard";
import { AreaChartWidget } from "@/components/shared/widgets/AreaChartWidget";
import { BarChartWidget } from "@/components/shared/widgets/BarChartWidget";

import { toneClass } from "./widget-utils";
import type {
  MainWidgetType,
  WidgetType,
  WidgetCatalogItem,
  WidgetDto,
  WidgetZone,
} from "@/features/dashboard/types/dashboard.types";
import { AreaChartCurve } from "../AreaChartCurve";

// ─── Column span per main widget ─────────────────────────────────────────────
// Set colSpan: 2 to span both columns (full width); 1 for half-width.
// Set chartHeight to control the chart's pixel height inside the card.

export const MAIN_WIDGET_SIZES: Record<
  MainWidgetType,
  { colSpan: 1 | 2; chartHeight: number }
> = {
  "net-pnl-curve": { colSpan: 2, chartHeight: 260 },
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

const fmtUsd = (v: number) => `$${(v / 1000).toFixed(0)}`;
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
    type: "net-pnl-curve",
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

export function getWidgetZone(type: WidgetType): WidgetZone {
  return WIDGET_CATALOG.find((w) => w.type === type)?.zone ?? "main";
}

// ─── DTO → component renderer ─────────────────────────────────────────────────

/** Renders a widget from a live API WidgetDto. */
export function renderWidgetFromDto(dto: WidgetDto): ReactNode {
  const label =
    WIDGET_CATALOG.find((w) => w.type === dto.widgetType)?.label ??
    dto.widgetType;

  switch (dto.renderType) {
    case "metric":
      return (
        <MetricCard
          title={label}
          value={`${dto.payload.displayValue}`}
          valueClassName={toneClass(dto.payload.tone)}
          info={dto.payload.description}
          rightSlot={
            dto.payload.points && (
              <AreaChartCurve
                data={
                  dto.payload.points as unknown as Record<string, unknown>[]
                }
                dataKey="y"
                xAxisKey="x"
                config={{
                  y: { label, color: "var(--chart-2)" },
                }}
                chartHeight={86}
                yTickFormatter={fmtUsd}
                tooltipValueFormatter={(v) => `${v.toLocaleString("en-US")}`}
                color="var(--chart-2)"
              />
            )
          }
        />
      );

    case "gauge":
      return (
        <GaugeMetricCard
          title={label}
          value={dto.payload.displayValue}
          valueNumber={dto.payload.percent}
          footerStats={dto.payload.stats}
          info={dto.payload.description}
        />
      );

    case "ring": {
      const pct =
        dto.payload.denominator > 0
          ? (dto.payload.numerator / dto.payload.denominator) * 100
          : 0;
      return (
        <RingMetricCard
          title={label}
          value={dto.payload.displayValue}
          valueNumber={pct}
          total={100}
          footerStats={dto.payload.stats}
          info={dto.payload.description}
        />
      );
    }

    case "range":
      return (
        <RangeMetricCard
          title={label}
          value={dto.payload.displayValue}
          leftLabel={dto.payload.left.label}
          leftValue={dto.payload.left.value}
          rightLabel={dto.payload.right.label}
          rightValue={dto.payload.right.value}
          ratio={dto.payload.ratio}
          info={dto.payload.description}
        />
      );

    case "area-chart":
      return (
        <AreaChartWidget
          title={label}
          data={dto.payload.points as unknown as Record<string, unknown>[]}
          dataKey="y"
          xAxisKey="x"
          config={{ y: { label, color: "var(--chart-2)" } }}
          chartHeight={
            (dto.widgetType as MainWidgetType) in MAIN_WIDGET_SIZES
              ? MAIN_WIDGET_SIZES[dto.widgetType as MainWidgetType].chartHeight
              : 220
          }
          showGradient
          showZeroLine
          yTickFormatter={fmtUsd}
          tooltipValueFormatter={(v) => `$${v.toLocaleString("en-US")}`}
          info={dto.payload.description}
          color="var(--chart-1)"
          splitAtZero
        />
      );

    case "bar-chart":
      return (
        <BarChartWidget
          title={label}
          data={dto.payload.points as unknown as Record<string, unknown>[]}
          dataKey="y"
          xAxisKey="x"
          config={{ y: { label } }}
          chartHeight={
            (dto.widgetType as MainWidgetType) in MAIN_WIDGET_SIZES
              ? MAIN_WIDGET_SIZES[dto.widgetType as MainWidgetType].chartHeight
              : 220
          }
          colorByValue
          yTickFormatter={fmtUsd}
          tooltipValueFormatter={fmtUsdFull}
          info={dto.payload.description}
        />
      );
    // case "distribution": {
    //   const total = dto.payload.segments.reduce((s, g) => s + g.value, 0);
    //   return (
    //     <DonutChartWidget
    //       title={label}
    //       data={dto.payload.segments.map((seg, i) => ({
    //         name: seg.name,
    //         value: seg.value,
    //         fill: SEGMENT_COLORS[i % SEGMENT_COLORS.length],
    //       }))}
    //       chartHeight={
    //         (dto.widgetType as MainWidgetType) in MAIN_WIDGET_SIZES
    //           ? MAIN_WIDGET_SIZES[dto.widgetType as MainWidgetType].chartHeight
    //           : 220
    //       }
    //       innerRadius={52}
    //       outerRadius={76}
    //       centerValue={total}
    //       centerLabel="trades"
    //       tooltipFormatter={(v, t) =>
    //         `${v} trades (${t > 0 ? ((v / t) * 100).toFixed(1) : 0}%)`
    //       }
    //     />
    //   );
    // }

    default:
      return null;
  }
}
