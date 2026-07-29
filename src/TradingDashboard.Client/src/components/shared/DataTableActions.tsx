import { MoreHorizontal } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { ConfirmDeleteButton } from "./ConfirmDeleteButton";

export type DataTableAction<TEntity> = {
  label: string;
  onClick: (entity: TEntity) => void;
  className?: string;
  needsConfirm?: boolean;
  needsConfirmButtonType?: "button" | "Icon";
  needsConfirmLabel?: string;
  icon?: React.ReactNode;
  buttonVariant?:
    | "default"
    | "destructive"
    | "outline"
    | "secondary"
    | "ghost"
    | "link";
};

type DataTableActionsProps<TEntity extends { id: string }> = {
  entity: TEntity;
  actions?: DataTableAction<TEntity>[];
};

export function DataTableActions<TEntity extends { id: string }>({
  entity,
  actions = [],
}: DataTableActionsProps<TEntity>) {
  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" size="sm" className="h-8 w-8 p-0">
          <span className="sr-only">Open menu</span>
          <MoreHorizontal className="h-4 w-4" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <DropdownMenuLabel>Actions</DropdownMenuLabel>
        {actions.map((action, index) =>
          !action.needsConfirm ? (
            <DropdownMenuItem
              key={`${action.label}-${index}`}
              className={action.className}
              onClick={() => action.onClick(entity)}
            >
              {action.label}
            </DropdownMenuItem>
          ) : (
            <ConfirmDeleteButton
              key={`${action.label}-${index}`}
              label={action.label}
              handleOnClick={() => action.onClick(entity)}
              buttonType={action.needsConfirmButtonType}
              variant={action.buttonVariant}
              className={action.className}
              confirmLabel={action.needsConfirmLabel}
            />
          ),
        )}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
