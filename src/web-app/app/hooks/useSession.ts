'use client';
import { useAuth } from '~/providers/auth-provider';

export const useSession = (required = false) => {
    const { user, isAuthenticated, isLoading, refresh, logout } = useAuth();

    return {
        user,
        isAuthenticated,
        isLoading,
        refresh,
        logout,
    };
};
