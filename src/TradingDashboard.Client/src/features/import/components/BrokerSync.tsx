import { AppInput } from "@/components/shared/AppInput";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { useEffect, useState } from "react";
import type { BrokerSyncProps } from "../types/import.types";
import { AppButton } from "@/components/shared/AppButton";

export function BrokerSync({
  brokerName,
  selectedAccount,
  accounts,
  onSaveCredentials,
}: BrokerSyncProps) {
  const [queryId, setQueryId] = useState<string>("");
  const [flexToken, setFlexToken] = useState<string>("");

  useEffect(() => {
    if (selectedAccount && selectedAccount !== "") {
      const account = accounts.find((x) => x.id === selectedAccount);
      if (account && account.brokerCredentials) {
        setQueryId(account.brokerCredentials.queryId);
        setFlexToken(account.brokerCredentials.token);
      }
    }
  }, [accounts, selectedAccount]);

  return (
    <Card>
      <CardContent>
        {brokerName === "Interactive Brokers" && (
          <div className="grid grid-cols-1 gap-4 max-w-md">
            <AppInput
              id="queryId"
              placeholder="Query ID"
              type="text"
              required={true}
              label="Query ID"
              value={queryId || ""}
              onChange={(e) => {
                setQueryId(e.target.value);
              }}
              autoComplete="off"
            />
            <AppInput
              id="flexToken"
              placeholder="Flex Token"
              type="password"
              required={true}
              label="Flex Token"
              value={flexToken || ""}
              onChange={(e) => {
                setFlexToken(e.target.value);
              }}
              autoComplete="new-password"
            />
          </div>
        )}
      </CardContent>
      <CardFooter>
        <AppButton
          className="primary"
          onClick={() => onSaveCredentials(queryId, flexToken)}
        >
          Save Credentials
        </AppButton>
      </CardFooter>
    </Card>
  );
}
