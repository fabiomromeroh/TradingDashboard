// hooks/useTradeEvents.ts
import { useCallback, useState } from "react";
import type {
  TradeDto,
  TradeEventDto,
  TradeEventType,
  TradeMetadata,
} from "../types/trade.types";

export interface AddTradeEventInput {
  type: TradeEventType;
  occurredAt: string;
  price?: number;
  note?: string;
}

interface UseTradeEventsResult {
  events: TradeEventDto[];
  metadata: TradeMetadata;
  isLoading: boolean;
  addEvent: (input: AddTradeEventInput) => void;
  updateEvent: (id: string, updates: Partial<TradeEventDto>) => void;
  updateMetadata: (updates: Partial<TradeMetadata>) => void;
  addCheckpoint: (
    description: string,
    status: "pending" | "intact" | "broken",
  ) => void;
  updateCheckpoint: (
    id: string,
    status: "pending" | "intact" | "broken",
  ) => void;
  deleteCheckpoint: (id: string) => void;
}

/**
 * TEMPORARY: seeds a plausible entry/add/trim/exit lifecycle around the
 * trade's own dates so the timeline/chart can be built before
 * `GET /trades/{tradeId}/events` exists. Replace the body with a fetch
 * (mirroring useTradesQuery) and wire mutations to API calls —
 * consumers only rely on the returned shape, so no UI code needs to change.
 */
export function useTradeEvents(trade: TradeDto): UseTradeEventsResult {
  const [events, setEvents] = useState<TradeEventDto[]>(() =>
    generateDummyEvents(trade),
  );

  const [metadata, setMetadata] = useState<TradeMetadata>(() => ({
    thesis:
      "Long-term growth play on tech sector. Expecting 20%+ upside over 6 months.",
    additionalNotes:
      "Earnings scheduled for next month — watch for volatility.",
    outcome: null,
    checkpoints: [
      { id: "cp-1", description: "Price holds above $95", status: "intact" },
      { id: "cp-2", description: "Volume confirms trend", status: "pending" },
    ],
  }));

  const addEvent = useCallback(
    (input: AddTradeEventInput) => {
      setEvents((prev) =>
        [
          ...prev,
          {
            id: crypto.randomUUID(),
            tradeId: trade.id,
            type: input.type,
            occurredAt: input.occurredAt,
            price: input.price ?? null,
            note: input.note ?? null,
          },
        ].sort(
          (a, b) =>
            new Date(a.occurredAt).getTime() - new Date(b.occurredAt).getTime(),
        ),
      );
    },
    [trade.id],
  );

  const updateEvent = useCallback(
    (id: string, updates: Partial<TradeEventDto>) => {
      setEvents((prev) =>
        prev.map((e) => (e.id === id ? { ...e, ...updates } : e)),
      );
    },
    [],
  );

  const updateMetadata = useCallback((updates: Partial<TradeMetadata>) => {
    setMetadata((prev) => ({ ...prev, ...updates }));
  }, []);

  const addCheckpoint = useCallback(
    (description: string, status: "pending" | "intact" | "broken") => {
      setMetadata((prev) => ({
        ...prev,
        checkpoints: [
          ...(prev.checkpoints || []),
          { id: crypto.randomUUID(), description, status },
        ],
      }));
    },
    [],
  );

  const updateCheckpoint = useCallback(
    (id: string, status: "pending" | "intact" | "broken") => {
      setMetadata((prev) => ({
        ...prev,
        checkpoints: (prev.checkpoints || []).map((cp) =>
          cp.id === id ? { ...cp, status } : cp,
        ),
      }));
    },
    [],
  );

  const deleteCheckpoint = useCallback((id: string) => {
    setMetadata((prev) => ({
      ...prev,
      checkpoints: (prev.checkpoints || []).filter((cp) => cp.id !== id),
    }));
  }, []);

  return {
    events,
    metadata,
    isLoading: false,
    addEvent,
    updateEvent,
    updateMetadata,
    addCheckpoint,
    updateCheckpoint,
    deleteCheckpoint,
  };
}

function generateDummyEvents(trade: TradeDto): TradeEventDto[] {
  const opened = new Date(trade.openedAt).getTime();
  const closed = trade.closedAt ? new Date(trade.closedAt).getTime() : null;
  const span = closed ? closed - opened : 1000 * 60 * 60 * 24 * 14;
  const at = (fraction: number) =>
    new Date(opened + span * fraction).toISOString();

  const events: TradeEventDto[] = [
    {
      id: "evt-entry",
      tradeId: trade.id,
      type: "Entry",
      occurredAt: trade.openedAt,
      price: trade.entryPrice,
      note: "Initial position opened per plan.",
    },
    {
      id: "evt-add",
      tradeId: trade.id,
      type: "Add",
      occurredAt: at(0.25),
      price: round2(trade.entryPrice * 1.02),
      note: "Added to the position on strength.",
    },
    {
      id: "evt-earnings-note",
      tradeId: trade.id,
      type: "Note",
      occurredAt: at(0.45),
      note: "Earnings reported — no change to thesis.",
    },
    {
      id: "evt-trim",
      tradeId: trade.id,
      type: "Trim",
      occurredAt: at(0.6),
      price: round2(trade.entryPrice * 1.08),
      note: "Trimmed a third of the position into strength.",
    },
  ];

  if (trade.status === "Closed" && trade.closedAt) {
    events.push({
      id: "evt-exit",
      tradeId: trade.id,
      type: "FinalExit",
      occurredAt: trade.closedAt,
      price: trade.closePrice,
      note: "Position closed.",
    });
  }

  return events;
}

function round2(value: number): number {
  return Math.round(value * 100) / 100;
}
