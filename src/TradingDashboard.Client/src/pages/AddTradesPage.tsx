import { useEffect, useState } from "react";
import { toSelectOptions } from "@/lib/utils";
import { useSetBrokerCredentialsMutation } from "@/features/account/hooks/setBrokerCredentialsMutation";
import { useLocation } from "react-router-dom";
import { Separator } from "@/components/ui/separator";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ImportHistoryTable } from "@/features/import/components/ImportHistoryTable";
import { ImportUpload } from "@/features/import";
import { useImportHistoryQuery } from "@/features/import/hooks/useImportHistoryQuery";
import { BrokerSync } from "@/features/import/components/BrokerSync";
import { AccountTable } from "@/features/account/components/AccountTable";
import { Card, CardContent } from "@/components/ui/card";
import { useAccountsQuery } from "@/features/account/hooks/useAccountsQuery";
import { Label } from "@/components/ui/label";
import { AppSelect } from "@/components/shared/AppSelect";
import { toast } from "sonner";
import { setAccounts } from "@/store/store";
import { useAppDispatch } from "@/store/hooks";

type ImportPageState = {
  accountId?: string;
};

export function AddTradesPage() {
  const dispatch = useAppDispatch();

  const {
    accounts,
    refetch: refetchAccounts,
    isLoading: isAccountsLoading,
    error: accountsError,
  } = useAccountsQuery();

  useEffect(() => {
    if (accounts.length > 0) {
      console.log("Layout useEffect triggered");
      dispatch(setAccounts(accounts));
    }
  }, [accounts, dispatch]);

  const location = useLocation();
  const importState = (location.state as ImportPageState | null) ?? {};
  const [selectedAccount, setSelectedAccount] = useState<string>(
    importState.accountId ?? "",
  );
  const { mutate: saveCredentials } = useSetBrokerCredentialsMutation();
  const [brokerName, setBrokerName] = useState<string>("");

  const {
    importHistory,
    refetch: refetchImportHistory,
    isLoading: isLoadingImportHistory,
  } = useImportHistoryQuery(selectedAccount);

  const handleSaveCredentials = (QueryId: string, Token: string) => {
    if (QueryId === "" || Token === "") {
      return toast.error("QueryId and Token cannot be empty");
    }
    saveCredentials(selectedAccount, {
      brokerCredentials: { BrokerName: brokerName, QueryId, Token },
    }).then(() => {
      refetchAccounts();
    });
  };

  const handleSelectedAccountChange = (value: string) => {
    setSelectedAccount(value);
    setBrokerName(accounts.find((x) => x.id === value)?.brokerName ?? "");
  };

  const handleRefresh = () => {
    refetchImportHistory();
    refetchAccounts();
  };

  const accountOptions = toSelectOptions(accounts);

  return (
    <div className="flex h-full  min-h-0 flex-col overflow-hidden">
      <div className="flex-1 min-h-0 overflow-y-auto">
        <Card>
          <CardContent>
            <AccountTable
              accounts={accounts}
              handleRefresh={handleRefresh}
              onAccountChange={refetchAccounts}
              isLoading={isAccountsLoading}
              error={accountsError}
            />
          </CardContent>
        </Card>
        <Separator className="my-4" />
        <Card>
          <CardContent>
            <div className="grid grid-cols-1 gap-4 max-w-md mb-5">
              <Label htmlFor="brokerSelect">Accounts </Label>
              <AppSelect
                name="accountSelect"
                options={accountOptions}
                value={selectedAccount}
                placeholder="Select an account to add trades.."
                onChange={(value) => {
                  handleSelectedAccountChange(value);
                }}
                className="w-full"
                groupLabel="Account"
              />
            </div>
            <Tabs defaultValue="importFile">
              <TabsList>
                <TabsTrigger disabled={!selectedAccount} value="importFile">
                  File Upload
                </TabsTrigger>
                <TabsTrigger disabled={!brokerName} value="brokerSync">
                  Broker Sync
                </TabsTrigger>
                <TabsTrigger disabled={true} value="manualAdd">
                  Manual
                </TabsTrigger>
              </TabsList>
              <TabsContent value="importFile">
                <ImportUpload
                  brokerName={brokerName}
                  selectedAccount={selectedAccount}
                  onImportCompleted={handleRefresh}
                />
              </TabsContent>
              <TabsContent value="brokerSync">
                <BrokerSync
                  brokerName={brokerName}
                  selectedAccount={selectedAccount}
                  onSaveCredentials={handleSaveCredentials}
                  accounts={accounts}
                />
              </TabsContent>
              <TabsContent value="manualAdd"></TabsContent>
            </Tabs>
          </CardContent>
        </Card>
        <Separator className="my-4" />
        <ImportHistoryTable
          accountId={selectedAccount}
          isLoading={isLoadingImportHistory}
          importHistory={importHistory}
          onRollbackCompleted={handleRefresh}
        />
      </div>
    </div>
  );
}
