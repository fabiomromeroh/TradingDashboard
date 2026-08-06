import { Info } from "lucide-react";
import { Cell, Legend, Pie, PieChart } from "recharts";
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
} from "@/components/ui/chart";
import { cn } from "@/lib/utils";

export interface DonutSegment {
  name: string;
  value: number;
  fill: string;
}

export interface DonutChartWidgetProps {
  title: string;
  description?: string;
  info?: string;
  data: DonutSegment[];
  /** Height in pixels for the chart area. Default: 220 */
  chartHeight?: number;
  innerRadius?: number;
  outerRadius?: number;
  /** Value displayed in the donut centre */
  centerValue?: string | number;
  /** Label below the centre value */
  centerLabel?: string;
  showLegend?: boolean;
  className?: string;
  tooltipFormatter?: (value: number, total: number) => string;
}

export function DonutChartWidget({
  title,
  description,
  info,
  data,
  chartHeight = 220,
  innerRadius = 60,
  outerRadius = 90,
  centerValue,
  centerLabel,
  showLegend = true,
  className,
  tooltipFormatter,
}: DonutChartWidgetProps) {
  const activeData = data.filter((d) => d.value > 0);
  const total = data.reduce((acc, d) => acc + d.value, 0);

  // Build ChartConfig from segment colours so ChartContainer is satisfied
  const config = Object.fromEntries(
    data.map((d) => [
      d.name.toLowerCase().replace(/[\s/]+/g, "-"),
      { label: d.name, color: d.fill },
    ]),
  );

  const defaultTooltipFormatter = (value: number) =>
    total > 0
      ? `${value} (${((value / total) * 100).toFixed(1)}%)`
      : String(value);

  // Shift centre text up a bit when the legend is shown
  const centreY = showLegend ? "42%" : "46%";
  const labelY = showLegend ? "53%" : "57%";

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
          <PieChart>
            <Pie
              data={activeData}
              dataKey="value"
              nameKey="name"
              cx="50%"
              cy="50%"
              innerRadius={innerRadius}
              outerRadius={outerRadius}
              paddingAngle={3}
              strokeWidth={0}
            >
              {activeData.map((entry) => (
                <Cell key={entry.name} fill={entry.fill} />
              ))}
            </Pie>
            <ChartTooltip
              content={
                <ChartTooltipContent
                  formatter={(value) =>
                    tooltipFormatter
                      ? tooltipFormatter(Number(value), total)
                      : defaultTooltipFormatter(Number(value))
                  }
                />
              }
            />
            {showLegend && (
              <Legend
                iconType="circle"
                iconSize={8}
                formatter={(value) => (
                  <span className="text-xs text-muted-foreground">{value}</span>
                )}
              />
            )}
            {centerValue !== undefined && (
              <text
                x="50%"
                y={centreY}
                textAnchor="middle"
                dominantBaseline="middle"
                fontSize={22}
                fontWeight={600}
              >
                {centerValue}
              </text>
            )}
            {centerLabel && (
              <text
                x="50%"
                y={labelY}
                textAnchor="middle"
                dominantBaseline="middle"
                fontSize={11}
              >
                {centerLabel}
              </text>
            )}
          </PieChart>
        </ChartContainer>
      </CardContent>
    </Card>
  );
}
