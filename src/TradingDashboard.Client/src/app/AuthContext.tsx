// context/AuthContext.jsx
import { createContext, useContext, useMemo, type ReactNode } from "react";
import { jwtDecode } from "jwt-decode";
import { useAppSelector } from "@/store/hooks";

interface DecodedToken {
  role?: string;
  [key: string]: unknown;
}

interface AuthContextValue {
  isAuthenticated: boolean;
  authCheckComplete: boolean;
  user: unknown; // replace with your actual User type from store
  role: string | null;
  hasRole: (requiredRole: string) => boolean;
  hasAnyRole: (roles: string[]) => boolean;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const accessToken = useAppSelector((state) => state.auth.accessToken);
  const user = useAppSelector((state) => state.auth.user);
  const authCheckComplete = useAppSelector(
    (state) => state.auth.authCheckComplete,
  );

  const role = useMemo<string | null>(() => {
    if (!accessToken) return null;
    try {
      const decoded = jwtDecode<DecodedToken>(accessToken);

      const roleKey = Object.keys(decoded).find((k) => k.endsWith("/role"));
      return (
        decoded.role ?? (roleKey ? (decoded[roleKey] as string) : null) ?? null
      );
    } catch {
      return null;
    }
  }, [accessToken]);

  const value = useMemo<AuthContextValue>(
    () => ({
      isAuthenticated: !!accessToken,
      authCheckComplete,
      user,
      role,
      hasRole: (requiredRole: string) => role === requiredRole,
      hasAnyRole: (roles: string[]) => roles.includes(role ?? ""),
    }),
    [accessToken, authCheckComplete, user, role],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);

  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
