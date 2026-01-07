"use client";

import { useHomestayStore } from "@/stores";

/**
 * Custom hook to access homestay state and actions (Zustand)
 */
export function useHomestays() {
  const {
    homestays,
    currentHomestay,
    isLoading,
    error,
    fetchHomestays,
    fetchHomestayById,
    createHomestay,
    updateHomestay,
    deleteHomestay,
    clearError,
    clearCurrentHomestay,
  } = useHomestayStore();

  return {
    homestays,
    currentHomestay,
    isLoading,
    error,
    fetchHomestays,
    fetchHomestayById,
    createHomestay,
    updateHomestay,
    deleteHomestay,
    clearError,
    clearCurrentHomestay,
  };
}

/**
 * Filter params for homestay search
 */
export interface HomestayFilterParams {
  location?: string;
  checkIn?: Date;
  checkOut?: Date;
  minPrice?: number;
  maxPrice?: number;
  types?: string[];
  amenities?: string[];
  minRating?: number;
}
