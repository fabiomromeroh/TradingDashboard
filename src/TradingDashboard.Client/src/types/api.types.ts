// Shapes your ASP.NET Core API returns on every response
export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export type SelectOption = {
  value: string;
  label: string;
};

export type ApiOption = {
  id: string;
  name: string;
};

export interface ApiError {
  code: string;
  message: string;
}

export interface ApiErrorResponse {
  status: string;
  title: string;
  errors: ApiError[];
}
