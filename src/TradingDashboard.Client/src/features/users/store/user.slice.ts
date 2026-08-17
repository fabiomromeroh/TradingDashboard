import type {
  ConfigFiltersDto,
  UserDto,
} from "@/features/users/types/user.types";
import { createSlice } from "@reduxjs/toolkit";

interface AuthState {
  accessToken: string | null;
  authCheckComplete: boolean;
  user: UserDto | null;
  isAuthenticated: boolean;
  configFilters: ConfigFiltersDto;
}

interface ConfigFilterState {
  accountIds: string[];
}

const initialState: AuthState = {
  accessToken: null,
  authCheckComplete: false,
  user: null,
  isAuthenticated: false,
  configFilters: { accountIds: [] },
};

type PatchFiltersPayload = Partial<ConfigFilterState>;

export const userSlice = createSlice({
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
    setConfigFilters(state, action: { payload: PatchFiltersPayload }) {
      state.configFilters = { ...state.configFilters, ...action.payload };
    },
  },
});
