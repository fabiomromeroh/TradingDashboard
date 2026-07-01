import apiClient from "@/lib/apiClient";
import type { ConfirmImportCommand, UploadImport } from "../types/import.types";

export async function uploadImport(
  file: File,
  accountId?: string,
  brokerName?: string,
): Promise<UploadImport> {
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
  confirmImportCommand: ConfirmImportCommand,
): Promise<string> {
  return apiClient.post("/imports/confirm", confirmImportCommand);
}
