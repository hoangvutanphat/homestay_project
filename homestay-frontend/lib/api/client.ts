import axios, {
  AxiosError,
  AxiosInstance,
  InternalAxiosRequestConfig,
} from "axios";
import { API_URL } from "../utils/constants";
import { ErrorResponse } from "@/types";

// Create axios instance
const apiClient: AxiosInstance = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
  timeout: 30000, // 30 seconds
  withCredentials: true, // Important: Gửi cookies với mỗi request
});

// Request interceptor - Không cần add token nữa vì dùng httpOnly cookie
apiClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // Token sẽ được browser tự động gửi qua cookie
    // Không cần: config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error: AxiosError) => {
    return Promise.reject(error);
  }
);

// Response interceptor - Handle errors
apiClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ErrorResponse>) => {
    if (error.response) {
      // Server responded with error status
      const { status, data } = error.response;
      const errorMessage = data?.message || "An error occurred";

      switch (status) {
        case 401:
          // Unauthorized - Only redirect if NOT on login page and NOT calling /auth/me
          const isLoginPage = window.location.pathname === "/login";
          const isAuthMeCall = error.config?.url?.includes("/auth/me");

          if (!isLoginPage && !isAuthMeCall) {
            window.location.href = "/login";
          }
          break;
        case 403:
          // Forbidden
          console.error("Access denied:", errorMessage);
          break;
        case 404:
          // Not found
          console.log("Resource not found:", errorMessage);
          break;
        case 500:
          // Server error
          console.error("Server error:", errorMessage);
          break;
        default:
          console.log("API Error:", errorMessage);
      }

      return Promise.reject(data || { message: errorMessage });
    } else if (error.request) {
      // Request was made but no response received
      console.error("Network Error: No response from server");
      return Promise.reject({
        message: "Network error. Please check your connection.",
      });
    } else {
      // Something else happened
      console.error("Error:", error.message);
      return Promise.reject({ message: error.message });
    }
  }
);

export default apiClient;
