import { Trash2Icon } from "lucide-react";

import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogMedia,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { Button } from "@/components/ui/button";

type DeleteConfirmationProps = {
  label: string;
  message?: string;
  handleOnClick: () => void;
  disabled?: boolean;
  buttonType?: "button" | "Icon";
  variant?:
    | "default"
    | "destructive"
    | "outline"
    | "secondary"
    | "ghost"
    | "link";
  className?: string;
  confirmLabel?: string;
};

export function ConfirmDeleteButton(props: DeleteConfirmationProps) {
  return (
    <AlertDialog>
      <AlertDialogTrigger asChild>
        <Button
          className={props.className}
          disabled={props.disabled}
          variant={props.variant || "destructive"}
        >
          {props.buttonType === "Icon" ? <Trash2Icon /> : props.label}
        </Button>
      </AlertDialogTrigger>
      <AlertDialogContent size="sm">
        <AlertDialogHeader>
          <AlertDialogMedia className="bg-destructive/10 text-destructive dark:bg-destructive/20 dark:text-destructive">
            <Trash2Icon />
          </AlertDialogMedia>
          <AlertDialogTitle>
            Delete {props.confirmLabel || props.label}
          </AlertDialogTitle>
          <AlertDialogDescription>
            {props.message ||
              `Are you sure you want to delete this ${props.confirmLabel?.toLowerCase() || props.label.toLowerCase() || "record"}?`}
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel variant="outline">Cancel</AlertDialogCancel>
          <AlertDialogAction
            variant="destructive"
            onClick={props.handleOnClick}
          >
            Delete
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
