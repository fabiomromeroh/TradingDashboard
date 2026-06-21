import { BrowserRouter, Routes, Route } from 'react-router-dom';
import AppLayout from '../components/layout/AppLayout';
import LoginPage from '../pages/LoginPage';
import DashboardPage from '../pages/DashboardPage';
import TradesPage from '../pages/TradesPage';
import ImportPage from '../pages/ImportPage';
import UsersPage from '@/pages/UsersPage';

export default function Router() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public route — no layout wrapper */}
        <Route path="/login" element={<LoginPage />} />

        {/* Protected routes share AppLayout (sidebar, topbar, etc.)
            Nested routes render inside the <Outlet /> in AppLayout */}
        <Route element={<AppLayout />}>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/trades" element={<TradesPage />} />
          <Route path="/import" element={<ImportPage />} />
                    <Route path="/users" element={<UsersPage />} />

        </Route>
      </Routes>
    </BrowserRouter>
  );
}

