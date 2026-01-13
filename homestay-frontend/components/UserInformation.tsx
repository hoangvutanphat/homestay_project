"use client";
import React, { useState } from "react";
import Footer from "@/components/Footer";
import ProtectedRoute from "@/components/auth/ProtectedRoute";
import ProfileCard from "@/components/profile/ProfileCard";
// import BookingHistory from "@/components/profile/BookingHistory"; // TODO: Implement later - host doesn't have bookings
import SecurityCard from "@/components/profile/SecurityCard";
import ChangePasswordModal from "./profile/ChangePasswordModal";

const UserInformation: React.FC = () => {
  const [isEditProfileModalOpen, setIsEditProfileModalOpen] = useState(false);
  const [isChangePasswordModalOpen, setIsChangePasswordModalOpen] =
    useState(false);

  return (
    <ProtectedRoute>
      <div className="min-h-screen flex flex-col">
        <div className="flex-1 container mx-auto px-4 py-8">
          <div className="flex flex-col md:flex-row gap-8">
            <div className="w-full md:w-1/3">
              <ProfileCard onEdit={() => setIsEditProfileModalOpen(true)} />
            </div>

            <div className="w-full md:w-2/3">
              {/* TODO: Implement BookingHistory later - need to handle different roles (guest/host) */}
              {/* <BookingHistory /> */}
              <SecurityCard
                onChangePassword={() => setIsChangePasswordModalOpen(true)}
              />
            </div>
          </div>
        </div>

        <ChangePasswordModal
          isOpen={isChangePasswordModalOpen}
          onClose={() => setIsChangePasswordModalOpen(false)}
        />

        <Footer />
      </div>
    </ProtectedRoute>
  );
};

export default UserInformation;
