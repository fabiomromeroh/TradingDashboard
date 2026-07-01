import type { Control, FieldValues, Path } from "react-hook-form";
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../ui/form";
import { AppSelect } from "./AppSelect";

type Option = { value: string; label: string };

type AppSelectFieldProps<T extends FieldValues> = {
  control: Control<T>;
  name: Path<T>;
  label: string;
  options: Option[];
  placeholder: string;
  className?: string;
};

export function AppSelectField<T extends FieldValues>({
  control,
  name,
  label,
  options,
  placeholder,
  className,
}: AppSelectFieldProps<T>) {
  return (
    <FormField
      control={control}
      name={name}
      render={({ field }) => (
        <FormItem>
          <FormLabel>{label}</FormLabel>
          <FormControl>
            <AppSelect
              name={name}
              defaultValue={field.value}
              placeholder={placeholder}
              options={options}
              value={field.value}
              className={className}
              onChange={field.onChange}
            />
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
