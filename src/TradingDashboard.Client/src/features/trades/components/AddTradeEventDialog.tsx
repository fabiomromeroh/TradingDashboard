// components/AddTradeEventDialog.tsx
import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { PlusIcon } from "lucide-react";
import { toast } from "sonner";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { AppButton } from "@/components/shared/AppButton";
import { AppSelectField } from "@/components/shared/AppSelectField";
import { getTradeEventLabel } from "./TradeEventBadge";
import type { TradeEventType } from "../types/trade.types";
import type { AddTradeEventInput } from "../hooks/useTradeEvents";

const EVENT_TYPE_OPTIONS: TradeEventType[] = [
  "Entry",
  "Add",
  "Trim",
  "PartialExit",
  "FinalExit",
  "Note",
];

const tradeEventSchema = z.object({
  type: z.enum(["Entry", "Add", "Trim", "PartialExit", "FinalExit", "Note"]),
  occurredAt: z.string().min(1, "Date is required"),
  price: z
    .string()
    .optional()
    .refine((v) => !v || !Number.isNaN(Number(v)), "Price must be a number"),
  note: z.string().max(500, "Keep notes under 500 characters").optional(),
});

type TradeEventFormValues = z.infer<typeof tradeEventSchema>;

function todayAsDateInputValue(): string {
  return new Date().toISOString().slice(0, 10);
}

export function AddTradeEventDialog({
  onAddEvent,
}: {
  onAddEvent: (input: AddTradeEventInput) => void;
}) {
  const [open, setOpen] = useState(false);
  const form = useForm<TradeEventFormValues>({
    resolver: zodResolver(tradeEventSchema),
    defaultValues: {
      type: "Note",
      occurredAt: todayAsDateInputValue(),
      price: "",
      note: "",
    },
  });

  function handleFormSubmit(values: TradeEventFormValues) {
    onAddEvent({
      type: values.type,
      occurredAt: new Date(values.occurredAt).toISOString(),
      price: values.price ? Number(values.price) : undefined,
      note: values.note || undefined,
    });
    toast.success("Event added");
    setOpen(false);
    form.reset({
      type: "Note",
      occurredAt: todayAsDateInputValue(),
      price: "",
      note: "",
    });
  }

  return (
    <Form {...form}>
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogTrigger asChild>
          <AppButton variant="outline" className="gap-2">
            <PlusIcon className="h-4 w-4" />
            Add Event
          </AppButton>
        </DialogTrigger>
        <DialogContent className="sm:max-w-sm">
          <DialogHeader>
            <DialogTitle>Add Event</DialogTitle>
            <DialogDescription>
              Log a change to this position, or leave a note.
            </DialogDescription>
          </DialogHeader>
          <form
            onSubmit={form.handleSubmit(handleFormSubmit)}
            className="space-y-4"
          >
            <div className="grid grid-cols-2 gap-4">
              <AppSelectField
                name="type"
                control={form.control}
                label="Type"
                placeholder="Select a type"
                options={EVENT_TYPE_OPTIONS.map((type) => ({
                  value: type,
                  label: getTradeEventLabel(type),
                }))}
              />
              <FormField
                control={form.control}
                name="occurredAt"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Date</FormLabel>
                    <FormControl>
                      <Input type="date" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <FormField
              control={form.control}
              name="price"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Price (optional)</FormLabel>
                  <FormControl>
                    <Input
                      type="number"
                      step="0.01"
                      placeholder="0.00"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="note"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Note</FormLabel>
                  <FormControl>
                    <Textarea
                      placeholder="What happened, and why?"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <DialogFooter>
              <DialogClose asChild>
                <AppButton variant="outline" type="button">
                  Cancel
                </AppButton>
              </DialogClose>
              <AppButton type="submit">Save</AppButton>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </Form>
  );
}
