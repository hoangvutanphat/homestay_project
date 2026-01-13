import React from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";

interface SecurityCardProps {
  onChangePassword: () => void;
}

const SecurityCard: React.FC<SecurityCardProps> = ({ onChangePassword }) => {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Bảo mật tài khoản</CardTitle>
        <CardDescription>Quản lý mật khẩu và tính năng bảo mật</CardDescription>
      </CardHeader>
      <CardContent className="space-y-4">
        <div>
          <h3 className="font-medium mb-2">Mật khẩu</h3>
          <p className="text-sm text-gray-500">
            Cập nhật mật khẩu định kỳ để tăng cường bảo mật
          </p>
          <Button variant="outline" className="mt-2" onClick={onChangePassword}>
            Đổi mật khẩu
          </Button>
        </div>
        <Separator />
        <div>
          <h3 className="font-medium mb-2">Thiết bị đã đăng nhập</h3>
          <p className="text-sm text-gray-500">
            Kiểm tra và quản lý các thiết bị đã đăng nhập
          </p>
          <Button variant="outline" className="mt-2">
            Xem thiết bị
          </Button>
        </div>
      </CardContent>
    </Card>
  );
};

export default SecurityCard;
