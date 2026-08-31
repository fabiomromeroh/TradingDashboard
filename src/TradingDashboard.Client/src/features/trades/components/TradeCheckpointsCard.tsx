// components/TradeCheckpointsCard.tsx
import { useState } from "react";
import {
  CheckIcon,
  AlertCircleIcon,
  CircleIcon,
  TrashIcon,
  ListChecksIcon,
  CalendarIcon,
} from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import type { TradeCheckpoint } from "../types/trade.types";
import { getDateFormat } from "@/lib/utils";

const statusIcons = {
  pending: <CircleIcon className="h-4 w-4 text-muted-foreground" />,
  intact: <CheckIcon className="h-4 w-4 text-primary" />,
  broken: <AlertCircleIcon className="h-4 w-4 text-destructive" />,
};

const statusLabel = {
  pending: "Pending",
  intact: "Intact",
  broken: "Broken",
};

export function TradeCheckpointsCard({
  checkpoints,
  onAdd,
  onUpdateStatus,
  onDelete,
}: {
  checkpoints: TradeCheckpoint[] | undefined;
  onAdd: (description: string) => void;
  onUpdateStatus: (id: string, status: "pending" | "intact" | "broken") => void;
  onDelete: (id: string) => void;
}) {
  const [newCheckpoint, setNewCheckpoint] = useState("");

  const handleAdd = () => {
    if (newCheckpoint.trim()) {
      onAdd(newCheckpoint);
      setNewCheckpoint("");
    }
  };

  const items = checkpoints || [];

  return (
    <div className="rounded-lg border border-border/50 bg-gradient-to-br from-card/70 to-card/50 p-4 border-primary">
      <h3 className="mb-3 text-sm font-semibold text-foreground uppercase tracking-wide text-muted-foreground flex items-center gap-2">
        <ListChecksIcon className="h-4 w-4" />
        Checkpoints
      </h3>

      <div className="space-y-2">
        {items.map((cp) => (
          <div
            key={cp.id}
            className="grid grid-cols-3 gap-3 rounded bg-muted/20 p-3 items-start text-left"
          >
            <div className="flex items-start gap-2 min-w-0">
              <CalendarIcon className="h-3.5 w-3.5 text-muted-foreground shrink-0 mt-0.5" />
              <p className="text-xs text-muted-foreground">
                {cp.createdAt ? getDateFormat(cp.createdAt) : "—"}
              </p>
            </div>
            <div className="min-w-0">
              <p className="text-xs text-muted-foreground line-clamp-2">
                {cp.description}
              </p>
            </div>
            <div className="flex items-center justify-between gap-2 shrink-0">
              <Badge
                variant={cp.status === "broken" ? "destructive" : "secondary"}
                className="text-xs"
              >
                {statusLabel[cp.status]}
              </Badge>
              <button
                onClick={() => {
                  const next = {
                    pending: "intact",
                    intact: "broken",
                    broken: "pending",
                  }[cp.status] as "pending" | "intact" | "broken";
                  onUpdateStatus(cp.id, next);
                }}
                className="rounded p-1 text-muted-foreground hover:text-foreground"
                title={`Current: ${statusLabel[cp.status]}. Click to cycle.`}
              >
                {statusIcons[cp.status]}
              </button>
              <button
                onClick={() => onDelete(cp.id)}
                className="rounded p-1 text-muted-foreground hover:bg-muted hover:text-destructive"
              >
                <TrashIcon className="h-3.5 w-3.5" />
              </button>
            </div>
          </div>
        ))}
      </div>

      <div className="mt-3 flex gap-2">
        <Input
          value={newCheckpoint}
          onChange={(e) => setNewCheckpoint(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === "Enter") {
              handleAdd();
            }
          }}
          placeholder="Add a checkpoint..."
          className="text-sm"
        />
        <Button size="sm" onClick={handleAdd}>
          Add
        </Button>
      </div>
    </div>
  );
}
