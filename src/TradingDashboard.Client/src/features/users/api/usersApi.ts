import apiClient from "@/lib/apiClient";
import type { CreateUserCommand, UserQuery } from "../types/user.type";

export async function getUsers(): Promise<UserQuery[]> {
  return apiClient.get("/users");
}

export async function createUser(user: CreateUserCommand) {
  return apiClient.post("/users/register", user);
}
