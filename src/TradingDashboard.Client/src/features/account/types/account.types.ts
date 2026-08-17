export type AccountStatus = "active" | "inactive" | "pending";

import type { ApiError } from "@/types/api.types";

export interface AccountDto {
  id: string;
  name: string;
  brokerId: string;
  brokerName: string;
  userId: string;
  importSourceType: string;
  brokerCredentials?: any;
  tradesCount: number;
}

export interface CreateAccountCommand {
  name: string;
  brokerId: string;
  importSourceType: string;
  initialBalance: number;
}

export interface SetCredentialsCommand {
  brokerCredentials: {
    BrokerName: string;
    QueryId: string;
    Token: string;
  };
}

export interface AccountTableProps {
  accounts?: AccountDto[];
  isLoading?: boolean;
  error?: ApiError[] | null;
  handleRefresh: () => void;
  onAccountChange: () => void;
}

export interface CreateAccountModalProps {
  handleOnAccountChange: () => void;
}

export interface UseAccountsResult {
  accounts: AccountDto[];
  error: ApiError[] | null;
  isLoading: boolean;
  refetch: () => void;
}
