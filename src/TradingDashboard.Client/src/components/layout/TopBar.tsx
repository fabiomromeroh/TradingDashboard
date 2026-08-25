import { useLocation } from "react-router-dom";
import { ThemeToggle } from "../shared/ThemeToggle";
import { FilterZone } from "./FilterZone";
import { SidebarTrigger } from "../ui/sidebar";

const pageTitles: Record<string, string> = {
  "/dashboard": "Dashboard",
  "/users": "Users",
  "/trades": "Trades",
  "/accounts": "Accounts",
  "/add-trades": "Add Trade",
  "/reports": "Reports",
  "/trades/detail": "Trade Detail",
};

export default function TopBar() {
  const { pathname } = useLocation();

  const title = pageTitles[pathname] ?? "";

  return (
    <header className="h-13 border-b border-border bg-card flex items-center justify-between px-6 shrink-0 text-foreground">
      <SidebarTrigger className="-ml-1" />

      <h3 className="text-sm font-semibold text-foreground">{title}</h3>
      <div className="flex items-center justify-end gap-2 ml-auto mr-6">
        <FilterZone />
        <ThemeToggle />
      </div>
    </header>
  );
}
