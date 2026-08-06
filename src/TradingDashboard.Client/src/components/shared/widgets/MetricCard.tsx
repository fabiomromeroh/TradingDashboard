import type { MetricWidgetBaseProps } from "./widget-types";
import { MetricWidgetShell } from "./MetricWidgetShell";

export function MetricCard(props: MetricWidgetBaseProps) {
  return <MetricWidgetShell {...props} />;
}
