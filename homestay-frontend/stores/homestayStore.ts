import { create } from "zustand";
import { HomestayResponse } from "@/types";
import * as homestayApi from "@/lib/api/homestay";

interface HomestayStore {
  homestays: HomestayResponse[];
  currentHomestay: HomestayResponse | null;
  isLoading: boolean;
  error: string | null;

  fetchHomestays: () => Promise<void>;
  fetchHomestayById: (id: string) => Promise<void>;
  createHomestay: (data: any) => Promise<HomestayResponse>;
  updateHomestay: (id: string, data: any) => Promise<void>;
  deleteHomestay: (id: string) => Promise<void>;
  clearError: () => void;
  clearCurrentHomestay: () => void;
}

export const useHomestayStore = create<HomestayStore>((set, get) => ({
  homestays: [],
  currentHomestay: null,
  isLoading: false,
  error: null,

  fetchHomestays: async () => {
    set({ isLoading: true, error: null });
    try {
      const homestays = await homestayApi.getAllHomestays();
      set({ homestays, isLoading: false });
    } catch (error: any) {
      set({
        error: error.message || "Failed to fetch homestays",
        isLoading: false,
      });
    }
  },

  fetchHomestayById: async (id) => {
    set({ isLoading: true, error: null });
    try {
      const homestay = await homestayApi.getHomestayById(id);
      set({ currentHomestay: homestay, isLoading: false });
    } catch (error: any) {
      set({
        error: error.message || "Failed to fetch homestay",
        isLoading: false,
      });
    }
  },

  createHomestay: async (data) => {
    set({ isLoading: true, error: null });
    try {
      const newHomestay = await homestayApi.createHomestay(data);
      set((state) => ({
        homestays: [...state.homestays, newHomestay],
        isLoading: false,
      }));
      return newHomestay;
    } catch (error: any) {
      set({
        error: error.message || "Failed to create homestay",
        isLoading: false,
      });
      throw error;
    }
  },

  updateHomestay: async (id, data) => {
    set({ isLoading: true, error: null });
    try {
      await homestayApi.updateHomestay(id, data);

      await get().fetchHomestays();

      set({ isLoading: false });
    } catch (error: any) {
      set({
        error: error.message || "Failed to update homestay",
        isLoading: false,
      });
      throw error;
    }
  },

  deleteHomestay: async (id) => {
    set({ isLoading: true, error: null });
    try {
      await homestayApi.deleteHomestay(id);

      set((state) => ({
        homestays: state.homestays.filter((h) => h.id !== id),
        isLoading: false,
      }));
    } catch (error: any) {
      set({
        error: error.message || "Failed to delete homestay",
        isLoading: false,
      });
      throw error;
    }
  },

  clearError: () => set({ error: null }),

  clearCurrentHomestay: () => set({ currentHomestay: null }),
}));
