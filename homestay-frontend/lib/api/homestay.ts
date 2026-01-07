import apiClient from "./client";
import {
  Homestay,
  HomestayResponse,
  CreateHomestayRequest,
  UpdateHomestayRequest,
} from "@/types";

/**
 * Get all homestays
 */
export async function getAllHomestays(): Promise<HomestayResponse[]> {
  const response = await apiClient.get<HomestayResponse[]>("/homestays");
  return response.data;
}

/**
 * Get homestay by ID
 */
export async function getHomestayById(id: string): Promise<HomestayResponse> {
  const response = await apiClient.get<HomestayResponse>(`/homestays/${id}`);
  return response.data;
}

/**
 * Create homestay (Host only)
 */
export async function createHomestay(
  data: CreateHomestayRequest
): Promise<HomestayResponse> {
  const response = await apiClient.post<HomestayResponse>("/homestays", data);
  return response.data;
}

/**
 * Update homestay (Host only)
 */
export async function updateHomestay(
  id: string,
  data: UpdateHomestayRequest
): Promise<void> {
  await apiClient.put(`/homestays/${id}`, data);
}

/**
 * Delete homestay (Host only)
 */
export async function deleteHomestay(id: string): Promise<void> {
  await apiClient.delete(`/homestays/${id}`);
}
