import { DataTable } from "@/components/shared/DataTable";
import type { AccountDto, AccountTableProps } from "../types/account.types";
import type { ColumnDef } from "@tanstack/react-table";
import { DataTableActions } from "@/components/shared/DataTableActions";
import { CreateAccountModal } from "./CreateAccountModal";
import { useState } from "react";
import { PlusIcon, RefreshCcw } from "lucide-react";
import { AppButton } from "@/components/shared/AppButton";
import { useDeleteAccountMutation } from "../hooks/useDeleteAccountMutation";
import { useSyncBrokerMutation } from "@/features/import/hooks/useSyncBrokerMutation";
import { toast } from "sonner";

export function AccountTable(props: AccountTableProps) {
  const { mutate: deleteAccount } = useDeleteAccountMutation();
  const { mutate: syncBroker } = useSyncBrokerMutation();
  const [loadingRows, setLoadingRows] = useState<Set<string>>(new Set());

  const handleImportTrades = async (account: AccountDto) => {
    if (account.importSourceType === "BrokerSync") {
      // Check if the account has broker credentials
      if (!account.brokerCredentials) {
        toast.error(
          `Account ${account.name} does not have broker credentials. Please set them up first.`,
        );
        return;
      }
      setLoadingRows((prev) => new Set(prev).add(account.id));

      await syncBroker({
        AccountId: account.id,
      })
        .then(() => {
          props.handleRefresh();
        })
        .finally(() => {
          setLoadingRows((prev) => {
            const next = new Set(prev);
            next.delete(account.id);
            return next;
          });
        });
    }
    if (account.importSourceType === "FileUpload") {
      // Handle file upload import logic here
    }
  };

  const handleDeleteAccount = (account: AccountDto) => {
    if (account.id) {
      deleteAccount(account.id).then((success) => {
        if (success) {
          props.handleRefresh();
        }
      });
    }
  };

  const columns: ColumnDef<AccountDto, unknown>[] = [
    { accessorKey: "name", header: "Account Name" },
    { accessorKey: "brokerName", header: "Broker" },
    { accessorKey: "tradesCount", header: "#Trades" },
    { accessorKey: "importSourceType", header: "Type" },
    {
      id: "actions",
      enableHiding: false,
      cell: ({ row }) => {
        const account = row.original;
        const tooltipText =
          account.importSourceType === "BrokerSync"
            ? "Sync now"
            : "Add manual trades";
        return (
          <>
            <AppButton
              variant="ghost"
              onClick={() => handleImportTrades(account)}
              tooltip={tooltipText}
              disabled={loadingRows.has(account.id)}
            >
              {account.importSourceType === "BrokerSync" ? (
                <RefreshCcw
                  className={`h-4 w-4 ${loadingRows.has(account.id) ? "animate-spin" : ""}`}
                />
              ) : (
                <PlusIcon className=" h-4 w-4" />
              )}
            </AppButton>

            <DataTableActions
              entity={account}
              actions={[
                {
                  label: "Import Trades",
                  onClick: () => handleImportTrades(account),
                },
                { label: "Edit", onClick: () => {} },
                {
                  label: "Delete",
                  className: "text-destructive",
                  needsConfirm: true,
                  needsConfirmButtonType: "button",
                  needsConfirmLabel: "Account",
                  buttonVariant: "ghost",

                  onClick: () => handleDeleteAccount(account),
                },
              ]}
            />
          </>
        );
      },
    },
  ];

  if (props.isLoading) {
    return (
      <p className="py-8 text-center text-muted-foreground">
        Loading accounts...
      </p>
    );
  }

  if (props.error) {
    return (
      <div className="py-8 text-center">
        <p className="text-red-600 mb-2">
          {props.error.map((e) => e.message).join(", ")}
        </p>
        <AppButton onClick={props.handleRefresh} className="text-sm underline">
          Try again
        </AppButton>
      </div>
    );
  }

  return (
    <DataTable
      columns={columns}
      data={props.accounts || []}
      toolbar={
        <CreateAccountModal handleOnAccountChange={props.handleRefresh} />
      }
    />
  );
}
