import apiClient from '../../../lib/apiClient';  // ← was: services/api
import type { Trade } from '../types/trade.types';

export async function getTrades(): Promise<Trade[]> {
  return apiClient.get('/trades');
}

export async function getTradeById(id: string): Promise<Trade> {
  return apiClient.get(`/trades/${id}`);
}

export async function closeTrade(id: string, exitPrice: number): Promise<Trade> {
  return apiClient.patch(`/trades/${id}/close`, { exitPrice });
}