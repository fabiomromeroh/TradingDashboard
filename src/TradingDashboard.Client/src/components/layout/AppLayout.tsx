import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";
import TopBar from "./TopBar";
import { useAccountsQuery } from "@/features/account/hooks/useAccountsQuery";
import { useAppDispatch } from "@/store/hooks";
import { setAccounts, setConfigFilters } from "@/store/store";
import { useEffect } from "react";
import { useConfigFiltersQuery } from "@/features/users/hooks/useConfigFiltersQuery";

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
    <div className="flex bg-background text-foreground h-screen min-h-0 overflow-y-auto ">
      <Sidebar />
      <div className="flex flex-col flex-1 min-w-0 ">
        <TopBar />
        <main className="flex-1 min-h-0  p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
