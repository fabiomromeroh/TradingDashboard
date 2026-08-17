import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";
import type { MetricTone } from "./widget-types";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function toneClass(tone: MetricTone = "default") {
  switch (tone) {
    case "success":
      return "text-emerald-600 dark:text-emerald-400";
    case "danger":
      return "text-rose-600 dark:text-rose-400";
    case "warning":
      return "text-amber-600 dark:text-amber-400";
    case "muted":
      return "text-muted-foreground";
    default:
      return "text-foreground";
  }
}

export function clamp(value: number, min: number, max: number) {
  return Math.min(Math.max(value, min), max);
}

export function polarToCartesian(
  cx: number,
  cy: number,
  radius: number,
  angle: number,
) {
  const rad = ((angle - 90) * Math.PI) / 180;
  return {
    x: cx + radius * Math.cos(rad),
    y: cy + radius * Math.sin(rad),
  };
}

export function describeArc(
  cx: number,
  cy: number,
  radius: number,
  startAngle: number,
  endAngle: number,
) {
  const start = polarToCartesian(cx, cy, radius, endAngle);
  const end = polarToCartesian(cx, cy, radius, startAngle);
  const largeArcFlag = endAngle - startAngle <= 180 ? "0" : "1";

  return [
    `M ${start.x} ${start.y}`,
    `A ${radius} ${radius} 0 ${largeArcFlag} 0 ${end.x} ${end.y}`,
  ].join(" ");
}

// Computes the % offset in the gradient where the value crosses zero
export function getZeroOffset(
  data: Record<string, unknown>[],
  dataKey: string,
) {
  const values = data.map((d) => Number(d[dataKey]) || 0);
  const max = Math.max(...values, 0);
  const min = Math.min(...values, 0);

  if (max <= 0) return 0; // all negative -> fully red
  if (min >= 0) return 1; // all positive -> fully green

  return max / (max - min);
}
