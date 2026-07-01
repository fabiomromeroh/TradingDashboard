import type { ReactNode } from "react";

export type MetricTone = "default" | "success" | "danger" | "warning" | "muted";

export type WidgetBadge = {
  label: string;
  tone?: MetricTone;
};

export type WidgetStat = {
  label: string;
  value: string;
  tone?: MetricTone;
};

export type MetricWidgetBaseProps = {
  title: string;
  value: string;
  description?: string;
  info?: string;
  badge?: WidgetBadge;
  footerStats?: WidgetStat[];
  rightSlot?: ReactNode;
  className?: string;
  valueClassName?: string;
};

export type GaugeSegment = {
  value: number;
  color: string;
};

export type GaugeMetricWidgetProps = MetricWidgetBaseProps & {
  valueNumber: number;
  min?: number;
  max?: number;
  segments?: GaugeSegment[];
};

export type RingMetricWidgetProps = MetricWidgetBaseProps & {
  valueNumber: number;
  total?: number;
  color?: string;
  trackColor?: string;
};

export type RangeMetricWidgetProps = MetricWidgetBaseProps & {
  leftLabel: string;
  leftValue: string;
  rightLabel: string;
  rightValue: string;
  ratio: number;
};
