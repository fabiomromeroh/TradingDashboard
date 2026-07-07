import type { ColumnDef } from "@tanstack/react-table";
import type { UserQuery } from "../types/user.types";
import { CreateUserModal } from "./CreateUserModal";
import { useUsersQuery } from "../hooks/useUsersQuery";
import { DataTable } from "@/components/shared/DataTable";
import { DataTableActions } from "@/components/shared/DataTableActions";

const columns: ColumnDef<UserQuery, unknown>[] = [
  { accessorKey: "firstName", header: "First Name" },
  { accessorKey: "lastName", header: "Last Name" },
  { accessorKey: "email", header: "Email" },
  {
    accessorKey: "isActive",
    header: "Status",
    cell: ({ getValue }) => {
      const active = getValue() as boolean;
      return (
        <span
          className={`inline-flex items-center rounded-full px-2 py-0.5 text-xs font-medium ${
            active
              ? "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400"
              : "bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400"
          }`}
        >
          {active ? "Active" : "Inactive"}
        </span>
      );
    },
  },
  {
    id: "actions",
    enableHiding: false,
    cell: ({ row }) => {
      const user = row.original;
      return (
        <DataTableActions
          entity={user}
          actions={[
            { label: "Import", onClick: () => {} },
            { label: "Reset password", onClick: () => {} },
          ]}
        />
      );
    },
  },
];

export function UserTable() {
  const { data: users, error, isLoading, refetch } = useUsersQuery();

  if (isLoading) {
    return (
      <p className="py-8 text-center text-muted-foreground">Loading users…</p>
    );
  }

  if (error && error.length > 0) {
    return (
      <div className="py-8 text-center">
        <p className="text-red-600 mb-2">{error[0].message}</p>
        <button onClick={refetch} className="text-sm underline">
          Try again
        </button>
      </div>
    );
  }

  return (
    <DataTable
      columns={columns}
      data={users}
      filterPlaceholder="Filter users..."
      toolbar={<CreateUserModal reload={refetch} />}
    />
  );
}
