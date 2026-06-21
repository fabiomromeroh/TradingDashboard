// Shapes your ASP.NET Core API returns on every response
export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface ApiError {
  message: string;
  errors?: Record<string, string[]>;  // validation errors from FluentValidation
}