import { MetricWidgetShell } from "./metric-widget-shell";
import { clamp, cn, describeArc } from "./widget-utils";
import type { GaugeMetricWidgetProps } from "./widget-types";

function Gauge({
  valueNumber,
  min = 0,
  max = 100,
  segments,
}: Pick<GaugeMetricWidgetProps, "valueNumber" | "min" | "max" | "segments">) {
  const normalized = clamp((valueNumber - min) / (max - min || 1), 0, 1);
  const angle = 180 * normalized;
  const pointer = 180 - angle;
  const defaultSegments = segments?.length
    ? segments
    : [
        { value: 0.7, color: "var(--color-emerald-500, #10b981)" },
        { value: 0.2, color: "var(--color-amber-400, #f59e0b)" },
        { value: 0.1, color: "var(--color-rose-400, #fb7185)" },
      ];

  const arcSegments = defaultSegments.map((segment, index, arr) => {
    const previousSweep = arr
      .slice(0, index)
      .reduce((sum, item) => sum + item.value * 180, 0);

    const startAngle = 180 - previousSweep;
    const endAngle = startAngle - segment.value * 180;

    return {
      ...segment,
      startAngle,
      endAngle,
      key: `${segment.color}-${index}`,
    };
  });

  return (
    <svg viewBox="0 0 140 88" className="h-[72px] w-[120px] overflow-visible">
      <path
        d={describeArc(70, 70, 48, 0, 180)}
        fill="none"
        stroke="hsl(var(--muted))"
        strokeOpacity="0.25"
        strokeWidth="10"
        strokeLinecap="round"
      />

      {arcSegments.map((segment) => (
        <path
          key={segment.key}
          d={describeArc(70, 70, 48, segment.endAngle, segment.startAngle)}
          fill="none"
          stroke={segment.color}
          strokeWidth="10"
          strokeLinecap="round"
        />
      ))}

      <line
        x1="70"
        y1="70"
        x2={70 + 34 * Math.cos(((pointer - 90) * Math.PI) / 180)}
        y2={70 + 34 * Math.sin(((pointer - 90) * Math.PI) / 180)}
        stroke="currentColor"
        strokeWidth="3"
        strokeLinecap="round"
        className="text-foreground"
      />
      <circle cx="70" cy="70" r="4.5" className="fill-foreground" />
    </svg>
  );
}

export function GaugeMetricCard({
  valueNumber,
  min = 0,
  max = 100,
  segments,
  footerStats,
  ...props
}: GaugeMetricWidgetProps) {
  return (
    <MetricWidgetShell
      {...props}
      footerStats={footerStats}
      rightSlot={
        <div className={cn("flex flex-col items-end gap-2 pt-1")}>
          <Gauge
            valueNumber={valueNumber}
            min={min}
            max={max}
            segments={segments}
          />
        </div>
      }
    />
  );
}
