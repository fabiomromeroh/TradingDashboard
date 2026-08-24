import type {
  DashboardConfig,
  ConfigFiltersDto,
  UserDto,
} from "@/features/users/types/user.types";
import { createSlice } from "@reduxjs/toolkit";

interface AuthState {
  accessToken: string | null;
  authCheckComplete: boolean;
  user: UserDto | null;
  isAuthenticated: boolean;
  filtersConfig: ConfigFiltersDto;
  dashboardConfig: DashboardConfig[];
}

interface ConfigFilterState {
  accountIds: string[];
}

const initialState: AuthState = {
  accessToken: null,
  authCheckComplete: false,
  user: null,
  isAuthenticated: false,
  filtersConfig: { accountIds: [] },
  dashboardConfig: [],
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
      state.filtersConfig = { ...state.filtersConfig, ...action.payload };
    },
    setConfigDashboard(state, action: { payload: DashboardConfig[] }) {
      state.dashboardConfig = [...action.payload];
    },
    loadUserConfig(
      state,
      action: {
        payload: {
          configs: Array<{ configType: string; filters?: any; widgets?: any }>;
        };
      },
    ) {
      const configs = action.payload.configs;

      const filtersConfig = configs.find((c) => c.configType === "filters");

      if (filtersConfig && filtersConfig.filters) {
        state.filtersConfig = {
          accountIds: filtersConfig.filters.accountIds || [],
          dateFrom: filtersConfig.filters.dateFrom,
          dateTo: filtersConfig.filters.dateTo,
        };
      }

      const dashboardConfig = configs.find((c) => c.configType === "dashboard");

      if (dashboardConfig && dashboardConfig.widgets) {
        state.dashboardConfig = dashboardConfig.widgets;
      }
    },
  },
});
