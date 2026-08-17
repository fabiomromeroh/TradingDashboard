import apiClient from "@/lib/apiClient";
import type {
  ConfigDto,
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
  return apiClient.post("/config", { filters });
}

export async function getConfigFilters(): Promise<ConfigDto> {
  return apiClient.get("/config");
}
