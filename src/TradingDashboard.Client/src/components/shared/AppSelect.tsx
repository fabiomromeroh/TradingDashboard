import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "../ui/select";

interface Options {
  value: string;
  label: string;
}

interface AppSelectProps {
  name: string;
  options: Options[];
  value?: string;
  defaultValue?: string;
  placeholder?: string;
  className?: string;
  groupLabel?: string;
  onChange: (value: string) => void;
}

export function AppSelect({
  name,
  options,
  value,
  defaultValue,
  placeholder,
  className,
  groupLabel,
  onChange,
}: AppSelectProps) {
  return (
    <Select
      name={name}
      defaultValue={defaultValue}
      value={value}
      onValueChange={onChange}
    >
      <SelectTrigger className={className}>
        <SelectValue placeholder={placeholder} />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          {groupLabel && <SelectLabel>{groupLabel}</SelectLabel>}
          {options.map((option) => (
            <SelectItem key={option.value} value={option.value}>
              {option.label}
            </SelectItem>
          ))}
        </SelectGroup>
      </SelectContent>
    </Select>
  );
}
