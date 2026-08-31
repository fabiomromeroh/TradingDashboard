// components/TradeDetail.tsx
import { useMemo, useState } from "react";
import { Spinner } from "@/components/ui/spinner";
import { Checkbox } from "@/components/ui/checkbox";
import { Badge } from "@/components/ui/badge";
import { useTradeCandles } from "@/components/shared/charts/useTradeCandles";
import type { TradeDto } from "../types/trade.types";
import { CORE_TRADE_EVENT_TYPES } from "../types/trade.types";
import { TradeChart } from "@/components/shared/charts/TradeChart";
import { TradeStatusBadge } from "./TradeStatusBadge";
import { TradeEventTimeline } from "./TradeEventTimeline";
import { TradeThesisCard } from "./TradeThesisCard";
import { TradeCheckpointsCard } from "./TradeCheckpointsCard";
import { TradeOutcomeCard } from "./TradeOutcomeCard";
import { TradeAdditionalNotesCard } from "./TradeAdditionalNotesCard";
import { AddTradeEventDialog } from "./AddTradeEventDialog";
import { useTradeEvents } from "../hooks/useTradeEvents";
import { getDateFormat, cn } from "@/lib/utils";

export function TradeDetail({ trade }: { trade: TradeDto }) {
  const { candles, isLoading } = useTradeCandles(trade.id, trade.entryPrice);
  const {
    events,
    metadata,
    addEvent,
    updateEvent,
    updateMetadata,
    addCheckpoint,
    updateCheckpoint,
    deleteCheckpoint,
  } = useTradeEvents(trade);
  const [showAllEvents, setShowAllEvents] = useState(false);

  const visibleEvents = useMemo(
    () =>
      showAllEvents
        ? events
        : events.filter((event) => CORE_TRADE_EVENT_TYPES.includes(event.type)),
    [events, showAllEvents],
  );

  const netReturn = trade.netReturn ?? null;

  return (
    <section className="flex flex-col gap-4">
      <div className="flex flex-wrap items-center justify-between gap-3">
        <div className="flex items-center gap-5 vertical-align-middle">
          <h2 className="text-lg font-bold! text-foreground !m-0 !p-0 leading-none">
            {trade.symbol}
          </h2>
          <Badge
            variant={trade.direction === "Long" ? "default" : "destructive"}
            className="text-sm font-bold px-4 py-4"
          >
            {trade.direction}
          </Badge>
          <TradeStatusBadge status={trade.status} />
        </div>
        <AddTradeEventDialog onAddEvent={addEvent} />
      </div>

      <div className="grid grid-cols-2 gap-4 rounded-lg border border-border bg-muted/20 p-4 sm:grid-cols-3 lg:grid-cols-6">
        <Stat label="Quantity" value={trade.quantity.toString()} />
        <Stat label="Entry Price" value={`$${trade.entryPrice.toFixed(2)}`} />
        <Stat
          label="Close Price"
          value={
            trade.closePrice != null ? `$${trade.closePrice.toFixed(2)}` : "—"
          }
        />
        <Stat
          label="Net Return"
          value={
            netReturn != null
              ? `${netReturn >= 0 ? "+" : ""}${netReturn.toFixed(2)}`
              : "—"
          }
          valueClassName={
            netReturn != null
              ? netReturn >= 0
                ? "text-primary"
                : "text-destructive"
              : undefined
          }
        />
        <Stat label="Opened" value={getDateFormat(trade.openedAt)} />
        <Stat
          label="Closed"
          value={trade.closedAt ? getDateFormat(trade.closedAt) : "—"}
        />
      </div>

      <div className="grid gap-4 lg:grid-cols-3">
        {/* Chart + Timeline on the left (2 cols on large screens) */}
        <div className="flex flex-col gap-4 lg:col-span-2">
          <div className="overflow-hidden rounded-lg border border-border">
            {isLoading ? (
              <div className="flex h-[450px] items-center justify-center">
                <Spinner />
              </div>
            ) : (
              <TradeChart candles={candles} height={450} />
            )}
          </div>

          <div className="rounded-lg border border-border bg-card/50 p-4">
            <div className="mb-4 flex flex-wrap items-center justify-between gap-2">
              <h3 className="text-sm font-semibold text-foreground uppercase tracking-wide text-muted-foreground">
                Timeline of Events
              </h3>
              <label className="flex items-center gap-2 text-sm text-muted-foreground">
                <Checkbox
                  checked={showAllEvents}
                  onCheckedChange={(checked) =>
                    setShowAllEvents(checked === true)
                  }
                />
                Show notes &amp; extra events
              </label>
            </div>
            <TradeEventTimeline
              events={visibleEvents}
              onUpdateEvent={updateEvent}
            />
          </div>
        </div>

        {/* Sidebar on the right (1 col on large screens) */}
        <div className="flex flex-col gap-4">
          <TradeThesisCard
            thesis={metadata.thesis}
            onUpdate={(value) => updateMetadata({ thesis: value })}
          />
          <TradeCheckpointsCard
            checkpoints={metadata.checkpoints}
            onAdd={(desc) => addCheckpoint(desc, "pending")}
            onUpdateStatus={updateCheckpoint}
            onDelete={deleteCheckpoint}
          />
          <TradeOutcomeCard
            outcome={metadata.outcome}
            netReturn={netReturn}
            onUpdate={(value) => updateMetadata({ outcome: value })}
          />
          <TradeAdditionalNotesCard
            notes={metadata.additionalNotes}
            onUpdate={(value) => updateMetadata({ additionalNotes: value })}
          />
        </div>
      </div>
    </section>
  );
}

function Stat({
  label,
  value,
  valueClassName,
}: {
  label: string;
  value: string;
  valueClassName?: string;
}) {
  return (
    <div className="flex flex-col gap-1">
      <span className="text-xs text-muted-foreground">{label}</span>
      <span
        className={cn("text-sm font-medium text-foreground", valueClassName)}
      >
        {value}
      </span>
    </div>
  );
}
