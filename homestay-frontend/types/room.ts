import { Amenity } from "./amenity";

export type RoomStatus = "AVAILABLE" | "UNAVAILABLE";

export interface Room {
  id: string;
  roomName: string;
  homestayId: string;
  homestayName: string;
  homestayAddress: string;
  homestayCity: string;
  capacity: number;
  basePrice: number;
  status: RoomStatus;
  createdAt: string;
  amenities: Amenity[];
}

export interface CreateRoomRequest {
  homestayId: string;
  roomName: string;
  capacity: number;
  basePrice: number;
  status: RoomStatus;
  amenityIds?: number[];
}

export interface UpdateRoomRequest {
  roomName?: string;
  capacity?: number;
  basePrice?: number;
  status?: RoomStatus;
  amenityIds?: number[];
}

export interface RoomResponse {
  id: string;
  roomName: string;
  homestayId: string;
  homestayName: string;
  homestayAddress: string;
  homestayCity: string;
  capacity: number;
  basePrice: number;
  status: RoomStatus;
  createdAt: string;
  amenities: Amenity[];
}
