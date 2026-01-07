import { format, parseISO } from "date-fns";
import { vi } from "date-fns/locale";
import { CURRENCY_SYMBOL, DATE_FORMAT, DATE_TIME_FORMAT } from "./constants";

/**
 * Format price to Vietnamese currency
 * @example formatPrice(1000000) => "1.000.000₫"
 */
export function formatPrice(price: number): string {
  return (
    new Intl.NumberFormat("vi-VN", {
      style: "decimal",
    }).format(price) + CURRENCY_SYMBOL
  );
}

/**
 * Format date string or Date object
 * @example formatDate('2024-01-01') => "01/01/2024"
 */
export function formatDate(
  date: string | Date,
  formatStr: string = DATE_FORMAT
): string {
  const dateObj = typeof date === "string" ? parseISO(date) : date;
  return format(dateObj, formatStr, { locale: vi });
}

/**
 * Format date with time
 * @example formatDateTime('2024-01-01T10:30:00') => "01/01/2024 10:30"
 */
export function formatDateTime(date: string | Date): string {
  return formatDate(date, DATE_TIME_FORMAT);
}

/**
 * Calculate number of nights between two dates
 */
export function calculateNights(
  checkIn: string | Date,
  checkOut: string | Date
): number {
  const checkInDate = typeof checkIn === "string" ? parseISO(checkIn) : checkIn;
  const checkOutDate =
    typeof checkOut === "string" ? parseISO(checkOut) : checkOut;
  const diffTime = Math.abs(checkOutDate.getTime() - checkInDate.getTime());
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  return diffDays;
}

/**
 * Truncate text with ellipsis
 */
export function truncate(text: string, maxLength: number): string {
  if (text.length <= maxLength) return text;
  return text.slice(0, maxLength) + "...";
}

/**
 * Get initials from name
 * @example getInitials('Nguyen Van A') => "NVA"
 */
export function getInitials(name: string): string {
  return name
    .split(" ")
    .map((word) => word[0])
    .join("")
    .toUpperCase()
    .slice(0, 3);
}
