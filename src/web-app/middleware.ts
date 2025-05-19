import { NextRequest, NextResponse } from 'next/server';
import AuthService from '@api/auth/service';

const protectedRoutes = ['/messages'];
const authRoutes = ['/login', '/register'];

const dynamicSlugPattern = /^\/[^/]*$/;

async function attemptRefresh(request: NextRequest): Promise<boolean> {
    try {
        const response = await AuthService.refreshToken();
        return !!response;
    } catch (error) {
        return false;
    }
}

export default async function middleware(req: NextRequest) {
    //Check if the current route is protected, public, or slug
    const path = req.nextUrl.pathname;
    const isDynamicSlugRoute = dynamicSlugPattern.test(path);

    if (isDynamicSlugRoute) {
        return NextResponse.next();
    }

    const isProtectedRoute = protectedRoutes.includes(path);
    const isAuthRoute = authRoutes.includes(path);

    let isUserAuthenticated = await AuthService.getAuthenticated();

    if (!isUserAuthenticated) {
        isUserAuthenticated = await attemptRefresh(req);
    }

    // Redirect to /login if the user is not authenticated
    if (isProtectedRoute && !isUserAuthenticated) {
        return NextResponse.redirect(new URL('/login', req.nextUrl));
    }

    if (isAuthRoute && isUserAuthenticated) {
        return NextResponse.redirect(new URL('/', req.nextUrl));
    }

    return NextResponse.next();
}

export const config = {
    matcher: ['/((?!api|_next/static|_next/image|.*\\.png$).*)'],
};
