import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "../ui/tooltip";
import { Button } from "../ui/button";

interface AppButtonProps {
  tooltip?: string;
  variant?:
    | "default"
    | "destructive"
    | "outline"
    | "secondary"
    | "ghost"
    | "link";
  size?: "default" | "sm" | "lg";
  disabled?: boolean;
  type?: "button" | "submit" | "reset";
  className?: string;
  children: React.ReactNode;
  role?: string;
  onClick?: () => void;
}

export function AppButton(props: AppButtonProps) {
  return props.tooltip ? (
    <TooltipProvider>
      <Tooltip>
        <TooltipTrigger asChild>
          <Button {...props}>{props.children}</Button>
        </TooltipTrigger>
        <TooltipContent>
          <p>{props.tooltip}</p>
        </TooltipContent>
      </Tooltip>
    </TooltipProvider>
  ) : (
    <Button {...props}>{props.children}</Button>
  );
}
