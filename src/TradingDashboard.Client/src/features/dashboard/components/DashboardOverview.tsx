import { useState } from "react";
import {
  DndContext,
  DragOverlay,
  KeyboardSensor,
  PointerSensor,
  closestCenter,
  useSensor,
  useSensors,
  type DragEndEvent,
  type DragStartEvent,
} from "@dnd-kit/core";
import {
  SortableContext,
  arrayMove,
  rectSortingStrategy,
  sortableKeyboardCoordinates,
} from "@dnd-kit/sortable";
import { Plus, LayoutDashboard } from "lucide-react";
import { Separator } from "@/components/ui/separator";
import { useDashboardLayout } from "../hooks/useDashboardLayout";
import { SortableWidget } from "./SortableWidget";
import { WidgetCatalogModal } from "./WidgetCatalogModal";
import type { DashboardWidget } from "../types/dashboard.types";
import { useAppSelector } from "@/store/hooks";
import { renderWidgetFromDto } from "@/components/shared/widgets/base/WidgetRegistry";
import { AppButton } from "@/components/shared/AppButton";

function EmptyZone({ label, onAdd }: { label: string; onAdd: () => void }) {
  return (
    <div
      className="flex flex-col items-center justify-center gap-3 rounded-xl border-2 border-dashed border-border/50 bg-muted/20 py-10 text-center"
      role="region"
      aria-label={`Empty ${label} zone`}
    >
      <LayoutDashboard className="size-8 text-muted-foreground/40" />
      <p className="text-sm text-muted-foreground">No widgets yet</p>
      <AppButton variant="outline" size="sm" onClick={onAdd}>
        <Plus className="size-3.5 mr-1.5" />
        Add Widget
      </AppButton>
    </div>
  );
}

/** Overlay preview shown while dragging */
function DragPreview({ widget }: { widget: DashboardWidget }) {
  const metricData = useAppSelector((x) =>
    x.metric.metrics.find((m) => m.widgetType === widget.type),
  );

  return (
    <div className="rotate-1 opacity-90 shadow-2xl pointer-events-none">
      {metricData && renderWidgetFromDto(metricData)}
    </div>
  );
}

export function DashboardOverview() {
  const { layout, saveLayout, addWidget, removeWidget } = useDashboardLayout();
  const [catalogOpen, setCatalogOpen] = useState(false);
  const [activeWidget, setActiveWidget] = useState<DashboardWidget | null>(
    null,
  );

  const sensors = useSensors(
    useSensor(PointerSensor, { activationConstraint: { distance: 8 } }),
    useSensor(KeyboardSensor, {
      coordinateGetter: sortableKeyboardCoordinates,
    }),
  );

  function handleDragStart(event: DragStartEvent) {
    const id = event.active.id as string;
    const found =
      layout.overview.find((w) => w.id === id) ??
      layout.main.find((w) => w.id === id) ??
      null;
    setActiveWidget(found);
  }

  function handleDragEnd(event: DragEndEvent) {
    setActiveWidget(null);
    const { active, over } = event;
    if (!over || active.id === over.id) return;

    const activeId = active.id as string;
    const overId = over.id as string;

    const inOverview = layout.overview.some((w) => w.id === activeId);
    if (inOverview) {
      const oldIdx = layout.overview.findIndex((w) => w.id === activeId);
      const newIdx = layout.overview.findIndex((w) => w.id === overId);
      if (oldIdx !== -1 && newIdx !== -1) {
        saveLayout({
          ...layout,
          overview: arrayMove(layout.overview, oldIdx, newIdx),
        });
      }
    } else {
      const oldIdx = layout.main.findIndex((w) => w.id === activeId);
      const newIdx = layout.main.findIndex((w) => w.id === overId);
      if (oldIdx !== -1 && newIdx !== -1) {
        saveLayout({ ...layout, main: arrayMove(layout.main, oldIdx, newIdx) });
      }
    }
  }

  return (
    <DndContext
      sensors={sensors}
      collisionDetection={closestCenter}
      onDragStart={handleDragStart}
      onDragEnd={handleDragEnd}
    >
      <div className="flex flex-col gap-6">
        {/* Page header */}
        <div className="flex items-center justify-between gap-4">
          <AppButton onClick={() => setCatalogOpen(true)} size="sm">
            <Plus className="size-4 mr-2" />
            Add Widget
          </AppButton>
        </div>

        {/* ── Overview zone ── */}
        <section>
          <div className="flex items-center gap-3 mb-4">
            <span className="text-[11px] font-semibold uppercase tracking-widest text-muted-foreground">
              Overview
            </span>
            <Separator className="flex-1" />
          </div>

          {layout.overview.length === 0 ? (
            <EmptyZone label="overview" onAdd={() => setCatalogOpen(true)} />
          ) : (
            <SortableContext
              items={layout.overview.map((w) => w.id)}
              strategy={rectSortingStrategy}
            >
              <div className="grid gap-4 max-[645px]:grid-cols-1 min-[646px]:max-[917px]:grid-cols-2 min-[918px]:max-[1180px]:grid-cols-3 min-[1180px]:max-[1630px]:grid-cols-4 min-[1630px]:grid-cols-5 min-[1630px]:max-[1800px]:max-[1630px]:grid-cols-4 min-[1800px]:grid-cols-6">
                {layout.overview.map((widget) => (
                  <SortableWidget
                    key={widget.id}
                    widget={widget}
                    onRemove={removeWidget}
                  />
                ))}
              </div>
            </SortableContext>
          )}
        </section>

        {/* ── Main zone ── */}
        <section>
          <div className="flex items-center gap-3 mb-4">
            <span className="text-[11px] font-semibold uppercase tracking-widest text-muted-foreground">
              Charts &amp; Analysis
            </span>
            <Separator className="flex-1" />
          </div>

          {layout.main.length === 0 ? (
            <EmptyZone label="main" onAdd={() => setCatalogOpen(true)} />
          ) : (
            <SortableContext
              items={layout.main.map((w) => w.id)}
              strategy={rectSortingStrategy}
            >
              <div className="grid gap-4 grid-cols-1 md:grid-cols-4">
                {layout.main.map((widget) => (
                  <SortableWidget
                    key={widget.id}
                    widget={widget}
                    onRemove={removeWidget}
                  />
                ))}
              </div>
            </SortableContext>
          )}
        </section>
      </div>

      {/* Drag ghost overlay */}
      <DragOverlay>
        {activeWidget ? <DragPreview widget={activeWidget} /> : null}
      </DragOverlay>

      {/* Add widget dialog */}
      <WidgetCatalogModal
        open={catalogOpen}
        onClose={() => setCatalogOpen(false)}
        onAdd={addWidget}
      />
    </DndContext>
  );
}
