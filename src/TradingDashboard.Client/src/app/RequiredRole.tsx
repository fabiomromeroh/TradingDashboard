import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthContext";

export default function RequiredRole({ roles }: { roles: string[] }) {
  const { hasAnyRole, authCheckComplete } = useAuth();

  if (!authCheckComplete) return null;

  if (!hasAnyRole(roles)) {
    return <Navigate to="/403" replace />;
  }

  return <Outlet />;
}
