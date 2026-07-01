import apiClient from "../../../lib/apiClient"; // ← was: services/api
import type { Trade } from "../types/trade.types";

export async function getTradesByAccountId(
  accountIds: string[],
): Promise<Trade[]> {
  return apiClient.post(`/trades/accounts`, accountIds);
}

export async function getTradeById(id: string): Promise<Trade> {
  return apiClient.get(`/trades/${id}`);
}

export async function closeTrade(
  id: string,
  exitPrice: number,
): Promise<Trade> {
  return apiClient.patch(`/trades/${id}/close`, { exitPrice });
}
