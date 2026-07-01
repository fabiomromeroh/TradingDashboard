export type AccountStatus = "active" | "inactive" | "pending";

export interface AccountQuery {
  id: string;
  name: string;
  brokerId: string;
  brokerName: string;
  userId: string;
  currency: string;
}

export interface CreateAccountCommand {
  name: string;
  brokerId: string;
  currency: string;
  initialBalance: number;
}
