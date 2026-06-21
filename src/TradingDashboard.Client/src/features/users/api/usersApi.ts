import apiClient from "@/lib/apiClient";
import type { CreateUserRequest, UserQuery } from "../types/user.type";

export async function getUsers(): Promise<UserQuery[]> {
    return apiClient.get('/users');
}

export async function createUser(user: CreateUserRequest): Promise<UserQuery> {
    return apiClient.post('/users/register', user);
}
