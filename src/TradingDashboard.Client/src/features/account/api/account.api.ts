import apiClient from "@/lib/apiClient";
import type {
  AccountDto,
  CreateAccountCommand,
  SetCredentialsCommand,
} from "../types/account.types";

export async function getAccounts(userId: string): Promise<AccountDto[]> {
  return await apiClient.get(`/accounts/user/${userId}`);
}

export async function createAccount(
  command: CreateAccountCommand,
): Promise<void> {
  return await apiClient.post("/accounts", command);
}

export async function setBrokerCredentials(
  accountId: string,
  credentials: SetCredentialsCommand,
): Promise<void> {
  return await apiClient.put(`/accounts/credentials/${accountId}`, credentials);
}

export async function deleteAccount(accountId: string): Promise<void> {
  return await apiClient.delete(`/accounts/${accountId}`);
}
