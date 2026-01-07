"use client";

import { useState } from "react";
import { useBookingStore } from "@/stores";
import apiClient from "@/lib/api/client";

/**
 * Custom hook to access booking state and actions (Zustand)
 */
export function useBookings() {
  const {
    bookings,
    currentBooking,
    isLoading,
    error,
    fetchUserBookings,
    fetchBookingById,
    createBooking,
    cancelBooking,
    confirmBooking,
    clearError,
    clearCurrentBooking,
  } = useBookingStore();

  return {
    bookings,
    currentBooking,
    isLoading,
    error,
    fetchUserBookings,
    fetchBookingById,
    createBooking,
    cancelBooking,
    confirmBooking,
    clearError,
    clearCurrentBooking,
  };
}

/**
 * Hook to update payment status after payment
 */
export function useUpdatePaymentStatus() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const updatePaymentStatus = async (orderId: string, status: string) => {
    setIsLoading(true);
    setError(null);
    try {
      await apiClient.post("/bookings/payment-success", { orderId, status });
      return { success: true };
    } catch (err: any) {
      const errorMsg =
        err.message || "Không thể cập nhật trạng thái thanh toán";
      setError(errorMsg);
      return { success: false, error: errorMsg };
    } finally {
      setIsLoading(false);
    }
  };

  return {
    updatePaymentStatus,
    isLoading,
    error,
  };
}

/**
 * Hook to create payment URL
 */
export function useCreatePaymentUrl() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const createPaymentUrl = async (orderId: string, totalPrice: number) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await apiClient.post<{ data: { paymentUrl: string } }>(
        "/bookings/create-payment",
        { orderId, totalPrice }
      );
      return { success: true, paymentUrl: response.data.data.paymentUrl };
    } catch (err: any) {
      const errorMsg = err.message || "Không thể tạo link thanh toán";
      setError(errorMsg);
      return { success: false, error: errorMsg };
    } finally {
      setIsLoading(false);
    }
  };

  return {
    createPaymentUrl,
    isLoading,
    error,
  };
}

/**
 * Hook to check homestay availability
 */
export function useCheckAvailability() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const checkAvailability = async (homestayId: string, date: Date) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await apiClient.get<{ data: { available: boolean } }>(
        `/bookings/check-availability`,
        {
          params: {
            homestayId,
            date: date.toISOString(),
          },
        }
      );
      return { success: true, available: response.data.data.available };
    } catch (err: any) {
      const errorMsg = err.message || "Không thể kiểm tra tình trạng phòng";
      setError(errorMsg);
      return { success: false, error: errorMsg };
    } finally {
      setIsLoading(false);
    }
  };

  return {
    checkAvailability,
    isLoading,
    error,
  };
}
