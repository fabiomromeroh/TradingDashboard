


export interface UserQuery {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean;
  avatarUrl?: string;
}

export interface CreateUserRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}


