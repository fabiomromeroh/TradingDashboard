import { Info } from "lucide-react";

import { Badge } from "@/components/ui/badge";
import { Card, CardContent, CardTitle } from "@/components/ui/card";
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "@/components/ui/tooltip";

import { cn, toneClass } from "../base/widget-utils";
import type { MetricWidgetBaseProps } from "./widget-types";

export function MetricWidgetShell({
  title,
  value,
  description,
  info,
  badge,
  // footerStats,
  rightSlot,
  className,
  valueClassName,
}: MetricWidgetBaseProps) {
  return (
    <Card
      className={cn(
        "border-border/60 bg-card shadow-sm h-[130px] pb-2 pt-3",
        className,
      )}
    >
      <CardContent className="flex flex-row items-start justify-between gap-3 pl-3 pr-3 h-full">
        <div className="space-y-1.5 ">
          <div className="flex items-center gap-1 pt-5">
            <CardTitle className="text-xs font-medium text-muted-foreground">
              {title}
            </CardTitle>
            {info ? (
              <TooltipProvider delayDuration={150}>
                <Tooltip>
                  <TooltipTrigger asChild>
                    <button
                      type="button"
                      className="text-muted-foreground/80 transition-colors hover:text-foreground"
                      aria-label={`More info about ${title}`}
                    >
                      <Info className="size-3.5" />
                    </button>
                  </TooltipTrigger>
                  <TooltipContent side="top" className="max-w-64 text-xs">
                    {info}
                  </TooltipContent>
                </Tooltip>
              </TooltipProvider>
            ) : null}
            {badge ? (
              <Badge
                variant="secondary"
                className={cn(
                  "ml-1 rounded-full px-2 py-0 text-[10px]",
                  toneClass(badge.tone),
                )}
              >
                {badge.label}
              </Badge>
            ) : null}
          </div>

          <div
            className={cn(
              "text-2xl font-semibold tracking-tight tabular-nums",
              valueClassName,
            )}
          >
            {value}
          </div>

          {description ? (
            <p className="text-xs text-muted-foreground">{description}</p>
          ) : null}
        </div>

        {rightSlot ? <div className="">{rightSlot}</div> : null}
      </CardContent>
      {/* 
      {footerStats?.length ? (
        <CardContent className="flex flex-wrap items-center gap-x-4">
          {footerStats.map((stat) => (
            <div
              key={`${stat.label}-${stat.value}`}
              className="flex items-center gap-1.5 text-xs tabular-nums"
            >
              <span className="text-muted-foreground">{stat.label}</span>
              <span className={cn("font-medium", toneClass(stat.tone))}>
                {stat.value}
              </span>
            </div>
          ))}
        </CardContent>
      ) : null} */}
    </Card>
  );
}
