// components/TradeThesisCard.tsx
import { useState } from "react";
import { CheckCircleIcon, ClockIcon, AlertCircleIcon } from "lucide-react";
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";

export function TradeThesisCard({
  thesis,
  onUpdate,
}: {
  thesis: string | null | undefined;
  onUpdate: (value: string) => void;
}) {
  const [isEditing, setIsEditing] = useState(false);
  const [text, setText] = useState(thesis || "");

  const handleSave = () => {
    onUpdate(text);
    setIsEditing(false);
  };

  return (
    <div className="rounded-lg border border-border/50 bg-gradient-to-br from-card/70 to-card/50 p-4 border-primary">
      <h3 className="mb-3 text-sm font-semibold text-foreground uppercase tracking-wide text-muted-foreground flex items-center gap-2">
        <CheckCircleIcon className="h-4 w-4" />
        Thesis
      </h3>
      {isEditing ? (
        <div className="space-y-2">
          <Textarea
            value={text}
            onChange={(e) => setText(e.target.value)}
            placeholder="Why did you enter this trade? What's your thesis?"
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
                setText(thesis || "");
                setIsEditing(false);
              }}
              className="flex-1"
            >
              Cancel
            </Button>
          </div>
        </div>
      ) : (
        <div className="space-y-2.5">
          <p
            onClick={() => setIsEditing(true)}
            className="cursor-text text-xs text-muted-foreground hover:text-foreground text-left pb-3"
          >
            {thesis ? (
              <span>{thesis}</span>
            ) : (
              <span className="italic opacity-60">
                Click to add your thesis
              </span>
            )}
          </p>
          <div className="border-t border-border/30 pt-3 space-y-3">
            <div className="grid grid-cols-2 gap-4 items-center">
              <div className="flex items-center gap-2">
                <ClockIcon className="h-3.5 w-3.5 text-muted-foreground" />
                <p className="text-xs text-muted-foreground uppercase tracking-wide">
                  Expected Holding Period
                </p>
              </div>
              <p className="text-sm font-medium text-foreground text-right">
                2–6 weeks
              </p>
            </div>

            <div className="grid grid-cols-2 gap-4 items-start">
              <div className="flex items-center gap-2">
                <AlertCircleIcon className="h-3.5 w-3.5 text-muted-foreground" />
                <p className="text-xs text-muted-foreground uppercase tracking-wide">
                  Invalidation Level
                </p>
              </div>
              <div className="text-right">
                <p className="text-sm font-medium text-chart-4">$94.50</p>
                <p className="text-xs text-muted-foreground mt-0.5 italic">
                  Below this level, thesis is invalidated.
                </p>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
