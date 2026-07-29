import apiClient from "../../../lib/apiClient"; // ← was: services/api
import type { ExecutionDto, TradeDto } from "../types/trade.types";

export async function getTradesByAccountId(
  accountIds: string[],
): Promise<TradeDto[]> {
  return apiClient.post(`/trades/accounts`, accountIds);
}

export async function getTradeById(id: string): Promise<TradeDto> {
  return apiClient.get(`/trades/${id}`);
}

export async function getTradeExecutions(id: string): Promise<ExecutionDto[]> {
  return apiClient.get(`/trades/${id}/executions`);
}

export async function closeTrade(
  id: string,
  exitPrice: number,
): Promise<TradeDto> {
  return apiClient.patch(`/trades/${id}/close`, { exitPrice });
}

export interface TradeStatusBadgeProps {
  status: string;
}
