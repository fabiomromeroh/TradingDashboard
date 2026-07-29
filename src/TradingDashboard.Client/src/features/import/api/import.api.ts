import apiClient from "@/lib/apiClient";
import type {
  ConfirmImportCommand,
  ImportHistoryDto,
  SyncBrokerCommand,
  SyncBrokerDto,
  UploadImportDto,
} from "../types/import.types";

export async function uploadImport(
  file: File,
  accountId?: string,
  brokerName?: string,
): Promise<UploadImportDto> {
  const formData = new FormData();
  formData.append("file", file);
  formData.append("brokerName", brokerName || "");
  formData.append("accountId", accountId || "");

  return apiClient.post("/imports/upload", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
}

export async function confirmImport(
  command: ConfirmImportCommand,
): Promise<string> {
  return apiClient.post("/imports/confirm", command);
}

export async function getImportHistory(
  accountId: string,
): Promise<ImportHistoryDto[]> {
  return apiClient.get(`/imports/account/${accountId}`);
}

export async function rollbackImport(id: string): Promise<void> {
  return apiClient.post(`/imports/rollback/${id}`);
}

export async function syncBroker(
  syncBrokerCommand: SyncBrokerCommand,
): Promise<SyncBrokerDto> {
  return apiClient.post("/imports/sync-broker", syncBrokerCommand);
}
