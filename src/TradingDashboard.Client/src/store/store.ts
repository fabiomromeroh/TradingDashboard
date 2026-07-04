import { accountSlice } from "@/features/account/store/accountSlice";
import { authSlice } from "@/features/auth/store/authSlice";
import { createSlice, configureStore } from "@reduxjs/toolkit";

const userSlice = createSlice({
  name: "user",
  initialState: {
    id: "",
    name: "John Doe",
    email: "",
  },
  reducers: {
    setUser(state, action) {
      state.id = action.payload.id;
      state.name = `${action.payload.firstName} ${action.payload.lastName}`;
      state.email = action.payload.email;
    },
  },
});

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
    user: userSlice.reducer,
    theme: themeSlice.reducer,
    account: accountSlice.reducer,
    auth: authSlice.reducer,

    // ...other reducers
  },
});

export const { setUser } = userSlice.actions;
export const { setTheme } = themeSlice.actions;
export const { setSelectedAccounts } = accountSlice.actions;
export const { setAccounts } = accountSlice.actions;
export const { setCredentials, logout } = authSlice.actions;

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
