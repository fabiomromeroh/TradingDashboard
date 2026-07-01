import { GaugeMetricCard } from "@/components/shared/widgets/gauge-metric-card";
import { MetricCard } from "@/components/shared/widgets/metric-card";
import { RangeMetricCard } from "@/components/shared/widgets/range-metric-card";
import { RingMetricCard } from "@/components/shared/widgets/ring-metric-card";

export function DashboardPage() {
  return (
    <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-5">
      <MetricCard
        title="Net P&L"
        value="$93,171.27"
        description="Across 35 closed trades"
        info="Net profit after subtracting all realized losses and fees."
        valueClassName="text-emerald-600 dark:text-emerald-400"
      />

      <GaugeMetricCard
        title="Trade Win %"
        value="71.43%"
        valueNumber={71.43}
        info="Winning trades divided by total closed trades."
        footerStats={[
          { label: "Wins", value: "25", tone: "success" },
          { label: "BE", value: "0", tone: "muted" },
          { label: "Losses", value: "10", tone: "danger" },
        ]}
      />

      <RingMetricCard
        title="Profit Factor"
        value="10.86"
        valueNumber={86}
        total={100}
        color="var(--chart-2)"
        trackColor="var(--chart-1)"
        info="Gross profit divided by gross loss."
        footerStats={[{ label: "Healthy", value: "> 2.0", tone: "success" }]}
      />

      <GaugeMetricCard
        title="Day Win %"
        value="91.67%"
        valueNumber={91.67}
        info="Winning trading days divided by all active trading days."
        footerStats={[
          { label: "Green", value: "11", tone: "success" },
          { label: "Flat", value: "0", tone: "muted" },
          { label: "Red", value: "1", tone: "danger" },
        ]}
      />

      <RangeMetricCard
        title="Avg win/loss trade"
        value="4.34"
        // description="Average winner divided by average loser"
        leftLabel="Avg win"
        leftValue="$4.1K"
        rightLabel="Avg loss"
        rightValue="-$945"
        ratio={0.82}
      />
    </div>
  );
}
