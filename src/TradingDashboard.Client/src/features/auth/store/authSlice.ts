import { createSlice, type PayloadAction } from "@reduxjs/toolkit";

interface AuthState {
  accessToken: string | null;
  user: { id: string; email: string } | null;
}

const initialState: AuthState = {
  accessToken: null,
  user: null,
};

export const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setCredentials(state, action: PayloadAction<AuthState>) {
      state.accessToken = action.payload.accessToken;
      state.user = action.payload.user;
    },
    logout: (state) => {
      state.accessToken = null;
      state.user = null;
    },
  },
});
