"use client";

import React, { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";
import { Menu, X, User, LogOut, Calendar, LayoutDashboard } from "lucide-react";
import { useAuth } from "@/hooks";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

const Navbar = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const { isAuthenticated, user, logout } = useAuth();
  const router = useRouter();

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const handleLogout = () => {
    logout();
    router.push("/");
  };

  return (
    <header className="sticky top-0 z-40 bg-white/95 backdrop-blur-md border-b">
      <div className="container flex items-center justify-between h-16 px-4 md:px-6">
        <Link href="/" className="flex items-center gap-2">
          <span className="text-2xl font-bold text-blue-600">Homestay</span>
        </Link>

        <button className="block md:hidden" onClick={toggleMenu}>
          {isMenuOpen ? <X size={24} /> : <Menu size={24} />}
        </button>

        <nav className="hidden md:flex items-center gap-6">
          <Link
            href="/"
            className="text-sm font-medium hover:text-blue-600 transition-colors"
          >
            Trang chủ
          </Link>
          <Link
            href="/homestays"
            className="text-sm font-medium hover:text-blue-600 transition-colors"
          >
            Khám phá
          </Link>
        </nav>

        <div className="hidden md:flex items-center gap-2">
          {isAuthenticated ? (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" size="sm" className="gap-2">
                  <User size={18} />
                  <span>{user?.fullName || user?.email}</span>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuLabel>Tài khoản</DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuItem asChild>
                  <Link href="/profile" className="cursor-pointer">
                    <User size={16} className="mr-2" />
                    Hồ sơ cá nhân
                  </Link>
                </DropdownMenuItem>

                {user?.role === "guest" && (
                  <DropdownMenuItem asChild className="cursor-pointer">
                    <Link href="/bookings">
                      <Calendar size={16} className="mr-2" />
                      Đặt phòng của tôi
                    </Link>
                  </DropdownMenuItem>
                )}

                {user?.role === "host" && (
                  <DropdownMenuItem asChild className="cursor-pointer">
                    <Link href="/dashboard">
                      <LayoutDashboard size={16} className="mr-2" />
                      Dashboard
                    </Link>
                  </DropdownMenuItem>
                )}
                <DropdownMenuSeparator />
                <DropdownMenuItem
                  onClick={handleLogout}
                  className="text-red-500 hover:text-red-600 cursor-pointer"
                >
                  <LogOut size={16} className="mr-2" />
                  Đăng xuất
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          ) : (
            <>
              <Button variant="ghost" size="sm" asChild>
                <Link href="/login">Đăng nhập</Link>
              </Button>
              <Button
                size="sm"
                className="bg-blue-600 hover:bg-blue-700"
                asChild
              >
                <Link href="/register" className="text-white">
                  Đăng ký
                </Link>
              </Button>
            </>
          )}
        </div>

        {isMenuOpen && (
          <div className="fixed inset-0 top-16 bg-white z-50 flex flex-col md:hidden">
            <nav className="flex flex-col gap-4 p-6 bg-white ">
              <Link
                href="/"
                className="text-lg font-medium py-2 hover:text-blue-600"
                onClick={toggleMenu}
              >
                Trang chủ
              </Link>
              <Link
                href="/homestays"
                className="text-lg font-medium py-2 hover:text-blue-600"
                onClick={toggleMenu}
              >
                Khám phá
              </Link>
              {/* </Link onClick={toggleMenu}></nav>
              
                Liên hệ
              </Link> */}

              <div className="mt-4 border-t pt-4">
                {isAuthenticated ? (
                  <div className="flex flex-col gap-2">
                    <p className="font-medium">
                      Xin chào, {user?.fullName || user?.email}
                    </p>
                    <Link
                      href="/bookings"
                      className="py-2 hover:text-blue-600"
                      onClick={toggleMenu}
                    >
                      <Calendar size={16} className="inline mr-2" />
                      Đặt phòng của tôi
                    </Link>

                    {user?.role === "host" && (
                      <Link
                        href="/dashboard"
                        className="py-2 hover:text-blue-600"
                        onClick={toggleMenu}
                      >
                        <LayoutDashboard size={16} className="inline mr-2" />
                        Dashboard
                      </Link>
                    )}

                    <Button
                      variant="outline"
                      className="mt-2 w-full justify-start text-red-500"
                      onClick={() => {
                        handleLogout();
                        toggleMenu();
                      }}
                    >
                      <LogOut size={16} className="mr-2" />
                      Đăng xuất
                    </Button>
                  </div>
                ) : (
                  <div className="flex flex-col gap-2">
                    <Button
                      variant="outline"
                      size="lg"
                      className="w-full"
                      asChild
                      onClick={toggleMenu}
                    >
                      <Link href="/login">Đăng nhập</Link>
                    </Button>
                    <Button
                      size="lg"
                      className="w-full bg-blue-600 hover:bg-blue-700"
                      asChild
                      onClick={toggleMenu}
                    >
                      <Link href="/register">Đăng ký</Link>
                    </Button>
                  </div>
                )}
              </div>
            </nav>
          </div>
        )}
      </div>
    </header>
  );
};

export default Navbar;
