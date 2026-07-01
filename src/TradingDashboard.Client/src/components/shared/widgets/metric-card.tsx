import type { MetricWidgetBaseProps } from "./widget-types"
import { MetricWidgetShell } from "./metric-widget-shell"

export function MetricCard(props: MetricWidgetBaseProps) {
  return <MetricWidgetShell {...props} />
}
