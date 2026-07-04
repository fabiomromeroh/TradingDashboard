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
  disabled: boolean;
};

export function ConfirmDeleteButton(props: DeleteConfirmationProps) {
  return (
    <AlertDialog>
      <AlertDialogTrigger asChild>
        <Button disabled={props.disabled} variant="destructive">
          <Trash2Icon />
        </Button>
      </AlertDialogTrigger>
      <AlertDialogContent size="sm">
        <AlertDialogHeader>
          <AlertDialogMedia className="bg-destructive/10 text-destructive dark:bg-destructive/20 dark:text-destructive">
            <Trash2Icon />
          </AlertDialogMedia>
          <AlertDialogTitle>Delete {props.label}</AlertDialogTitle>
          <AlertDialogDescription>
            {props.message ||
              `Are you sure you want to delete this ${props.label.toLowerCase() || "record"}?`}
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
