import { NavLink, useLocation } from "react-router-dom";
import {
  ArrowLeftRight,
  ChartBar,
  LayoutDashboardIcon,
  Plus,
  Users,
} from "lucide-react";
import { useAppSelector } from "@/store/hooks";
import { useLogoutMutation } from "@/features/users/hooks/useLogoutMutation";
import { useAuth } from "@/app/AuthContext";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupLabel,
  SidebarGroupContent,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarRail,
} from "@/components/ui/sidebar";
import { NavUser } from "./NavUser";

type NavItem = {
  to: string;
  icon: React.ComponentType<{ className?: string; size?: number }>;
  label: string;
};

type NavSection = {
  title: string;
  items: NavItem[];
};

export function AppSidebar() {
  const user = useAppSelector((x) => x.auth.user);
  const { mutate: logoutUser } = useLogoutMutation();
  const { hasRole } = useAuth();
  const location = useLocation();

  const navSections: NavSection[] = [
    {
      title: "Overview",
      items: [
        { to: "/dashboard", icon: LayoutDashboardIcon, label: "Dashboard" },
      ],
    },
    {
      title: "Journal",
      items: [
        { to: "/trades", icon: ArrowLeftRight, label: "Trades" },
        { to: "/reports", icon: ChartBar, label: "Reports" },
      ],
    },
  ];

  if (hasRole("Admin")) {
    navSections.push({
      title: "Admin",
      items: [{ to: "/users", icon: Users, label: "Users" }],
    });
  }

  return (
    <Sidebar collapsible="icon">
      <SidebarHeader className="border-b border-sidebar-border">
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton>
              <span className="text-sidebar-foreground font-bold text-base tracking-tight">
                Trading Dashboard
              </span>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>

      <SidebarContent>
        <SidebarGroup className="pt-3">
          <SidebarGroupContent>
            <SidebarMenu>
              <SidebarMenuItem>
                <SidebarMenuButton
                  asChild
                  className="bg-sidebar-primary text-sidebar-primary-foreground hover:bg-sidebar-primary/90"
                >
                  <NavLink to="/add-trades">
                    <Plus size={16} />
                    <span>Add Trades</span>
                  </NavLink>
                </SidebarMenuButton>
              </SidebarMenuItem>
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>

        {navSections.map(({ title, items }) => (
          <SidebarGroup key={title}>
            <SidebarGroupLabel>{title}</SidebarGroupLabel>
            <SidebarGroupContent>
              <SidebarMenu>
                {items.map(({ to, icon: Icon, label }) => (
                  <SidebarMenuItem key={to}>
                    <SidebarMenuButton
                      asChild
                      isActive={location.pathname.startsWith(to)}
                    >
                      <NavLink to={to}>
                        <Icon size={16} />
                        <span>{label}</span>
                      </NavLink>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                ))}
              </SidebarMenu>
            </SidebarGroupContent>
          </SidebarGroup>
        ))}
      </SidebarContent>

      <SidebarFooter className="flex flex-row items-center justify-between gap-2 border-t border-sidebar-border">
        <NavUser
          user={{ fullName: user?.fullName ?? "", email: user?.email ?? "" }}
          logoutUser={logoutUser}
        />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  );
}
