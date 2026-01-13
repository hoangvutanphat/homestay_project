import apiClient from "./client";

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export interface ChangePasswordResponse {
  message: string;
}

/**
 * Change user password
 */
export async function changePassword(
  data: ChangePasswordRequest
): Promise<ChangePasswordResponse> {
  const response = await apiClient.post<ChangePasswordResponse>(
    "/auth/change-password",
    data
  );
  return response.data;
}

/**
 * Update user profile
 */
export async function updateUserProfile(
  userId: string,
  data: any
): Promise<any> {
  const response = await apiClient.put(`/users/${userId}`, data);
  return response.data;
}

/**
 * Get user by ID
 */
export async function getUserById(userId: string): Promise<any> {
  const response = await apiClient.get(`/users/${userId}`);
  return response.data;
}
