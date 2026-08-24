import type { ApiError } from "@/types/api.types";

import type {
  WidgetType,
  WidgetZone,
} from "@/features/dashboard/types/dashboard.types";
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
  accountIds: string[];
  dateFrom?: string;
  dateTo?: string;
}

export interface ConfigDashboardCommand {
  zone: string;
  type: string;
}

export interface DashboardConfig {
  zone: WidgetZone;
  type: WidgetType;
}
export interface ConfigFiltersDto {
  accountIds: string[];
  dateFrom?: string;
  dateTo?: string;
}

export interface ConfigQueryResult {
  config: any | null;
  isLoading: boolean;
  error: ApiError[] | null;
  refetch: () => Promise<any | null>;
}
