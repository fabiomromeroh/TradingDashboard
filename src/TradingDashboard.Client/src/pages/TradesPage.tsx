import { Card, CardContent } from "@/components/ui/card";
import { TradeTable } from "../features/trades";

// The page's only job: arrange feature components on the screen.
// No hooks, no API calls, no state here.
export default function TradesPage() {
  return (
    <div className="flex h-full min-h-0 flex-col overflow-hidden">
      <Card className="flex min-h-0 flex-1 flex-col overflow-hidden">
        <CardContent className="flex min-h-0 flex-1 flex-col p-4">
          <TradeTable />
        </CardContent>
      </Card>
    </div>
  );
}
