import { useState } from "react";
import { Plus } from "lucide-react";
import { cn } from "@/lib/utils";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  MAIN_CATALOG,
  OVERVIEW_CATALOG,
} from "../../../components/shared/widgets/base/WidgetRegistry";
import type { WidgetCatalogItem } from "../types/dashboard.types";
import type { WidgetType, WidgetZone } from "../types/dashboard.types";

interface WidgetCatalogProps {
  open: boolean;
  onClose: () => void;
  onAdd: (type: WidgetType, zone: WidgetZone) => void;
}

function CatalogCard({
  item,
  onAdd,
}: {
  item: WidgetCatalogItem;
  onAdd: () => void;
}) {
  return (
    <div
      className={cn(
        "flex flex-col gap-2 rounded-lg border border-border/60 bg-card p-4",
        "hover:border-border hover:bg-accent/30 transition-colors",
      )}
    >
      <div className="flex items-start justify-between gap-2">
        <span className="text-sm font-medium leading-tight">{item.label}</span>
        <Badge variant="secondary" className="shrink-0 text-[10px]">
          {item.zone}
        </Badge>
      </div>
      <p className="text-xs text-muted-foreground leading-relaxed flex-1">
        {item.description}
      </p>
      <Button
        size="sm"
        variant="outline"
        className="w-full mt-1"
        onClick={onAdd}
      >
        <Plus className="size-3.5 mr-1.5" />
        Add
      </Button>
    </div>
  );
}

export function WidgetCatalogModal({
  open,
  onClose,
  onAdd,
}: WidgetCatalogProps) {
  const [activeTab, setActiveTab] = useState<"overview" | "main">("overview");

  function handleAdd(item: WidgetCatalogItem) {
    onAdd(item.type, item.zone);
    onClose();
  }

  return (
    <Dialog open={open} onOpenChange={(v) => !v && onClose()}>
      <DialogContent className="sm:max-w-6xl">
        <DialogHeader>
          <DialogTitle>Add Widget</DialogTitle>
          <DialogDescription></DialogDescription>
        </DialogHeader>

        <Tabs
          value={activeTab}
          onValueChange={(v) => setActiveTab(v as "overview" | "main")}
        >
          <TabsList className="mb-4">
            <TabsTrigger value="overview">Metric Widgets</TabsTrigger>
            <TabsTrigger value="main">Chart Widgets</TabsTrigger>
          </TabsList>

          <TabsContent value="overview">
            <p className="text-xs text-muted-foreground mb-4">
              Compact metric cards shown in the overview row at the top of the
              dashboard.
            </p>
            <div className="grid grid-cols-2 sm:grid-cols-3 gap-3">
              {OVERVIEW_CATALOG.map((item) => (
                <CatalogCard
                  key={item.type}
                  item={item}
                  onAdd={() => handleAdd(item)}
                />
              ))}
            </div>
          </TabsContent>

          <TabsContent value="main">
            <p className="text-xs text-muted-foreground mb-4">
              Full-size chart and analysis widgets displayed in the main
              dashboard area.
            </p>
            <div className="grid grid-cols-2 gap-3">
              {MAIN_CATALOG.map((item) => (
                <CatalogCard
                  key={item.type}
                  item={item}
                  onAdd={() => handleAdd(item)}
                />
              ))}
            </div>
          </TabsContent>
        </Tabs>
      </DialogContent>
    </Dialog>
  );
}
