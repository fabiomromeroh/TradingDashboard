import { Card, CardContent } from "@/components/ui/card";
import { TradeTable } from "../features/trades";

// The page's only job: arrange feature components on the screen.
// No hooks, no API calls, no state here.
export default function TradesPage() {
  return (
    <div>
      <Card>
        <CardContent>
          <TradeTable />
        </CardContent>
      </Card>
    </div>
  );
}
