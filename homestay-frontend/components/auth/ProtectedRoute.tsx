"use client";

import React, { useEffect } from "react";
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
  allowedRoles = ["GUEST", "HOST", "ADMIN"],
}) => {
  const { isAuthenticated, isLoading, user, checkRole } = useAuth();
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    // Redirect to login if not authenticated
    if (!isLoading && !isAuthenticated) {
      toast.error("Vui lòng đăng nhập để tiếp tục");
      router.push(`/login?from=${encodeURIComponent(pathname)}`);
      return;
    }

    // Check if user has required role
    if (!isLoading && isAuthenticated && !checkRole(allowedRoles)) {
      toast.error("Bạn không có quyền truy cập trang này");
      router.push("/");
    }
  }, [isLoading, isAuthenticated, allowedRoles, pathname, router, checkRole]);

  // Show loading state while checking authentication
  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-screen">
        Đang tải...
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
