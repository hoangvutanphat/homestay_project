import apiClient from "./client";
import { LoginRequest, RegisterRequest, LoginResponse, User } from "@/types";

/**
 * Login user
 */
export async function login(data: LoginRequest): Promise<LoginResponse> {
  const response = await apiClient.post<LoginResponse>("/auth/login", data);
  return response.data;
}

/**
 * Register new user
 */
export async function register(data: RegisterRequest): Promise<LoginResponse> {
  const response = await apiClient.post<LoginResponse>("/auth/register", data);
  return response.data;
}

/**
 * Logout user (if backend has logout endpoint)
 */
export async function logout(): Promise<void> {
  await apiClient.post("/auth/logout");
}

/**
 * Get current user profile
 */
export async function getCurrentUser(): Promise<User> {
  const response = await apiClient.get<User>("/auth/me");
  return response.data;
}

/**
 * Refresh token (if implemented)
 */
export async function refreshToken(): Promise<{ token: string }> {
  const response = await apiClient.post<{ token: string }>("/auth/refresh");
  return response.data;
}
