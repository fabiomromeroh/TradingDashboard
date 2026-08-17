import { Info } from "lucide-react";
import {
  Bar,
  BarChart,
  CartesianGrid,
  Cell,
  ReferenceLine,
  XAxis,
  YAxis,
} from "recharts";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "@/components/ui/tooltip";
import { Button } from "@/components/ui/button";
import {
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
  type ChartConfig,
} from "@/components/ui/chart";
import { cn } from "@/lib/utils";

export interface BarChartWidgetProps {
  title: string;
  description?: string;
  info?: string;
  data: Record<string, unknown>[];
  dataKey: string;
  xAxisKey?: string;
  config: ChartConfig;
  /** Height in pixels for the chart area. Default: 220 */
  chartHeight?: number;
  className?: string;
  /** Color each bar green/red based on whether its value is positive or negative */
  colorByValue?: boolean;
  positiveColor?: string;
  negativeColor?: string;
  yTickFormatter?: (value: number) => string;
  tooltipValueFormatter?: (value: number) => string;
}

export function BarChartWidget({
  title,
  description,
  info,
  data,
  dataKey,
  xAxisKey = "date",
  config,
  chartHeight = 220,
  className,
  colorByValue = false,
  positiveColor = "#10b981",
  negativeColor = "#f43f5e",
  yTickFormatter = String,
  tooltipValueFormatter,
}: BarChartWidgetProps) {
  const defaultColor =
    (Object.values(config)[0] as { color?: string } | undefined)?.color ??
    "var(--chart-1)";

  return (
    <Card className={cn("border-border/60 shadow-sm", className)}>
      <CardHeader className="pb-2">
        <div className="flex items-center gap-1.5">
          <CardTitle className="text-sm font-medium text-muted-foreground">
            {title}
          </CardTitle>
          {info && (
            <TooltipProvider delayDuration={150}>
              <Tooltip>
                <TooltipTrigger asChild>
                  <Button
                    variant="ghost"
                    size="icon"
                    className="size-5 shrink-0 text-muted-foreground/70 hover:text-foreground"
                    aria-label={`More info about ${title}`}
                  >
                    <Info className="size-3.5" />
                  </Button>
                </TooltipTrigger>
                <TooltipContent side="top" className="max-w-64 text-xs">
                  {info}
                </TooltipContent>
              </Tooltip>
            </TooltipProvider>
          )}
        </div>
        {description && (
          <p className="text-xs text-muted-foreground">{description}</p>
        )}
      </CardHeader>

      <CardContent className="pt-0">
        <ChartContainer
          config={config}
          style={{ height: chartHeight }}
          className="w-full"
        >
          <BarChart
            data={data}
            margin={{ top: 4, right: 8, bottom: 0, left: 0 }}
          >
            <CartesianGrid
              strokeDasharray="3 3"
              stroke="hsl(var(--border))"
              strokeOpacity={0.5}
              vertical={false}
            />
            <XAxis
              dataKey={xAxisKey}
              tick={{ fontSize: 8 }}
              tickLine={false}
              axisLine={false}
              angle={45}
              interval={"preserveEnd"}
              tickCount={2}
            />
            <YAxis
              tick={{ fontSize: 10 }}
              tickLine={false}
              axisLine={false}
              tickFormatter={yTickFormatter}
              width={44}
              padding={{ bottom: 20 }}
            />
            {colorByValue && (
              <ReferenceLine
                y={0}
                stroke="hsl(var(--border))"
                strokeWidth={1.5}
              />
            )}
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
            <Bar dataKey={dataKey} radius={[3, 3, 0, 0]}>
              {data.map((entry, idx) => (
                <Cell
                  key={`cell-${idx}`}
                  fill={
                    colorByValue
                      ? Number(entry[dataKey]) >= 0
                        ? positiveColor
                        : negativeColor
                      : defaultColor
                  }
                  fillOpacity={0.85}
                />
              ))}
            </Bar>
          </BarChart>
        </ChartContainer>
      </CardContent>
    </Card>
  );
}
