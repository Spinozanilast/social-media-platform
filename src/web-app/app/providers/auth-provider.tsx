'use client';
import { createContext, useCallback, useContext, useEffect, useState } from 'react';
import AuthService from '~/api/auth/service';
import UserStorage from '~/api/storage/user';
import { LoginRequest, User } from '~api/auth/types';

interface AuthContextType {
    user: User | null;
    isAuthorized: boolean;
    isLoading: boolean;
    checkAuth: () => Promise<void>;
    login: (payload: LoginRequest) => Promise<void>;
    logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);
    const [isAuthorized, setIsAuthorized] = useState(false);
    const [isLoading, setIsLoading] = useState(true);

    const checkAuth = async () => {
        setIsLoading(true);
        try {
            const currentUser = await AuthService.getCurrentUser();
            if (currentUser) {
                setUser(currentUser);
                setIsAuthorized(true);
            }
            else {
                setUser(null);
                setIsAuthorized(false);
            }
        }
        catch (error) {
            setUser(null);
            setIsAuthorized(false);
        }
        finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        checkAuth();
    }, []);

    const login = useCallback(async (payload: LoginRequest) => {
        await AuthService.login(payload);
        await checkAuth();

    }, []);

    const logout = useCallback(async () => {
        try {
            await AuthService.logOut();
        } catch (error) {
            UserStorage.saveUserId("");
            setUser(null);
            setIsAuthorized(false);
        }
    }, []);

    const value = {
        user,
        isAuthorized,
        isLoading,
        checkAuth,
        login,
        logout,
    };

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
}

export const useAuth = () => {
    const context = useContext(AuthContext);

    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }

    return context;
};