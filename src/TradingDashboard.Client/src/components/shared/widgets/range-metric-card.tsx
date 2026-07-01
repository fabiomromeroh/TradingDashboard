import { MetricWidgetShell } from "./metric-widget-shell";
import { clamp } from "./widget-utils";
import type { RangeMetricWidgetProps } from "./widget-types";

export function RangeMetricCard({
  leftLabel,
  leftValue,
  rightLabel,
  rightValue,
  ratio,
  ...props
}: RangeMetricWidgetProps) {
  const safeRatio = clamp(ratio, 0, 1);

  return (
    <MetricWidgetShell
      {...props}
      rightSlot={
        <div className="flex min-w-[132px] flex-col gap-2 pt-2">
          <div className="h-2.5 overflow-hidden rounded-full bg-muted">
            <div className="flex h-full w-full">
              <div
                className="bg-emerald-500"
                style={{ width: `${safeRatio * 100}%` }}
              />
              <div
                className="bg-rose-400/90"
                style={{ width: `${(1 - safeRatio) * 100}%` }}
              />
            </div>
          </div>
          <div className="flex items-center justify-between gap-4 text-[11px] font-medium tabular-nums">
            <div className="text-emerald-600 dark:text-emerald-400">
              <span className="mr-1 text-muted-foreground">{leftLabel}</span>
              {leftValue}
            </div>
            <div className="text-rose-600 dark:text-rose-400">
              <span className="mr-1 text-muted-foreground">{rightLabel}</span>
              {rightValue}
            </div>
          </div>
        </div>
      }
    />
  );
}
