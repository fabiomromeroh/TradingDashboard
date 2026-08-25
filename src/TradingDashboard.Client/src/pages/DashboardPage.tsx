import { DashboardOverview } from "@/features/dashboard/components/DashboardOverview";

export function DashboardPage() {
  return (
    <div className="flex h-full flex-1 flex-col overflow-hidden">
      <DashboardOverview />
    </div>
  );
}
