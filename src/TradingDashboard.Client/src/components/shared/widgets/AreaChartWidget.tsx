import { Info } from "lucide-react";
import { getZeroOffset } from "@/components/shared/widgets/base/widget-utils";
import {
  Area,
  AreaChart,
  CartesianGrid,
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

export interface AreaChartWidgetProps {
  title?: string;
  description?: string;
  info?: string;
  data: Record<string, unknown>[];
  dataKey: string;
  xAxisKey?: string;
  config: ChartConfig;
  chartHeight?: number;
  className?: string;
  color?: string;
  showGradient?: boolean;
  showZeroLine?: boolean;
  yTickFormatter?: (value: number) => string;
  tooltipValueFormatter?: (value: number) => string;
  /** New: color the area red below 0 / green above 0 */
  splitAtZero?: boolean;
  positiveColor?: string;
  negativeColor?: string;
  hideXAxis?: boolean;
  hideYAxis?: boolean;
}

export function AreaChartWidget({
  title,
  description,
  info,
  data,
  dataKey,
  xAxisKey = "date",
  config,
  chartHeight = 220,
  className,
  color = "var(--chart-2)",
  showGradient = true,
  showZeroLine = false,
  yTickFormatter = String,
  tooltipValueFormatter,
  splitAtZero = false,
  positiveColor = "var(--primary)", // green-600
  negativeColor = "var(--destructive)", // red-600
  hideXAxis = false,
  hideYAxis = false,
}: AreaChartWidgetProps) {
  const gradientId = `area-grad-${dataKey}`;
  const strokeGradientId = `stroke-grad-${dataKey}`;
  const offset = splitAtZero ? getZeroOffset(data, dataKey) : 0;

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
          <AreaChart
            data={data}
            margin={{ top: 4, right: 8, bottom: 0, left: 0 }}
          >
            <defs>
              {splitAtZero ? (
                <>
                  {/* Fill gradient: green above zero, red below */}
                  <linearGradient id={gradientId} x1="0" y1="0" x2="0" y2="1">
                    <stop
                      offset={offset}
                      stopColor={positiveColor}
                      stopOpacity={0.3}
                    />
                    <stop
                      offset={offset}
                      stopColor={negativeColor}
                      stopOpacity={0.3}
                    />
                  </linearGradient>
                  {/* Stroke gradient: matches the line color to the fill split */}
                  <linearGradient
                    id={strokeGradientId}
                    x1="0"
                    y1="0"
                    x2="0"
                    y2="1"
                  >
                    <stop offset={offset} stopColor={positiveColor} />
                    <stop offset={offset} stopColor={negativeColor} />
                  </linearGradient>
                </>
              ) : (
                showGradient && (
                  <linearGradient id={gradientId} x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor={color} stopOpacity={0.25} />
                    <stop offset="95%" stopColor={color} stopOpacity={0} />
                  </linearGradient>
                )
              )}
            </defs>

            <CartesianGrid
              strokeDasharray="3 3"
              stroke="hsl(var(--border))"
              strokeOpacity={0.5}
            />
            <XAxis
              dataKey={xAxisKey}
              tick={{ fontSize: 8 }}
              tickLine={false}
              axisLine={false}
              interval={"preserveStartEnd"}
              hide={hideXAxis}
              angle={45}
            />
            <YAxis
              tick={{ fontSize: 10 }}
              tickLine={false}
              axisLine={false}
              tickFormatter={yTickFormatter}
              width={44}
              hide={hideYAxis}
              interval={"preserveStartEnd"}
              padding={{ bottom: 30 }}
            />
            {(showZeroLine || splitAtZero) && (
              <ReferenceLine y={0} stroke="hsl(var(--border))" />
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
            <Area
              type="monotone"
              dataKey={dataKey}
              stroke={splitAtZero ? `url(#${strokeGradientId})` : color}
              strokeWidth={2}
              fill={
                splitAtZero || showGradient ? `url(#${gradientId})` : "none"
              }
              dot={false}
              activeDot={{ r: 4 }}
            />
          </AreaChart>
        </ChartContainer>
      </CardContent>
    </Card>
  );
}
