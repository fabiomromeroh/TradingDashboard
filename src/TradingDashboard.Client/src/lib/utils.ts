import type { ApiOption, SelectOption } from "@/types/api.types";
import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function toSelectOptions(items: ApiOption[]): SelectOption[] {
  return items.map((item) => ({
    value: item.id,
    label: item.name,
  }));
}
