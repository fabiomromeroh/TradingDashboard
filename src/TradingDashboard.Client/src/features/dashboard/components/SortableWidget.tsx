import { useSortable } from "@dnd-kit/sortable";
import { GripVertical, X } from "lucide-react";
import { cn } from "@/lib/utils";
import { CSS } from "@dnd-kit/utilities";
import { Button } from "@/components/ui/button";
import type { DashboardWidget } from "../types/dashboard.types";
import { getWidgetColSpan } from "../../../components/shared/widgets/base/WidgetRegistry";
import { LiveWidget } from "./LiveWidget";

interface SortableWidgetProps {
  widget: DashboardWidget;
  onRemove: (id: string, zone: DashboardWidget["zone"]) => void;
}

export function SortableWidget({ widget, onRemove }: SortableWidgetProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: widget.id });

  const colSpan = getWidgetColSpan(widget.type);

  return (
    <div
      ref={setNodeRef}
      style={{
        transform: CSS.Transform.toString(transform),
        transition,
      }}
      className={cn(
        "relative group touch-none",
        colSpan === 2 && "md:col-span-3",
        isDragging && "opacity-40 z-50 scale-[1.02]",
      )}
      {...attributes}
    >
      {/* Drag handle */}
      <Button
        variant="ghost"
        size="icon"
        {...listeners}
        aria-label="Drag to reorder"
        className={cn(
          "absolute top-2 left-2 z-10 size-7",
          "opacity-0 group-hover:opacity-100 transition-opacity",
          "cursor-grab active:cursor-grabbing",
        )}
      >
        <GripVertical className="size-4" />
      </Button>

      {/* Remove button */}
      <Button
        variant="ghost"
        size="icon"
        onClick={() => onRemove(widget.id, widget.zone)}
        aria-label="Remove widget"
        className={cn(
          "absolute top-2 right-2 z-10 size-7",
          "opacity-0 group-hover:opacity-100 transition-opacity",
        )}
      >
        <X className="size-3.5" />
      </Button>

      <LiveWidget widget={widget} />
    </div>
  );
}
