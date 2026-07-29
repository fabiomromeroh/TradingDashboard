import apiClient from "@/lib/apiClient";
import type {
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
