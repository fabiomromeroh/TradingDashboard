import { createSlice } from "@reduxjs/toolkit";
import type { AccountDto } from "../types/account.types";

export const accountSlice = createSlice({
  name: "account",
  initialState: {
    selectedAccounts: [],
    accounts: [] as AccountDto[],
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
