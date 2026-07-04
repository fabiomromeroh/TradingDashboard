import { createSlice } from "@reduxjs/toolkit";

export const accountSlice = createSlice({
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
