import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";
import TopBar from "./TopBar";
import { useAccountsQuery } from "@/features/account/hooks/useAccountsQuery";
import { useAppDispatch } from "@/store/hooks";
import { setAccounts, setSelectedAccounts } from "@/store/store";
import { useEffect } from "react";
import { TooltipProvider } from "../ui/tooltip";

export default function AppLayout() {
  const dispatch = useAppDispatch();

  const { accounts } = useAccountsQuery();

  useEffect(() => {
    if (accounts.length > 0) {
      console.log("Layout useEffect triggered");

      dispatch(setAccounts(accounts));
      dispatch(setSelectedAccounts([accounts[0].id]));
    }
  }, [accounts, dispatch]);

  return (
    <div className="flex bg-background text-foreground h-screen min-h-0 overflow-hidden">
      <Sidebar />
      <div className="flex flex-col flex-1 min-w-0 ">
        <TopBar />
        <main className="flex-1 min-h-0  p-6">
          <TooltipProvider>
            <Outlet />
          </TooltipProvider>
        </main>
      </div>
    </div>
  );
}
