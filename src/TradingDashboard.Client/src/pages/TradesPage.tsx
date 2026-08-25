import { Card, CardContent } from "@/components/ui/card";
import { TradeTable } from "../features/trades";

export default function TradesPage() {
  return (
    <div className="h-full w-full overflow-hidden bg-background p-4">
      <Card className="flex h-full flex-col overflow-hidden">
        <CardContent className="flex h-full flex-1 flex-col overflow-hidden p-4">
          <TradeTable />
        </CardContent>
      </Card>
    </div>
  );
}
