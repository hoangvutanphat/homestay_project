"use client";

import { useState } from "react";
import { changePassword, updateUserProfile, getUserById } from "@/lib/api/user";
import { getCurrentUser } from "@/lib/api/auth";
import type { ChangePasswordRequest } from "@/lib/api/user";

/**
 * Comprehensive user management hook
 * Handles all user-related operations: profile, password, etc.
 */
export const useUser = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Change password
  const handleChangePassword = async (data: ChangePasswordRequest) => {
    setIsLoading(true);
    setError(null);

    try {
      const response = await changePassword(data);
      return { success: true, message: response.message };
    } catch (err: any) {
      const errorMessage =
        err.response?.data?.message || err.message || "Đổi mật khẩu thất bại";
      setError(errorMessage);
      return { success: false, message: errorMessage };
    } finally {
      setIsLoading(false);
    }
  };

  // Update profile
  const handleUpdateProfile = async (userId: string, data: any) => {
    setIsLoading(true);
    setError(null);

    try {
      const response = await updateUserProfile(userId, data);
      return { success: true, data: response };
    } catch (err: any) {
      const errorMessage =
        err.response?.data?.message || err.message || "Cập nhật thất bại";
      setError(errorMessage);
      return { success: false, message: errorMessage };
    } finally {
      setIsLoading(false);
    }
  };

  // Get user profile
  const handleGetUser = async (userId?: string) => {
    setIsLoading(true);
    setError(null);

    try {
      const response = userId
        ? await getUserById(userId)
        : await getCurrentUser();
      return { success: true, data: response };
    } catch (err: any) {
      const errorMessage =
        err.response?.data?.message || err.message || "Lấy thông tin thất bại";
      setError(errorMessage);
      return { success: false, message: errorMessage };
    } finally {
      setIsLoading(false);
    }
  };

  return {
    // Actions
    changePassword: handleChangePassword,
    updateProfile: handleUpdateProfile,
    getUser: handleGetUser,

    // State
    isLoading,
    error,
    clearError: () => setError(null),
  };
};

// Legacy exports for backward compatibility (optional)
export const useChangePassword = () => {
  const { changePassword, isLoading, error } = useUser();
  return { changePassword, isLoading, error };
};

export const useUpdateProfile = () => {
  const { updateProfile, isLoading, error } = useUser();
  return { updateProfile, isLoading, error };
};

export const useGetUserIsLogin = () => {
  const { getUser, isLoading, error } = useUser();
  return { getUserIsLogin: getUser, isLoading, error };
};
