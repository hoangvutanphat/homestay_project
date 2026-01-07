import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  const token = request.cookies.get("token")?.value;
  const { pathname } = request.nextUrl;

  // Public routes that don't need authentication
  const publicRoutes = ["/", "/homestays", "/login", "/register"];
  const isPublicRoute = publicRoutes.some(
    (route) => pathname === route || pathname.startsWith("/homestays/")
  );

  // Auth pages - redirect to home if already authenticated
  const isAuthPage =
    pathname.startsWith("/login") || pathname.startsWith("/register");
  if (isAuthPage && token) {
    return NextResponse.redirect(new URL("/", request.url));
  }

  // Protected routes - redirect to login if not authenticated
  const protectedPrefixes = ["/dashboard", "/bookings", "/admin", "/profile"];
  const isProtectedRoute = protectedPrefixes.some((prefix) =>
    pathname.startsWith(prefix)
  );

  if (isProtectedRoute && !token) {
    const url = new URL("/login", request.url);
    url.searchParams.set("from", pathname);
    return NextResponse.redirect(url);
  }

  return NextResponse.next();
}

export const config = {
  matcher: [
    /*
     * Match all request paths except for:
     * - api (API routes)
     * - _next/static (static files)
     * - _next/image (image optimization files)
     * - favicon.ico (favicon file)
     * - public folder
     */
    "/((?!api|_next/static|_next/image|favicon.ico|.*\\..*|public).*)",
  ],
};
