"use client";

import React from "react";
import { User, Mail, Phone, MapPin, Shield, Calendar } from "lucide-react";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import { useAuth } from "@/hooks/useAuth";
import { useRouter } from "next/navigation";

interface ProfileCardProps {
  onEdit: () => void;
}

const ProfileCard: React.FC<ProfileCardProps> = ({ onEdit }) => {
  const { user, logout } = useAuth();
  const router = useRouter();

  const handleLogout = () => {
    logout();
    router.push("/");
  };

  return (
    <Card>
      <CardHeader className="pb-2">
        <CardTitle>Hồ sơ cá nhân</CardTitle>
        <CardDescription>Quản lý thông tin tài khoản của bạn</CardDescription>
      </CardHeader>
      <CardContent className="pt-4">
        <div className="flex flex-col items-center space-y-4 mb-6">
          <div className="w-24 h-24 bg-brand-blue/20 rounded-full flex items-center justify-center">
            <User className="h-12 w-12 text-brand-blue" />
          </div>
          <div className="text-center">
            <h3 className="font-medium text-lg">{user?.fullName}</h3>
            <p className="text-sm text-gray-500">
              {user?.role === "admin"
                ? "Quản trị viên"
                : user?.role === "host"
                ? "Chủ nhà"
                : "Người dùng"}
            </p>
          </div>
        </div>

        <Separator className="my-4" />

        <div className="space-y-4">
          <div className="flex items-center space-x-3">
            <Mail className="h-5 w-5 text-gray-500" />
            <div>
              <p className="text-sm text-gray-500">Email</p>
              <p className="font-medium">{user?.email}</p>
            </div>
          </div>

          <div className="flex items-center space-x-3">
            <Phone className="h-5 w-5 text-gray-500" />
            <div>
              <p className="text-sm text-gray-500">Số điện thoại</p>
              <p className="font-medium">
                {user?.phone ? user.phone : "Chưa cập nhật"}
              </p>
            </div>
          </div>

          <div className="flex items-center space-x-3">
            <MapPin className="h-5 w-5 text-gray-500" />
            <div>
              <p className="text-sm text-gray-500">Địa chỉ</p>
              <p className="font-medium">{"Chưa cập nhật"}</p>
            </div>
          </div>

          <div className="flex items-center space-x-3">
            <Shield className="h-5 w-5 text-gray-500" />
            <div>
              <p className="text-sm text-gray-500">Loại tài khoản</p>
              <p className="font-medium capitalize">{user?.role}</p>
            </div>
          </div>

          <div className="flex items-center space-x-3">
            <Calendar className="h-5 w-5 text-gray-500" />
            <div>
              <p className="text-sm text-gray-500">Ngày tham gia</p>
              <p className="font-medium">
                {user?.createdAt
                  ? new Date(user.createdAt).toLocaleDateString("vi-VN")
                  : ""}
              </p>
            </div>
          </div>
        </div>
      </CardContent>
      <CardFooter className="flex justify-between flex-col gap-2">
        <Button variant="outline" className="w-full sm:w-auto" onClick={onEdit}>
          Chỉnh sửa hồ sơ
        </Button>
        <Button
          variant="destructive"
          className="w-full sm:w-auto"
          onClick={handleLogout}
        >
          Đăng xuất
        </Button>
      </CardFooter>
    </Card>
  );
};

export default ProfileCard;
