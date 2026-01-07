import apiClient from "./client";
import {
  Room,
  RoomResponse,
  CreateRoomRequest,
  UpdateRoomRequest,
  AmenityDto,
} from "@/types";

/**
 * Get room by ID
 */
export async function getRoomById(id: string): Promise<RoomResponse> {
  const response = await apiClient.get<RoomResponse>(`/rooms/${id}`);
  return response.data;
}

/**
 * Get rooms by homestay ID
 */
export async function getRoomsByHomestayId(
  homestayId: string
): Promise<RoomResponse[]> {
  const response = await apiClient.get<RoomResponse[]>(
    `/rooms/homestay/${homestayId}`
  );
  return response.data;
}

/**
 * Create new room (Host only)
 */
export async function createRoom(
  data: CreateRoomRequest
): Promise<RoomResponse> {
  const response = await apiClient.post<RoomResponse>("/rooms", data);
  return response.data;
}

/**
 * Update room (Host only)
 */
export async function updateRoom(
  id: string,
  data: UpdateRoomRequest
): Promise<void> {
  await apiClient.put(`/rooms/${id}`, data);
}

/**
 * Delete room (Host only)
 */
export async function deleteRoom(id: string): Promise<void> {
  await apiClient.delete(`/rooms/${id}`);
}

/**
 * Get all amenities
 */
export async function getAllAmenities(): Promise<AmenityDto[]> {
  const response = await apiClient.get<AmenityDto[]>("/rooms/amenities");
  return response.data;
}
