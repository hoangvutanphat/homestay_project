"use client";

import { useEffect } from "react";
import { useAuth } from "@/hooks";

interface AuthProviderProps {
  children: React.ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const { setIsLoading, isLoading } = useAuth();

  useEffect(() => {
    // Zustand persist đã tự động rehydrate user từ localStorage
    // Chỉ cần set isLoading = false để app bắt đầu render
    setIsLoading(false);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Đang tải...</p>
        </div>
      </div>
    );
  }

  return <>{children}</>;
}
