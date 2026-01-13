"use client";

import React, { useEffect, useState } from "react";
import { useRouter, usePathname } from "next/navigation";
import { useAuth } from "@/hooks";
import { UserRole } from "@/types";
import { toast } from "sonner";

interface ProtectedRouteProps {
  children: React.ReactNode;
  allowedRoles?: UserRole[];
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  children,
  allowedRoles = ["guest", "host", "admin"],
}) => {
  const { isAuthenticated, isLoading, checkRole } = useAuth();
  const router = useRouter();
  const pathname = usePathname();
  const [isHydrated, setIsHydrated] = useState(false);

  // Wait for client-side hydration
  useEffect(() => {
    setIsHydrated(true);
  }, []);

  useEffect(() => {
    // Only check auth after hydration is complete
    if (!isHydrated) return;

    console.log("Auth check:", { isLoading, isAuthenticated });

    // Check authentication after loading is complete
    if (!isLoading && !isAuthenticated) {
      toast.message("Vui lòng đăng nhập để tiếp tục");
      router.push(`/login?from=${encodeURIComponent(pathname)}`);
      return;
    }

    // Check role authorization
    if (!isLoading && isAuthenticated && !checkRole(allowedRoles)) {
      toast.error("Bạn không có quyền truy cập trang này");
      router.push("/");
    }
  }, [
    isHydrated,
    isLoading,
    isAuthenticated,
    allowedRoles,
    pathname,
    router,
    checkRole,
  ]);

  // Show loading during initial load or before hydration
  if (!isHydrated || isLoading) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-brand-blue mx-auto mb-4"></div>
          <p className="text-gray-600">Đang tải...</p>
        </div>
      </div>
    );
  }

  // Don't render if not authenticated or doesn't have proper role
  if (!isAuthenticated || !checkRole(allowedRoles)) {
    return null;
  }

  // If authenticated and has proper role, render the children
  return <>{children}</>;
};

export default ProtectedRoute;
