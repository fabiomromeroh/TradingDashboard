import type { MetricWidgetBaseProps } from "./base/widget-types";
import { MetricWidgetShell } from "./base/MetricWidgetShell";

export function MetricCard(props: MetricWidgetBaseProps) {
  return <MetricWidgetShell {...props} />;
}
