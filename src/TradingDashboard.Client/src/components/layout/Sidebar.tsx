import { NavLink } from "react-router-dom";
import {
  ArrowLeftRight,
  Briefcase,
  ChartBar,
  LayoutDashboardIcon,
  Plus,
  Users,
} from "lucide-react";
import { useAppSelector } from "@/store/hooks";
import { useLogoutUser } from "@/features/auth/hooks/useLogoutUser";
import { AvatarDropdown } from "../shared/AvatarDropdown";
import { useAuth } from "@/app/AuthContext";

export default function Sidebar() {
  const user = useAppSelector((x) => x.auth.user);
  const { logoutUser } = useLogoutUser();
  const { hasRole } = useAuth();

  const navItems = [
    { to: "/dashboard", icon: LayoutDashboardIcon, label: "Dashboard" },

    { to: "/trades", icon: ArrowLeftRight, label: "Trades" },
    { to: "/accounts", icon: Briefcase, label: "Accounts" },
    { to: "/reports", icon: ChartBar, label: "Reports" },
  ];

  if (hasRole("Admin")) {
    navItems.push({ to: "/users", icon: Users, label: "Users" });
  }

  return (
    <aside className="h-screen bg-sidebar flex flex-col">
      {/* Logo */}
      <div className="px-5 py-4 border-b border-sidebar-border">
        <span className="text-sidebar-foreground font-bold text-base tracking-tight">
          Trading Dashboard
        </span>
      </div>

      <nav className="flex mt-5 px-3 py-1 flex-col">
        <NavLink
          className="flex items-center gap-3 px-3 py-2 rounded-md text-sm font-medium transition-colors bg-sidebar-primary text-sidebar-primary-foreground"
          key="/add-trades"
          to="/add-trades"
        >
          <Plus className="text-current" size={16} />
          Add Trades
        </NavLink>
      </nav>

      {/* Nav */}
      <nav className="flex-1 px-3 py-4">
        {navItems.map(({ to, icon: Icon, label }) => (
          <NavLink
            key={to}
            to={to}
            end={to === "/"}
            className={({ isActive }) =>
              `flex items-center gap-3 px-3 py-2 rounded-md text-sm font-medium transition-colors p-2 ${
                isActive
                  ? "bg-sidebar-primary text-sidebar-primary-foreground"
                  : "text-muted-foreground hover:bg-sidebar-accent/20 hover:text-sidebar-foreground"
              }`
            }
          >
            <Icon className="text-current" size={16} />
            {label}
          </NavLink>
        ))}
      </nav>

      {/* Footer */}
      <div className="px-5 py-4 items-center flex justify-between  mr-auto gap-2">
        <AvatarDropdown onClick={logoutUser} />

        {user?.fullName}
      </div>
    </aside>
  );
}
