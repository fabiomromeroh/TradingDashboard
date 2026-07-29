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
