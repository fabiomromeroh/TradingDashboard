import * as React from "react";
import { Check, ChevronDown } from "lucide-react";

import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";

export type AppMultiSelectOption = {
  label: string;
  value: string;
  disabled?: boolean;
};

type AppMultiSelectProps = {
  options: AppMultiSelectOption[];
  value: string[];
  onValueChange: (value: string[]) => void;
  placeholder?: string;
  label?: string;
  className?: string;
  contentClassName?: string;
  maxDisplayedItems?: number;
  disabled?: boolean;
};

export function AppMultiSelect({
  options,
  value,
  onValueChange,
  placeholder = "Select options",
  label,
  className,
  contentClassName,
  maxDisplayedItems = 2,
  disabled,
}: AppMultiSelectProps) {
  const [open, setOpen] = React.useState(false);

  const selectedOptions = options.filter((option) =>
    value.includes(option.value),
  );

  const toggleOption = (optionValue: string) => {
    const exists = value.includes(optionValue);

    if (exists) {
      const filtered = value.filter((v) => v !== optionValue);
      return onValueChange(filtered);
    }

    const newValue = [...value, optionValue];
    console.log("selected", newValue);

    return onValueChange(newValue);
  };

  const clearAll = () => {
    onValueChange([]);
  };

  const displayValue = React.useMemo(() => {
    if (selectedOptions.length === 0) {
      return placeholder;
    }

    if (selectedOptions.length <= maxDisplayedItems) {
      return selectedOptions.map((option) => option.label).join(", ");
    }

    const visible = selectedOptions
      .slice(0, maxDisplayedItems)
      .map((option) => option.label)
      .join(", ");

    return `${visible} +${selectedOptions.length - maxDisplayedItems} more`;
  }, [selectedOptions, placeholder, maxDisplayedItems]);

  return (
    <div className="flex flex-col gap-2">
      {label ? (
        <label className="text-sm font-medium text-foreground">{label}</label>
      ) : null}

      <Popover open={open} onOpenChange={setOpen}>
        <PopoverTrigger asChild>
          <Button
            type="button"
            variant="outline"
            role="combobox"
            aria-expanded={open}
            disabled={disabled}
            className={cn(
              "w-full justify-between font-normal",
              selectedOptions.length === 0 && "text-muted-foreground",
              className,
            )}
          >
            <span className="truncate text-left">{displayValue}</span>
            <ChevronDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
          </Button>
        </PopoverTrigger>

        <PopoverContent
          align="start"
          className={cn(
            "w-[var(--radix-popover-trigger-width)] p-2",
            contentClassName,
          )}
        >
          <div className="space-y-1">
            {options.map((option) => {
              const checked = value.includes(option.value);

              return (
                <button
                  key={option.value}
                  type="button"
                  disabled={option.disabled}
                  onClick={() => toggleOption(option.value)}
                  className={cn(
                    "flex w-full items-center gap-2 rounded-md px-2 py-2 text-sm transition-colors",
                    "hover:bg-accent hover:text-accent-foreground",
                    "disabled:pointer-events-none disabled:opacity-50",
                  )}
                >
                  <Checkbox checked={checked} className="pointer-events-none" />
                  <span className="flex-1 text-left">{option.label}</span>
                  {checked ? (
                    <Check className="h-4 w-4 text-muted-foreground" />
                  ) : null}
                </button>
              );
            })}
          </div>

          {value.length > 0 ? (
            <>
              <div className="my-2 h-px bg-border" />
              <Button
                type="button"
                variant="ghost"
                className="w-full justify-start"
                onClick={clearAll}
              >
                Clear selection
              </Button>
            </>
          ) : null}
        </PopoverContent>
      </Popover>
    </div>
  );
}
