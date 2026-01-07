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

      switch (status) {
        case 401:
          // Unauthorized - Redirect to login (cookie đã hết hạn hoặc invalid)
          window.location.href = "/login";
          break;
        case 403:
          // Forbidden
          console.error("Access denied:", data.message);
          break;
        case 404:
          // Not found
          console.error("Resource not found:", data.message);
          break;
        case 500:
          // Server error
          console.error("Server error:", data.message);
          break;
        default:
          console.error("API Error:", data.message);
      }

      return Promise.reject(data);
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
