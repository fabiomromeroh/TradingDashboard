import { DataTable } from "@/components/shared/DataTable";
import type { AccountQuery } from "../types/account.types";
import type { ColumnDef } from "@tanstack/react-table";
import { DataTableActions } from "@/components/shared/DataTableActions";
import { useAccounts } from "../hooks/useAccounts";
import { CreateAccountModal } from "./CreateAccountModal";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { setAccounts } from "@/store/store";
import { useAppDispatch } from "@/store/hooks";

export function AccountTable() {
  const dispatch = useAppDispatch();
  const { accounts, isLoading, error, refetch } = useAccounts();

  useEffect(() => {
    if (accounts.length > 0) {
      dispatch(setAccounts(accounts));
    }
  }, [accounts, dispatch]);

  const navigate = useNavigate();

  const importTrades = (account: AccountQuery) => {
    const accountId = account.id ?? account.name;
    const brokerName = account.brokerName ?? "";
    const accountName = account.name ?? "";

    navigate("/import", {
      state: {
        accountId,
        accountName,
        brokerName,
      },
    });
  };

  const columns: ColumnDef<AccountQuery, unknown>[] = [
    { accessorKey: "name", header: "Account Name" },
    { accessorKey: "brokerName", header: "Broker" },
    { accessorKey: "currency", header: "Currency" },
    { accessorKey: "Trades", header: "Trades" },
    {
      id: "actions",
      enableHiding: false,
      cell: ({ row }) => {
        const account = row.original;

        return (
          <DataTableActions
            entity={account}
            actions={[
              {
                label: "Import Trades",
                onClick: () => importTrades(account),
              },
              { label: "Edit", onClick: () => {} },
            ]}
          />
        );
      },
    },
  ];

  if (isLoading) {
    return (
      <p className="py-8 text-center text-muted-foreground">
        Loading accounts...
      </p>
    );
  }

  if (error) {
    return (
      <div className="py-8 text-center">
        <p className="text-red-600 mb-2">{error}</p>
        <button onClick={refetch} className="text-sm underline">
          Try again
        </button>
      </div>
    );
  }

  return (
    <DataTable
      columns={columns}
      data={accounts}
      showFilter={false}
      toolbar={<CreateAccountModal handleOnAccountChange={refetch} />}
    />
  );
}
