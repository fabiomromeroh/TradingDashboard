import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";
import TopBar from "./TopBar";
import { useAccounts } from "@/features/account/hooks/useAccounts";
import { useAppDispatch } from "@/store/hooks";
import { setAccounts, setSelectedAccounts, setUser } from "@/store/store";
import { useEffect } from "react";
import { useUsers } from "@/features/users";

export default function AppLayout() {
  const dispatch = useAppDispatch();
  const { users } = useUsers();

  const { accounts } = useAccounts();

  useEffect(() => {
    if (users.length > 0) {
      dispatch(setUser(users[0]));
    }
    if (accounts.length > 0) {
      dispatch(setAccounts(accounts));
      dispatch(setSelectedAccounts([accounts[0].id]));
    }
  }, [accounts, users, dispatch]);

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
