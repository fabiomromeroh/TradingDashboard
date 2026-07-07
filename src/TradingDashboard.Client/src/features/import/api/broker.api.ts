import apiClient from "@/lib/apiClient";
import type { Broker } from "../types/broker.types";

export async function getBrokers(): Promise<Broker[]> {
  return apiClient.get("/brokers");
}
