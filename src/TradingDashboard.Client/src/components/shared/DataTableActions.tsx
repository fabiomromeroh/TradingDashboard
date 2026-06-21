import { MoreHorizontal } from 'lucide-react';
import { Button } from '@/components/ui/button';
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';

export type DataTableAction<TEntity> = {
    label: string;
    onClick: (entity: TEntity) => void;
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
                <DropdownMenuItem onClick={() => navigator.clipboard.writeText(entity.id)}>
                    Copy ID
                </DropdownMenuItem>
                <DropdownMenuSeparator />
                <DropdownMenuItem>View details</DropdownMenuItem>
                {actions.map((action, index) => (
                    <DropdownMenuItem key={`${action.label}-${index}`} onClick={() => action.onClick(entity)}>
                        {action.label}
                    </DropdownMenuItem>
                ))}
                <DropdownMenuItem className="text-destructive">Delete</DropdownMenuItem>
            </DropdownMenuContent>
        </DropdownMenu>
    );
}