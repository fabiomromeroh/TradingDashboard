import apiClient from "@/lib/apiClient";
import type {
  CreateUserCommand,
  LoginResponse,
  LoginUserCommand,
  UserQuery,
} from "../types/user.type";

export async function getUsers(): Promise<UserQuery[]> {
  return apiClient.get("/users");
}

export async function createUser(user: CreateUserCommand) {
  return apiClient.post("/users/register", user);
}

export async function loginUserApi(
  loginUserCommand: LoginUserCommand,
): Promise<LoginResponse> {
  return apiClient.post("/users/login", loginUserCommand);
}

export async function logoutUserApi(): Promise<void> {
  return apiClient.post("/users/logout");
}
