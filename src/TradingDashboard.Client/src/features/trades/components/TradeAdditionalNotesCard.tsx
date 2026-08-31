// components/TradeAdditionalNotesCard.tsx
import { useState } from "react";
import { FileTextIcon } from "lucide-react";
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";

export function TradeAdditionalNotesCard({
  notes,
  onUpdate,
}: {
  notes: string | null | undefined;
  onUpdate: (value: string) => void;
}) {
  const [isEditing, setIsEditing] = useState(false);
  const [text, setText] = useState(notes || "");

  const handleSave = () => {
    onUpdate(text);
    setIsEditing(false);
  };

  return (
    <div className="rounded-lg border border-border bg-card/50 p-4">
      <h3 className="mb-3 text-sm font-semibold text-foreground uppercase tracking-wide text-muted-foreground flex items-center gap-2">
        <FileTextIcon className="h-4 w-4" />
        Additional Notes
      </h3>
      {isEditing ? (
        <div className="space-y-2">
          <Textarea
            value={text}
            onChange={(e) => setText(e.target.value)}
            placeholder="Any other notes or observations?"
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
                setText(notes || "");
                setIsEditing(false);
              }}
              className="flex-1"
            >
              Cancel
            </Button>
          </div>
        </div>
      ) : (
        <p
          onClick={() => setIsEditing(true)}
          className="cursor-text text-sm text-muted-foreground hover:text-foreground text-left"
        >
          {notes ? (
            <span>{notes}</span>
          ) : (
            <span className="italic opacity-60">Click to add notes</span>
          )}
        </p>
      )}
    </div>
  );
}
