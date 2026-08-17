import {
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import { AreaChart, Area, XAxis, YAxis, CartesianGrid } from "recharts";

import type { ChartConfig } from "@/components/ui/chart";

interface AreaChartCurveProps {
  config: ChartConfig;
  chartHeight?: number;
  data: Record<string, unknown>[];
  dataKey: string;
  xAxisKey?: string;
  color?: string;
  yTickFormatter?: (value: number) => string;
  tooltipValueFormatter?: (value: number) => string;
}

export function AreaChartCurve({
  config,
  chartHeight = 100,
  data,
  dataKey,
  xAxisKey,
  yTickFormatter,

  color = "var(--chart-2)",
  tooltipValueFormatter,
}: AreaChartCurveProps) {
  return (
    <ChartContainer
      config={config}
      style={{ height: chartHeight }}
      className="w-full"
    >
      <AreaChart data={data} margin={{ top: 4, right: 8, bottom: 0, left: 0 }}>
        <CartesianGrid
          strokeDasharray="3 3"
          stroke="hsl(var(--border))"
          strokeOpacity={0.5}
        />
        <XAxis
          dataKey={xAxisKey}
          tick={{ fontSize: 10 }}
          tickLine={false}
          axisLine={false}
          interval="preserveStartEnd"
          hide={true}
        />
        <YAxis
          tick={{ fontSize: 10 }}
          tickLine={false}
          axisLine={false}
          tickFormatter={yTickFormatter ?? ((value) => value.toString())}
          width={44}
          hide={true}
        />

        <ChartTooltip
          content={
            <ChartTooltipContent
              formatter={
                tooltipValueFormatter
                  ? (value) => tooltipValueFormatter(Number(value))
                  : undefined
              }
            />
          }
        />
        <Area
          type="monotone"
          dataKey={dataKey}
          stroke={color}
          strokeWidth={2}
          fill={"none"}
          dot={false}
          activeDot={{ r: 4 }}
        />
      </AreaChart>
    </ChartContainer>
  );
}
