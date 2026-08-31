import { useState } from "react";
import { CrownIcon } from "lucide-react";
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";

export function TradeOutcomeCard({
  outcome,
  netReturn,
  onUpdate,
}: {
  outcome: string | null | undefined;
  netReturn: number | null;
  onUpdate: (value: string) => void;
}) {
  const [isEditing, setIsEditing] = useState(false);
  const [text, setText] = useState(outcome || "");

  const handleSave = () => {
    onUpdate(text);
    setIsEditing(false);
  };

  const isWin = netReturn != null && netReturn > 0;
  const rMultiple = netReturn != null ? (netReturn / 1).toFixed(2) : null;

  return (
    <div className="rounded-lg border border-border/50 bg-gradient-to-br from-card/70 to-card/50 p-4 border-yellow-500">
      <div className="mb-3 flex items-center justify-between">
        <h3 className="text-sm font-semibold text-foreground uppercase tracking-wide text-muted-foreground flex items-center gap-2">
          <CrownIcon className="h-4 w-4" />
          Outcome
        </h3>
      </div>
      {isEditing ? (
        <div className="space-y-2">
          <Textarea
            value={text}
            onChange={(e) => setText(e.target.value)}
            placeholder="How did the trade turn out? What did you learn?"
            className="min-h-24"
          />
          <div className="flex gap-2">
            <Button size="sm" onClick={handleSave} className="flex-1">
              Save
            </Button>
            <Button
              size="sm"
              variant="outline"
              onClick={() => {
                setText(outcome || "");
                setIsEditing(false);
              }}
              className="flex-1"
            >
              Cancel
            </Button>
          </div>
        </div>
      ) : (
        <div className="space-y-0">
          {rMultiple != null && (
            <>
              <div className="pb-3 border-b border-border/30">
                <div className="grid grid-cols-2 gap-4 text-left">
                  <div className="flex flex-col items-start justify-start">
                    <p
                      className={`text-3xl font-bold ${
                        isWin ? "text-chart-2" : "text-destructive"
                      }`}
                    >
                      {isWin ? "+" : ""}
                      {rMultiple}R
                    </p>
                  </div>
                  <div className="flex flex-col items-start justify-start">
                    <Badge
                      variant={isWin ? "default" : "destructive"}
                      className="text-sm font-semibold px-6 py-3"
                    >
                      {isWin ? "WIN" : "LOSS"}
                    </Badge>
                  </div>
                </div>
              </div>

              <div className="pt-3">
                <p className="text-xs font-bold text-muted-foreground tracking-wider text-left uppercase pb-3">
                  Lesson Learned
                </p>
                <div
                  onClick={() => setIsEditing(true)}
                  className="cursor-text text-xs text-muted-foreground hover:text-foreground text-left"
                >
                  {outcome ? (
                    <span>{outcome}</span>
                  ) : (
                    <span className="italic opacity-60">
                      Click to add notes
                    </span>
                  )}
                </div>
              </div>
            </>
          )}
        </div>
      )}
    </div>
  );
}
