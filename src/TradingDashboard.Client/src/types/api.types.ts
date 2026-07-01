// Shapes your ASP.NET Core API returns on every response
export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface ApiError {
  message: string;
  errors?: Record<string, string[]>; // validation errors from FluentValidation
}

export type SelectOption = {
  value: string;
  label: string;
};

export type ApiOption = {
  id: string;
  name: string;
};

export interface ApiResponse<T> {
  isSuccess: boolean;
  isFailure: boolean;
  statusCode: string;
  value: T[] | T | null;
  errors: [];
}
