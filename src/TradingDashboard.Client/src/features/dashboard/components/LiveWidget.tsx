import { RefreshCw } from "lucide-react";
import { Button } from "@/components/ui/button";
import LoadingSpinner from "@/components/shared/LoadingSpinner";
import { renderWidgetFromDto } from "@/components/shared/widgets/base/WidgetRegistry";
import { useDashboardWidget } from "../hooks/useDashboardWidget";
import type { DashboardWidget } from "../types/dashboard.types";

interface LiveWidgetProps {
  widget: DashboardWidget;
}

export function LiveWidget({ widget }: LiveWidgetProps) {
  const { data, isLoading, error, refetch } = useDashboardWidget(widget.type);

  if (isLoading) {
    return (
      <div className="flex h-full min-h-[120px] w-full items-center justify-center rounded-lg border border-dashed border-border bg-card">
        <LoadingSpinner />
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex h-full min-h-[120px] w-full flex-col items-center justify-center gap-2 rounded-lg border border-dashed border-destructive/40 bg-card text-center text-sm text-muted-foreground">
        <span>Failed to load widget</span>
        <Button variant="ghost" size="sm" onClick={refetch} className="gap-1.5">
          <RefreshCw className="h-3.5 w-3.5" />
          Retry
        </Button>
      </div>
    );
  }

  if (!data) {
    // No accounts selected or data unavailable — show sample
    return <></>;
  }

  return <>{renderWidgetFromDto(data)}</>;
}
