export type BookingStatus = "PENDING" | "CONFIRMED" | "CANCELLED" | "COMPLETED";

export interface Booking {
  id: string;
  userId: string;
  roomId: string;
  roomName: string;
  homestayName: string;
  checkIn: string;
  checkOut: string;
  status: BookingStatus;
  totalAmount: number;
  createdAt: string;
}

export interface CreateBookingRequest {
  roomId: string;
  checkIn: string;
  checkOut: string;
}

export interface BookingResponse {
  id: string;
  userId: string;
  roomId: string;
  roomName: string;
  homestayName: string;
  checkIn: string;
  checkOut: string;
  status: BookingStatus;
  totalAmount: number;
  createdAt: string;
}
