"use client";

import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Home, ArrowLeft, Search, MapPin } from "lucide-react";
import { useRouter } from "next/navigation";

export default function NotFound() {
  const router = useRouter();

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 via-white to-purple-50 px-4 py-12 relative overflow-hidden">
      {/* Background decoration */}
      <div className="absolute inset-0 overflow-hidden pointer-events-none">
        <div className="absolute top-20 left-10 w-72 h-72 bg-blue-200 rounded-full mix-blend-multiply filter blur-3xl opacity-30 animate-blob"></div>
        <div className="absolute top-40 right-10 w-72 h-72 bg-purple-200 rounded-full mix-blend-multiply filter blur-3xl opacity-30 animate-blob animation-delay-2000"></div>
        <div className="absolute -bottom-8 left-1/2 w-72 h-72 bg-pink-200 rounded-full mix-blend-multiply filter blur-3xl opacity-30 animate-blob animation-delay-4000"></div>
      </div>

      <div className="text-center relative z-10 max-w-2xl mx-auto">
        {/* 404 Number with animation */}
        <div className="mb-8 relative">
          <div className="absolute inset-0 flex items-center justify-center">
            <div className="text-[12rem] md:text-[16rem] font-black text-gray-100 select-none">
              404
            </div>
          </div>
          <h1 className="text-8xl md:text-9xl font-black text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-purple-600 relative z-10 animate-pulse">
            404
          </h1>
        </div>

        {/* Content */}
        <div className="mb-10 space-y-4">
          <h2 className="text-3xl md:text-4xl font-bold text-gray-800">
            Oops! Trang không tồn tại
          </h2>
          <p className="text-gray-600 text-lg max-w-md mx-auto">
            Có vẻ như bạn đã lạc đường. Trang bạn đang tìm kiếm không tồn tại
            hoặc đã bị di chuyển.
          </p>
        </div>

        {/* Action buttons */}
        <div className="flex flex-col sm:flex-row gap-4 justify-center mb-12">
          <Button
            variant="outline"
            size="lg"
            className="gap-2 border-2 hover:border-blue-600 hover:text-blue-600 transition-all"
            onClick={() => router.back()}
          >
            <ArrowLeft size={20} />
            Quay lại
          </Button>
          <Button
            asChild
            size="lg"
            className="gap-2 bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700 text-white shadow-lg hover:shadow-xl transition-all"
          >
            <Link href="/">
              <Home size={20} />
              Về trang chủ
            </Link>
          </Button>
        </div>

        {/* Quick links */}
        <div className="border-t border-gray-200 pt-8">
          <p className="text-sm text-gray-500 mb-6">
            Hoặc khám phá các trang khác:
          </p>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
            <Link
              href="/homestays"
              className="group flex flex-col items-center gap-2 p-4 rounded-lg hover:bg-white hover:shadow-md transition-all"
            >
              <div className="w-12 h-12 rounded-full bg-blue-100 flex items-center justify-center group-hover:bg-blue-600 transition-colors">
                <MapPin
                  className="text-blue-600 group-hover:text-white transition-colors"
                  size={24}
                />
              </div>
              <span className="text-sm font-medium text-gray-700 group-hover:text-blue-600">
                Khám phá
              </span>
            </Link>
            <Link
              href="/search"
              className="group flex flex-col items-center gap-2 p-4 rounded-lg hover:bg-white hover:shadow-md transition-all"
            >
              <div className="w-12 h-12 rounded-full bg-purple-100 flex items-center justify-center group-hover:bg-purple-600 transition-colors">
                <Search
                  className="text-purple-600 group-hover:text-white transition-colors"
                  size={24}
                />
              </div>
              <span className="text-sm font-medium text-gray-700 group-hover:text-purple-600">
                Tìm kiếm
              </span>
            </Link>
            <Link
              href="/login"
              className="group flex flex-col items-center gap-2 p-4 rounded-lg hover:bg-white hover:shadow-md transition-all"
            >
              <div className="w-12 h-12 rounded-full bg-green-100 flex items-center justify-center group-hover:bg-green-600 transition-colors">
                <Home
                  className="text-green-600 group-hover:text-white transition-colors"
                  size={24}
                />
              </div>
              <span className="text-sm font-medium text-gray-700 group-hover:text-green-600">
                Đăng nhập
              </span>
            </Link>
            <Link
              href="/register"
              className="group flex flex-col items-center gap-2 p-4 rounded-lg hover:bg-white hover:shadow-md transition-all"
            >
              <div className="w-12 h-12 rounded-full bg-orange-100 flex items-center justify-center group-hover:bg-orange-600 transition-colors">
                <Home
                  className="text-orange-600 group-hover:text-white transition-colors"
                  size={24}
                />
              </div>
              <span className="text-sm font-medium text-gray-700 group-hover:text-orange-600">
                Đăng ký
              </span>
            </Link>
          </div>
        </div>
      </div>

      <style jsx>{`
        @keyframes blob {
          0%,
          100% {
            transform: translate(0px, 0px) scale(1);
          }
          33% {
            transform: translate(30px, -50px) scale(1.1);
          }
          66% {
            transform: translate(-20px, 20px) scale(0.9);
          }
        }
        .animate-blob {
          animation: blob 7s infinite;
        }
        .animation-delay-2000 {
          animation-delay: 2s;
        }
        .animation-delay-4000 {
          animation-delay: 4s;
        }
      `}</style>
    </div>
  );
}
