"use client";

import { useAuthStore } from "@/stores";
import { UserRole } from "@/types";

export function useAuth() {
  const {
    user,
    isAuthenticated,
    isLoading,
    error,
    login,
    register,
    logout,
    clearError,
    setUser,
  } = useAuthStore();

  const checkRole = (allowedRoles: UserRole[]): boolean => {
    if (!user) return false;
    return allowedRoles.includes(user.role);
  };

  return {
    user,
    isAuthenticated,
    isLoading,
    error,
    login,
    register,
    logout,
    clearError,
    setUser,
    checkRole,
  };
}
