"use client";

import { useCallback } from "react";
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
    setIsLoading,
  } = useAuthStore();

  const checkRole = useCallback(
    (allowedRoles: UserRole[]): boolean => {
      if (!user) return false;
      return allowedRoles.includes(user.role);
    },
    [user]
  );

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
    setIsLoading,
    checkRole,
  };
}
