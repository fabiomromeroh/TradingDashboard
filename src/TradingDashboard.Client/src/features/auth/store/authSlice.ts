import type { UserQuery } from "@/features/users";
import { createSlice } from "@reduxjs/toolkit";

interface AuthState {
  accessToken: string | null;
  authCheckComplete: boolean;
  user: UserQuery | null;
  isAuthenticated: boolean;
}

const initialState: AuthState = {
  accessToken: null,
  authCheckComplete: false,
  user: null,
  isAuthenticated: false,
};

export const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setAccessToken(state, action) {
      state.accessToken = action.payload;
      state.isAuthenticated = !!state.accessToken;
    },
    setAuthCheckComplete(state, action) {
      state.authCheckComplete = action.payload;
    },
    logout: (state) => {
      state.accessToken = null;
      state.authCheckComplete = true;
      state.isAuthenticated = false;
    },
    setUser(state, action) {
      state.user = {
        ...action.payload,
        fullName: `${action.payload.firstName} ${action.payload.lastName}`,
      };
    },
  },
});
