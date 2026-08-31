// components/TradeEventTimeline.tsx
import { useState } from "react";
import { cn } from "@/lib/utils";
import { TradeEventBadge, getTradeEventDotClassName } from "./TradeEventBadge";
import type { TradeEventDto } from "../types/trade.types";
import { Textarea } from "@/components/ui/textarea";

function formatEventDate(raw: string): string {
  return new Date(raw).toLocaleDateString("en-IE", {
    year: "numeric",
    month: "short",
    day: "2-digit",
  });
}

export function TradeEventTimeline({
  events,
  onUpdateEvent,
}: {
  events: TradeEventDto[];
  onUpdateEvent?: (id: string, updates: Partial<TradeEventDto>) => void;
}) {
  const [editingId, setEditingId] = useState<string | null>(null);
  const [editText, setEditText] = useState("");

  if (events.length === 0) {
    return (
      <p className="text-sm text-muted-foreground">
        No events to show yet — add one to start the timeline.
      </p>
    );
  }

  const sorted = [...events].sort(
    (a, b) =>
      new Date(a.occurredAt).getTime() - new Date(b.occurredAt).getTime(),
  );

  const startEditing = (event: TradeEventDto) => {
    setEditingId(event.id);
    setEditText(event.note || "");
  };

  const saveEdit = (eventId: string) => {
    onUpdateEvent?.(eventId, { note: editText || null });
    setEditingId(null);
  };

  return (
    <div className="overflow-x-auto pb-2">
      <div className="relative flex min-w-max gap-6">
        {/* connects the dots into a single timeline */}
        <div className="pointer-events-none absolute inset-x-8 top-[11px] h-px bg-border" />
        {sorted.map((event) => (
          <div
            key={event.id}
            className="flex w-56 shrink-0 flex-col items-center gap-2"
          >
            {/* dot on the timeline */}
            <span
              className={cn(
                "z-10 h-3 w-3 shrink-0 rounded-full ring-4 ring-card",
                getTradeEventDotClassName(event.type),
              )}
            />
            {/* event card with header and body sections */}
            <div className="w-full rounded-md border border-border bg-card/50 overflow-hidden border-primary">
              {/* header: badge and date */}
              <div className="flex items-center justify-between gap-2 border-b border-border/50 px-3 py-2">
                <TradeEventBadge type={event.type} />
                <span className="text-xs text-muted-foreground">
                  {formatEventDate(event.occurredAt)}
                </span>
              </div>
              {/* body: price and note (inline editing) */}
              <div className="px-3 py-2.5">
                {event.price != null && (
                  <p className="text-xs font-medium text-foreground pb-2">
                    ${event.price.toFixed(2)}
                  </p>
                )}
                {editingId === event.id ? (
                  <div className="mt-2 flex flex-col gap-2">
                    <Textarea
                      value={editText}
                      onChange={(e) => setEditText(e.target.value)}
                      placeholder="Add notes..."
                      className="min-h-20 text-xs"
                    />
                    <div className="flex gap-1">
                      <button
                        onClick={() => saveEdit(event.id)}
                        className="flex-1 rounded bg-primary/20 px-2 py-1 text-xs text-primary hover:bg-primary/30"
                      >
                        Save
                      </button>
                      <button
                        onClick={() => setEditingId(null)}
                        className="flex-1 rounded bg-muted px-2 py-1 text-xs text-muted-foreground hover:bg-muted/80"
                      >
                        Cancel
                      </button>
                    </div>
                  </div>
                ) : (
                  <p
                    onClick={() => startEditing(event)}
                    className="mt-1 cursor-text line-clamp-3 text-xs text-muted-foreground hover:text-foreground hover:underline"
                  >
                    {event.note || (
                      <span className="italic opacity-60">
                        Click to add note
                      </span>
                    )}
                  </p>
                )}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
