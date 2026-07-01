import { createSlice, configureStore } from "@reduxjs/toolkit";

const userSlice = createSlice({
  name: "user",
  initialState: {
    name: "John Doe",
    email: "",
  },
  reducers: {
    setUser(state, action) {
      state.name = action.payload.name;
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

const accountSlice = createSlice({
  name: "account",
  initialState: {
    selectedAccounts: [],
    accounts: [],
  },
  reducers: {
    setSelectedAccounts(state, action) {
      state.selectedAccounts = action.payload;
    },
    setAccounts(state, action) {
      state.accounts = action.payload;
    },
  },
});

const store = configureStore({
  reducer: {
    user: userSlice.reducer,
    theme: themeSlice.reducer,
    account: accountSlice.reducer,
    // ...other reducers
  },
});

export const { setUser } = userSlice.actions;
export const { setTheme } = themeSlice.actions;
export const { setSelectedAccounts } = accountSlice.actions;
export const { setAccounts } = accountSlice.actions;

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
