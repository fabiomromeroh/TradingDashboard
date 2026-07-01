import apiClient from "@/lib/apiClient";
import type {
  AccountQuery,
  CreateAccountCommand,
} from "../types/account.types";

export async function getAccounts(): Promise<AccountQuery[]> {
  return apiClient.get("/accounts/user/dd7e0338-d43d-4f24-a274-22bbf194dc3e");
}

export async function createAccount(accountData: CreateAccountCommand) {
  return apiClient.post("/accounts", accountData);
}
