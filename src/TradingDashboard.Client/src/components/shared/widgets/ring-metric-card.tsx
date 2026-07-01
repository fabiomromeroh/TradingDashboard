import { Pie, PieChart } from "recharts"

import { ChartContainer } from "@/components/ui/chart"

import { MetricWidgetShell } from "./metric-widget-shell"
import { clamp } from "./widget-utils"
import type { RingMetricWidgetProps } from "./widget-types"

export function RingMetricCard({
  valueNumber,
  total = 100,
  color = "var(--chart-1)",
  trackColor = "hsl(var(--muted))",
  ...props
}: RingMetricWidgetProps) {
  const safeValue = clamp(valueNumber, 0, total)
  const chartData = [
    { name: "value", value: safeValue, fill: color },
    { name: "rest", value: Math.max(total - safeValue, 0), fill: trackColor },
  ]

  return (
    <MetricWidgetShell
      {...props}
      rightSlot={
        <ChartContainer
          config={{
            value: { label: "Value", color },
            rest: { label: "Remaining", color: trackColor },
          }}
          className="h-[80px] w-[80px] min-h-[80px]"
        >
          <PieChart>
            <Pie
              data={chartData}
              dataKey="value"
              innerRadius={24}
              outerRadius={34}
              startAngle={90}
              endAngle={-270}
              stroke="none"
              paddingAngle={2}
            />
          </PieChart>
        </ChartContainer>
      }
    />
  )
}
