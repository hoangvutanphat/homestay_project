import { create } from "zustand";
import { BookingResponse, CreateBookingRequest } from "@/types";
import * as bookingApi from "@/lib/api/booking";

interface BookingStore {
  bookings: BookingResponse[];
  currentBooking: BookingResponse | null;
  isLoading: boolean;
  error: string | null;

  fetchUserBookings: () => Promise<void>;
  fetchBookingById: (id: string) => Promise<void>;
  createBooking: (data: CreateBookingRequest) => Promise<BookingResponse>;
  cancelBooking: (id: string) => Promise<void>;
  confirmBooking: (id: string) => Promise<void>;
  clearError: () => void;
  clearCurrentBooking: () => void;
}

export const useBookingStore = create<BookingStore>((set, get) => ({
  bookings: [],
  currentBooking: null,
  isLoading: false,
  error: null,

  fetchUserBookings: async () => {
    set({ isLoading: true, error: null });
    try {
      const bookings = await bookingApi.getUserBookings();
      set({ bookings, isLoading: false });
    } catch (error: any) {
      set({
        error: error.message || "Failed to fetch bookings",
        isLoading: false,
      });
    }
  },

  fetchBookingById: async (id) => {
    set({ isLoading: true, error: null });
    try {
      const booking = await bookingApi.getBookingById(id);
      set({ currentBooking: booking, isLoading: false });
    } catch (error: any) {
      set({
        error: error.message || "Failed to fetch booking",
        isLoading: false,
      });
    }
  },

  createBooking: async (data) => {
    set({ isLoading: true, error: null });
    try {
      const newBooking = await bookingApi.createBooking(data);
      set((state) => ({
        bookings: [...state.bookings, newBooking],
        currentBooking: newBooking,
        isLoading: false,
      }));
      return newBooking;
    } catch (error: any) {
      set({
        error: error.message || "Failed to create booking",
        isLoading: false,
      });
      throw error;
    }
  },

  // Cancel booking
  cancelBooking: async (id) => {
    set({ isLoading: true, error: null });
    try {
      await bookingApi.cancelBooking(id);

      // Update booking status in state
      set((state) => ({
        bookings: state.bookings.map((b) =>
          b.id === id ? { ...b, status: "CANCELLED" as const } : b
        ),
        isLoading: false,
      }));
    } catch (error: any) {
      set({
        error: error.message || "Failed to cancel booking",
        isLoading: false,
      });
      throw error;
    }
  },

  // Confirm booking (after payment)
  confirmBooking: async (id) => {
    set({ isLoading: true, error: null });
    try {
      await bookingApi.confirmBooking(id);

      // Update booking status in state
      set((state) => ({
        bookings: state.bookings.map((b) =>
          b.id === id ? { ...b, status: "CONFIRMED" as const } : b
        ),
        isLoading: false,
      }));
    } catch (error: any) {
      set({
        error: error.message || "Failed to confirm booking",
        isLoading: false,
      });
      throw error;
    }
  },

  // Clear error
  clearError: () => set({ error: null }),

  // Clear current booking
  clearCurrentBooking: () => set({ currentBooking: null }),
}));
