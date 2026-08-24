import { accountSlice } from "@/features/account/store/account.slice";
import { metricSlice } from "@/features/dashboard/store/metric.slice";
import { userSlice } from "@/features/users/store/user.slice";
import { createSlice, configureStore } from "@reduxjs/toolkit";

const themeSlice = createSlice({
  name: "theme",
  initialState: {
    value: (localStorage.getItem("theme") || "system") as
      | "light"
      | "dark"
      | "system",
  },
  reducers: {
    setTheme(state, action) {
      state.value = action.payload;
      localStorage.setItem("theme", action.payload);
    },
  },
});

const store = configureStore({
  reducer: {
    theme: themeSlice.reducer,
    account: accountSlice.reducer,
    auth: userSlice.reducer,
    metric: metricSlice.reducer,

    // ...other reducers
  },
});

export const { setTheme } = themeSlice.actions;
export const { setSelectedAccounts } = accountSlice.actions;
export const { setAccounts } = accountSlice.actions;
export const {
  logout,
  setAccessToken,
  setAuthCheckComplete,
  setUser,
  setConfigFilters,
  setConfigDashboard,
  loadUserConfig,
} = userSlice.actions;
export const { setMetrics } = metricSlice.actions;

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
