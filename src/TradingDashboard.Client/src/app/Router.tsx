import { BrowserRouter, Routes, Route } from "react-router-dom";
import AppLayout from "../components/layout/AppLayout";
import LoginPage from "../pages/LoginPage";
import TradesPage from "../pages/TradesPage";
import AccountsPage from "../pages/AccountsPage";
import UsersPage from "@/pages/UsersPage";
import { AddTradesPage } from "@/pages/AddTradesPage";
import { DashboardPage } from "@/pages/DashboardPage";
import { ReportsPage } from "@/pages/ReportsPage";

export default function Router() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public route — no layout wrapper */}
        <Route path="/login" element={<LoginPage />} />

        {/* Protected routes share AppLayout (sidebar, topbar, etc.)
            Nested routes render inside the <Outlet /> in AppLayout */}
        <Route element={<AppLayout />}>
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/trades" element={<TradesPage />} />
          <Route path="/accounts" element={<AccountsPage />} />
          <Route path="/users" element={<UsersPage />} />
          <Route path="/add-trades" element={<AddTradesPage />} />
          <Route path="/reports" element={<ReportsPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
