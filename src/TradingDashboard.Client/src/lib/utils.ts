import type {
  ApiError,
  ApiErrorResponse,
  ApiOption,
  SelectOption,
} from "@/types/api.types";
import { clsx, type ClassValue } from "clsx";
import { toast } from "sonner";
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

export function normalizeErrors(response: ApiErrorResponse): ApiError[] {
  if (Array.isArray(response.errors)) {
    return response.errors;
  }
  return [
    { code: "error", message: "Something went wrong. Please try again." },
  ];
}

export function handleApiError(response: ApiErrorResponse) {
  const errors = normalizeErrors(response);
  if (errors.length === 1) {
    toast.error(errors[0].message, { duration: 5000 });
  } else {
    toast.error(errors.map((e) => `• ${e.message}`).join("\n"), {
      duration: 7000,
    });
  }
  return errors;
}

export function getDateFormat(raw: string) {
  if (!raw) return "—";
  return new Date(raw).toLocaleString("en-IE", {
    year: "numeric",
    month: "short",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
    timeZone: "UTC",
  });
}
