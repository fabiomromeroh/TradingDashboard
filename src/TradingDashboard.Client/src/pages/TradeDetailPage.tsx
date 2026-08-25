import { useParams, useNavigate } from "react-router-dom";
import { Card, CardContent } from "@/components/ui/card";
import { AppButton } from "@/components/shared/AppButton";
import { TradeDetail } from "@/features/trades/components/TradeDetail";
import { useTradesQuery } from "@/features/trades/hooks/useTradesQuery";
import { Spinner } from "@/components/ui/spinner";
import { ArrowLeft } from "lucide-react";

export default function TradeDetailPage() {
  const { tradeId } = useParams<{ tradeId: string }>();
  const navigate = useNavigate();
  const { trades, isLoading } = useTradesQuery();

  if (isLoading) {
    return (
      <div className="flex h-full items-center justify-center">
        <Spinner />
      </div>
    );
  }

  const trade = trades.find((t) => t.id === tradeId);

  if (!trade) {
    return (
      <div className="flex h-full flex-col items-center justify-center gap-4">
        <p className="text-lg text-muted-foreground">Trade not found</p>
        <AppButton onClick={() => navigate("/trades")}>
          Back to Trades
        </AppButton>
      </div>
    );
  }

  return (
    <div className="flex h-full min-h-0 flex-col gap-4 overflow-hidden">
      <div className="flex items-center gap-3">
        <AppButton
          variant="ghost"
          size="sm"
          onClick={() => navigate("/trades")}
          className="gap-2"
        >
          <ArrowLeft className="h-4 w-4" />
          Back to Trades
        </AppButton>
      </div>

      <Card className="flex min-h-0 flex-1 flex-col overflow-hidden">
        <CardContent className="flex min-h-0 flex-1 flex-col p-4 overflow-auto">
          <TradeDetail trade={trade} />
        </CardContent>
      </Card>
    </div>
  );
}
