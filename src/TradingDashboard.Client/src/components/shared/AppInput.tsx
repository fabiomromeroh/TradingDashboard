import { Field, FieldDescription, FieldLabel } from "../ui/field";
import { Input } from "../ui/input";

type AppInputProps = {
  id: string;
  name?: string;
  placeholder?: string;
  type?: "text" | "password" | "number" | "email";
  required?: boolean;
  className?: string;
  fieldDesc?: string;
  label?: string;
  value?: string;
  autoComplete?: string;
  onBlur?: (e: React.FocusEvent<HTMLInputElement>) => void;
  onFocus?: (e: React.FocusEvent<HTMLInputElement>) => void;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
};

export function AppInput(props: AppInputProps) {
  return (
    <Field>
      {props.label && <FieldLabel htmlFor={props.id}>{props.label}</FieldLabel>}
      <Input {...props} />
      {props.fieldDesc && (
        <FieldDescription>{props.fieldDesc}</FieldDescription>
      )}
    </Field>
  );
}
