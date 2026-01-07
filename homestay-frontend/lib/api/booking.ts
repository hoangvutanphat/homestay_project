import apiClient from "./client";
import { Booking, BookingResponse, CreateBookingRequest } from "@/types";

/**
 * Create new booking
 */
export async function createBooking(
  data: CreateBookingRequest
): Promise<BookingResponse> {
  const response = await apiClient.post<BookingResponse>("/bookings", data);
  return response.data;
}

/**
 * Get booking by ID
 */
export async function getBookingById(id: string): Promise<BookingResponse> {
  const response = await apiClient.get<BookingResponse>(`/bookings/${id}`);
  return response.data;
}

/**
 * Get user's bookings
 */
export async function getUserBookings(): Promise<BookingResponse[]> {
  const response = await apiClient.get<BookingResponse[]>("/bookings/user");
  return response.data;
}

/**
 * Cancel booking
 */
export async function cancelBooking(id: string): Promise<void> {
  await apiClient.post(`/bookings/${id}/cancel`);
}

/**
 * Confirm booking (after payment)
 */
export async function confirmBooking(id: string): Promise<void> {
  await apiClient.post(`/bookings/${id}/confirm`);
}
