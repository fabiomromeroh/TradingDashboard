import apiClient from "@/lib/apiClient";
import type { Account } from "../types/account.types";

export async function getAccounts(): Promise<Account[]> {
  return apiClient.get('/accounts/user/dd7e0338-d43d-4f24-a274-22bbf194dc3e');
}