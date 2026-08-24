import { Outlet } from "react-router-dom";
import TopBar from "./TopBar";
import { useAccountsQuery } from "@/features/account/hooks/useAccountsQuery";
import { useAppDispatch } from "@/store/hooks";
import { setAccounts } from "@/store/store";
import { useEffect } from "react";
import { useConfigQuery } from "@/features/users/hooks/useConfigQuery";
import { SidebarProvider, SidebarInset } from "@/components/ui/sidebar";
import { AppSidebar } from "./AppSidebar";

export default function AppLayout() {
  const dispatch = useAppDispatch();

  const { accounts } = useAccountsQuery();
  useConfigQuery();

  useEffect(() => {
    if (accounts.length > 0) {
      dispatch(setAccounts(accounts));
    }
  }, [accounts, dispatch]);

  return (
    <SidebarProvider className="bg-background text-foreground">
      <AppSidebar />
      <SidebarInset className="min-w-0">
        <TopBar />
        <main className="flex-1 min-h-0 overflow-y-auto p-6">
          <Outlet />
        </main>
      </SidebarInset>
    </SidebarProvider>
  );
}
