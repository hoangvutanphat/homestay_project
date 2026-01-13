"use client";
import React, { useState, useMemo } from "react";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Alert, AlertDescription } from "@/components/ui/alert";
import {
  Lock,
  Eye,
  EyeOff,
  AlertCircle,
  CheckCircle2,
  Loader2,
} from "lucide-react";
import { toast } from "sonner";

import { useAuth } from "@/hooks";
import { useChangePassword } from "@/hooks/useUser";

interface ChangePasswordModalProps {
  isOpen: boolean;
  onClose: () => void;
}

interface PasswordFormData {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

interface PasswordVisibility {
  current: boolean;
  new: boolean;
  confirm: boolean;
}

interface PasswordStrength {
  strength: number;
  label: string;
  color: string;
}

interface PasswordRequirement {
  label: string;
  met: boolean;
}

const INITIAL_FORM_DATA: PasswordFormData = {
  currentPassword: "",
  newPassword: "",
  confirmPassword: "",
};

const INITIAL_PASSWORD_VISIBILITY: PasswordVisibility = {
  current: false,
  new: false,
  confirm: false,
};

const STRENGTH_LABELS = ["Rất yếu", "Yếu", "Trung bình", "Mạnh", "Rất mạnh"];
const STRENGTH_COLORS = [
  "bg-red-500",
  "bg-orange-500",
  "bg-yellow-500",
  "bg-blue-500",
  "bg-green-500",
];

const calculatePasswordStrength = (password: string): PasswordStrength => {
  if (!password) return { strength: 0, label: "", color: "" };

  let strength = 0;
  if (password.length >= 8) strength++;
  if (password.length >= 12) strength++;
  if (/[a-z]/.test(password) && /[A-Z]/.test(password)) strength++;
  if (/\d/.test(password)) strength++;
  if (/[^a-zA-Z0-9]/.test(password)) strength++;

  return {
    strength,
    label: STRENGTH_LABELS[strength],
    color: STRENGTH_COLORS[strength],
  };
};

const getPasswordRequirements = (password: string): PasswordRequirement[] => [
  { label: "Ít nhất 6 ký tự", met: password.length >= 6 },
  {
    label: "Chứa chữ hoa và chữ thường",
    met: /[a-z]/.test(password) && /[A-Z]/.test(password),
  },
  { label: "Chứa số", met: /\d/.test(password) },
  { label: "Chứa ký tự đặc biệt", met: /[^a-zA-Z0-9]/.test(password) },
];

const ChangePasswordModal = ({ isOpen, onClose }: ChangePasswordModalProps) => {
  const { changePassword } = useChangePassword();
  const { user } = useAuth();

  const [isLoading, setIsLoading] = useState(false);
  const [formData, setFormData] = useState<PasswordFormData>(INITIAL_FORM_DATA);
  const [showPasswords, setShowPasswords] = useState<PasswordVisibility>(
    INITIAL_PASSWORD_VISIBILITY
  );

  const passwordStrength = useMemo(
    () => calculatePasswordStrength(formData.newPassword),
    [formData.newPassword]
  );
  const passwordRequirements = useMemo(
    () => getPasswordRequirements(formData.newPassword),
    [formData.newPassword]
  );
  const isPasswordMatch =
    formData.newPassword &&
    formData.confirmPassword &&
    formData.newPassword === formData.confirmPassword;

  const handleInputChange = (field: keyof PasswordFormData, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const togglePasswordVisibility = (field: keyof PasswordVisibility) => {
    setShowPasswords((prev) => ({ ...prev, [field]: !prev[field] }));
  };

  const validateForm = (): boolean => {
    const validations = [
      {
        condition: !formData.currentPassword.trim(),
        message: "Vui lòng nhập mật khẩu hiện tại",
      },
      {
        condition: !formData.newPassword.trim(),
        message: "Vui lòng nhập mật khẩu mới",
      },
      {
        condition: formData.newPassword.length < 6,
        message: "Mật khẩu mới phải có ít nhất 6 ký tự",
      },
      {
        condition: formData.newPassword !== formData.confirmPassword,
        message: "Mật khẩu xác nhận không khớp",
      },
      {
        condition: formData.currentPassword === formData.newPassword,
        message: "Mật khẩu mới phải khác mật khẩu hiện tại",
      },
    ];

    for (const { condition, message } of validations) {
      if (condition) {
        toast.error(message);
        return false;
      }
    }

    return true;
  };

  const handleSave = async () => {
    if (!validateForm()) return;

    setIsLoading(true);
    try {
      const result = await changePassword({
        currentPassword: formData.currentPassword,
        newPassword: formData.newPassword,
      });

      if (result.success) {
        toast.success("Mật khẩu đã được thay đổi thành công");
        handleClose();
      } else {
        toast.error(result.message || "Đổi mật khẩu thất bại");
      }
    } catch (error: any) {
      toast.error(error?.message || "Có lỗi xảy ra, vui lòng thử lại");
    } finally {
      setIsLoading(false);
    }
  };

  const handleClose = () => {
    if (isLoading) return;
    setFormData(INITIAL_FORM_DATA);
    setShowPasswords(INITIAL_PASSWORD_VISIBILITY);
    onClose();
  };

  return (
    <Dialog open={isOpen} onOpenChange={handleClose}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <Lock className="h-5 w-5 text-brand-blue" />
            Đổi mật khẩu
          </DialogTitle>
          <DialogDescription>
            Tạo mật khẩu mới mạnh mẽ để bảo vệ tài khoản của bạn
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-5 py-4">
          <PasswordInput
            id="currentPassword"
            label="Mật khẩu hiện tại"
            value={formData.currentPassword}
            showPassword={showPasswords.current}
            onChange={(value) => handleInputChange("currentPassword", value)}
            onToggleVisibility={() => togglePasswordVisibility("current")}
            placeholder="Nhập mật khẩu hiện tại"
            disabled={isLoading}
          />

          <div className="space-y-2">
            <PasswordInput
              id="newPassword"
              label="Mật khẩu mới"
              value={formData.newPassword}
              showPassword={showPasswords.new}
              onChange={(value) => handleInputChange("newPassword", value)}
              onToggleVisibility={() => togglePasswordVisibility("new")}
              placeholder="Nhập mật khẩu mới"
              disabled={isLoading}
            />

            {formData.newPassword && (
              <>
                <PasswordStrengthIndicator strength={passwordStrength} />
                <PasswordRequirementsList requirements={passwordRequirements} />
              </>
            )}
          </div>

          <div className="space-y-2">
            <PasswordInput
              id="confirmPassword"
              label="Xác nhận mật khẩu mới"
              value={formData.confirmPassword}
              showPassword={showPasswords.confirm}
              onChange={(value) => handleInputChange("confirmPassword", value)}
              onToggleVisibility={() => togglePasswordVisibility("confirm")}
              placeholder="Nhập lại mật khẩu mới"
              disabled={isLoading}
            />

            {formData.confirmPassword && (
              <PasswordMatchIndicator isMatch={isPasswordMatch || false} />
            )}
          </div>

          <Alert className="border-amber-200 bg-amber-50">
            <AlertCircle className="h-4 w-4 text-amber-600" />
            <AlertDescription className="text-xs text-amber-800">
              Sau khi đổi mật khẩu, bạn sẽ cần đăng nhập lại bằng mật khẩu mới.
            </AlertDescription>
          </Alert>
        </div>

        <DialogFooter className="gap-2">
          <Button variant="outline" onClick={handleClose} disabled={isLoading}>
            Hủy
          </Button>
          <Button
            onClick={handleSave}
            disabled={
              isLoading || !formData.currentPassword || !isPasswordMatch
            }
            className="bg-brand-blue hover:bg-brand-blue/90"
          >
            {isLoading ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                Đang xử lý...
              </>
            ) : (
              "Đổi mật khẩu"
            )}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

// Sub-components
interface PasswordInputProps {
  id: string;
  label: string;
  value: string;
  showPassword: boolean;
  onChange: (value: string) => void;
  onToggleVisibility: () => void;
  placeholder: string;
  disabled: boolean;
}

const PasswordInput: React.FC<PasswordInputProps> = ({
  id,
  label,
  value,
  showPassword,
  onChange,
  onToggleVisibility,
  placeholder,
  disabled,
}) => (
  <div className="space-y-2">
    <Label htmlFor={id} className="text-sm font-medium">
      {label}
    </Label>
    <div className="relative">
      <Input
        id={id}
        type={showPassword ? "text" : "password"}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
        className="pr-10"
        disabled={disabled}
      />
      <button
        type="button"
        onClick={onToggleVisibility}
        className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-500 hover:text-gray-700 transition-colors"
        disabled={disabled}
      >
        {showPassword ? (
          <EyeOff className="h-4 w-4" />
        ) : (
          <Eye className="h-4 w-4" />
        )}
      </button>
    </div>
  </div>
);

interface PasswordStrengthIndicatorProps {
  strength: PasswordStrength;
}

const PasswordStrengthIndicator: React.FC<PasswordStrengthIndicatorProps> = ({
  strength,
}) => (
  <div className="space-y-2">
    <div className="flex items-center justify-between text-xs">
      <span className="text-gray-600">Độ mạnh mật khẩu:</span>
      <span className="font-medium">{strength.label}</span>
    </div>
    <div className="flex gap-1 h-1.5">
      {[...Array(5)].map((_, i) => (
        <div
          key={i}
          className={`flex-1 rounded-full transition-colors ${
            i < strength.strength ? strength.color : "bg-gray-200"
          }`}
        />
      ))}
    </div>
  </div>
);

interface PasswordRequirementsListProps {
  requirements: PasswordRequirement[];
}

const PasswordRequirementsList: React.FC<PasswordRequirementsListProps> = ({
  requirements,
}) => (
  <div className="bg-gray-50 rounded-lg p-3 space-y-1.5">
    <p className="text-xs font-medium text-gray-700 mb-2">Yêu cầu mật khẩu:</p>
    {requirements.map((req, index) => (
      <div key={index} className="flex items-center gap-2 text-xs">
        {req.met ? (
          <CheckCircle2 className="h-3.5 w-3.5 text-green-500 flex-shrink-0" />
        ) : (
          <AlertCircle className="h-3.5 w-3.5 text-gray-400 flex-shrink-0" />
        )}
        <span className={req.met ? "text-green-700" : "text-gray-600"}>
          {req.label}
        </span>
      </div>
    ))}
  </div>
);

interface PasswordMatchIndicatorProps {
  isMatch: boolean;
}

const PasswordMatchIndicator: React.FC<PasswordMatchIndicatorProps> = ({
  isMatch,
}) => (
  <div className="flex items-center gap-2 text-xs">
    {isMatch ? (
      <>
        <CheckCircle2 className="h-3.5 w-3.5 text-green-500" />
        <span className="text-green-700">Mật khẩu khớp</span>
      </>
    ) : (
      <>
        <AlertCircle className="h-3.5 w-3.5 text-red-500" />
        <span className="text-red-700">Mật khẩu không khớp</span>
      </>
    )}
  </div>
);

export default ChangePasswordModal;
