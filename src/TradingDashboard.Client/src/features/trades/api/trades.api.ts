import apiClient from "../../../lib/apiClient"; // ← was: services/api
import type {
  CreateTradeEventCommand,
  ExecutionDto,
  TradeDto,
  TradeEventDto,
} from "../types/trade.types";

export async function getTrades(): Promise<TradeDto[]> {
  return apiClient.get(`/trades`);
}

export interface PaginatedTradesResponse {
  items: TradeDto[];
  nextCursor: string | null;
  totalCount: number;
}

export async function getTradesPaginated({
  pageSize = 100,
  cursor = "",
}: {
  pageSize?: number;
  cursor?: string;
}): Promise<PaginatedTradesResponse> {
  return apiClient.get(
    `/trades/paginated?pageSize=${pageSize}&cursor=${cursor}`,
  );
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

// Not called yet — useTradeEvents currently seeds dummy data; swap it to call
// these once the backend endpoints exist.
export async function getTradeEvents(
  tradeId: string,
): Promise<TradeEventDto[]> {
  return apiClient.get(`/trades/${tradeId}/events`);
}

export async function createTradeEvent(
  tradeId: string,
  command: CreateTradeEventCommand,
): Promise<TradeEventDto> {
  return apiClient.post(`/trades/${tradeId}/events`, command);
}

export interface TradeStatusBadgeProps {
  status: string;
}
