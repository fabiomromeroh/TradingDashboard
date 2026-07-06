import { accountSlice } from "@/features/account/store/accountSlice";
import { authSlice } from "@/features/auth/store/authSlice";
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
    auth: authSlice.reducer,

    // ...other reducers
  },
});

export const { setTheme } = themeSlice.actions;
export const { setSelectedAccounts } = accountSlice.actions;
export const { setAccounts } = accountSlice.actions;
export const { logout, setAccessToken, setAuthCheckComplete, setUser } =
  authSlice.actions;

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
