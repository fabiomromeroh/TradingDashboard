export interface UserQuery {
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
  user: UserQuery;
}

export interface LoginUserCommand {
  email: string;
  password: string;
}

export interface LogoutUserCommand {
  accessToken: string;
}
