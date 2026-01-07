export interface Homestay {
  id: string;
  name: string;
  description?: string;
  address: string;
  city: string;
  hostId: string;
  hostName?: string;
  createdAt: string;
}

export interface CreateHomestayRequest {
  name: string;
  description?: string;
  address: string;
  city: string;
}

export interface UpdateHomestayRequest {
  name?: string;
  description?: string;
  address?: string;
  city?: string;
}

export interface HomestayResponse {
  id: string;
  name: string;
  description?: string;
  address: string;
  city: string;
  hostId: string;
  createdAt: string;
}
