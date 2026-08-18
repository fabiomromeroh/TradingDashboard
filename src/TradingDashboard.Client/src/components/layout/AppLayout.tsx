import { Outlet } from "react-router-dom";
import TopBar from "./TopBar";
import { useAccountsQuery } from "@/features/account/hooks/useAccountsQuery";
import { useAppDispatch } from "@/store/hooks";
import { setAccounts, setConfigFilters } from "@/store/store";
import { useEffect } from "react";
import { useConfigFiltersQuery } from "@/features/users/hooks/useConfigFiltersQuery";
import { SidebarProvider, SidebarInset } from "@/components/ui/sidebar";
import { AppSidebar } from "./AppSidebar";

export default function AppLayout() {
  const dispatch = useAppDispatch();

  const { accounts } = useAccountsQuery();
  const { config } = useConfigFiltersQuery();

  useEffect(() => {
    if (accounts.length > 0) {
      dispatch(setAccounts(accounts));
    }

    if (config && config.filters) {
      dispatch(setConfigFilters(config.filters));
    }
  }, [accounts, dispatch, config]);

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
