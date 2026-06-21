import { Select, SelectContent, SelectGroup, SelectItem, SelectTrigger, SelectValue } from "../ui/select";

interface Options {
    value: string;
    label: string;
}

interface AppSelectProps {
    name: string;
    options: Options[];
    value: string;
    defaultValue: string;
    placeholder?: string;
    onChange: (value: string) => void;
}

export function AppSelect({ name, options, value, defaultValue, placeholder, onChange }: AppSelectProps) { 
    return <Select  name={name} defaultValue={defaultValue} value={value} onValueChange={onChange}>
            <SelectTrigger className="w-[180px]">
                <SelectValue placeholder={placeholder} />
            </SelectTrigger>
            <SelectContent>
                <SelectGroup>
                    {options.map((option) => (
                <SelectItem key={option.value} value={option.value}>{option.label}</SelectItem>
            
                ))}
                </SelectGroup>
            </SelectContent>
        </Select>
    
}