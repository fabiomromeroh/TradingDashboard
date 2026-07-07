import apiClient from "@/lib/apiClient";
import type {
  AccountQuery,
  CreateAccountCommand,
} from "../types/account.types";

export async function getAccounts(userId: string): Promise<AccountQuery[]> {
  return apiClient.get(`/accounts/user/${userId}`);
}

export async function createAccount(accountData: CreateAccountCommand) {
  return apiClient.post("/accounts", accountData);
}
