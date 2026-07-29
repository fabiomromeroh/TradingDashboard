import { BrowserRouter, Routes, Route } from "react-router-dom";
import AppLayout from "../components/layout/AppLayout";
import LoginPage from "../pages/LoginPage";
import TradesPage from "../pages/TradesPage";
import UsersPage from "@/pages/UsersPage";
import { AddTradesPage } from "@/pages/AddTradesPage";
import { DashboardPage } from "@/pages/DashboardPage";
import { ReportsPage } from "@/pages/ReportsPage";
import RegisterPage from "@/pages/RegisterPage";
import ProtectedRoute from "./ProtectedRoute";
import RequireRole from "./RequiredRole";
import { AuthProvider } from "./AuthContext";
import Forbidden403Page from "@/pages/Forbidden403Page";

export default function Router() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          {/* Public route — no layout wrapper */}
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/403" element={<Forbidden403Page />} />

          {/* Protected routes share AppLayout (sidebar, topbar, etc.)
            Nested routes render inside the <Outlet /> in AppLayout */}
          <Route element={<ProtectedRoute />}>
            <Route element={<AppLayout />}>
              <Route path="/" element={<DashboardPage />} />
              <Route path="/dashboard" element={<DashboardPage />} />
              <Route path="/trades" element={<TradesPage />} />
              {/* <Route path="/accounts" element={<AccountsPage />} /> */}
              <Route path="/add-trades" element={<AddTradesPage />} />
              <Route path="/reports" element={<ReportsPage />} />

              <Route element={<RequireRole roles={["Admin"]} />}>
                <Route path="/users" element={<UsersPage />} />
              </Route>
            </Route>
          </Route>
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  );
}
