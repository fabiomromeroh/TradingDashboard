import type { ApiError } from "@/types/api.types";

export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  fullName?: string;
  email: string;
  isActive: boolean;
  avatarUrl?: string;
}

export interface CreateUserCommand {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  user: UserDto;
}

export interface LoginUserCommand {
  email: string;
  password: string;
}

export interface LogoutUserCommand {
  accessToken: string;
}

export interface ConfigFiltersCommand {
  AccountIds: string[];
}

export interface ConfigDto {
  filters: ConfigFiltersDto;
}

export interface ConfigFiltersDto {
  accountIds: string[];
}

export interface ConfigFiltersQueryResult {
  config: ConfigDto | null;
  isLoading: boolean;
  error: ApiError[] | null;
  refetch: () => Promise<ConfigDto | null>;
}
