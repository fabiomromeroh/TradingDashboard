import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";
import TopBar from "./TopBar";
import { useAccountsQuery } from "@/features/account/hooks/useAccountsQuery";
import { useAppDispatch } from "@/store/hooks";
import { setAccounts, setSelectedAccounts } from "@/store/store";
import { useEffect } from "react";

export default function AppLayout() {
  const dispatch = useAppDispatch();

  const { accounts } = useAccountsQuery();

  useEffect(() => {
    if (accounts.length > 0) {
      dispatch(setAccounts(accounts));
      dispatch(setSelectedAccounts([accounts[0].id]));
    }
  }, [accounts, dispatch]);

  return (
    <div className="flex h-screen bg-background text-foreground">
      <Sidebar />
      <div className="flex flex-col flex-1 min-w-0 overflow-hidden">
        <TopBar />
        <main className="flex-1 min-h-0 overflow-hidden p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
