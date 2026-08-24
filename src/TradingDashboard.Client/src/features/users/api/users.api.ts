import apiClient from "@/lib/apiClient";
import type {
  ConfigDashboardCommand,
  ConfigFiltersCommand,
  CreateUserCommand,
  LoginResponse,
  LoginUserCommand,
  UserDto,
} from "../types/user.types";

export async function getUsers(): Promise<UserDto[]> {
  return apiClient.get("/users");
}

export async function createUser(user: CreateUserCommand): Promise<void> {
  return apiClient.post("/users/register", user);
}

export async function login(
  loginUserCommand: LoginUserCommand,
): Promise<LoginResponse> {
  return apiClient.post("/users/login", loginUserCommand);
}

export async function logout(): Promise<void> {
  return apiClient.post("/users/logout");
}

export async function updateConfigFilters(
  filters: ConfigFiltersCommand,
): Promise<void> {
  return apiClient.post("/users/config", {
    ConfigType: "filters",
    Config: filters,
  });
}

export async function updateConfigDashboard(
  dashboardLayout: ConfigDashboardCommand[],
): Promise<void> {
  return apiClient.post("/users/config", {
    ConfigType: "dashboard",
    Config: dashboardLayout,
  });
}

export async function getUserConfig(): Promise<any> {
  return apiClient.get("/users/config");
}
