import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { apiService, LoginRequest, RegisterRequest, LoginResponse } from '../services/api';

interface User {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    address?: string;
    dateOfBirth: string;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string;
    profileImageUrl?: string;
    roles: string[];
}

interface AuthContextType {
    user: User | null;
    isAuthenticated: boolean;
    isLoading: boolean;
    login: (credentials: LoginRequest) => Promise<void>;
    register: (userData: RegisterRequest) => Promise<void>;
    logout: () => void;
    hasRole: (role: string) => boolean;
    hasAnyRole: (roles: string[]) => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        // Check if user is already logged in
        const initializeAuth = async () => {
            try {
                const currentUser = apiService.getCurrentUser();
                const isAuth = apiService.isAuthenticated();

                if (isAuth && currentUser) {
                    setUser(currentUser);
                }
            } catch (error) {
                console.error('Auth initialization error:', error);
                // Clear invalid auth data
                apiService.logout();
            } finally {
                setIsLoading(false);
            }
        };

        initializeAuth();
    }, []);

    const login = async (credentials: LoginRequest) => {
        try {
            setIsLoading(true);
            const response: LoginResponse = await apiService.login(credentials);
            setUser(response.user);
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        } finally {
            setIsLoading(false);
        }
    };

    const register = async (userData: RegisterRequest) => {
        try {
            setIsLoading(true);
            const response: LoginResponse = await apiService.register(userData);
            setUser(response.user);
        } catch (error) {
            console.error('Registration error:', error);
            throw error;
        } finally {
            setIsLoading(false);
        }
    };

    const logout = () => {
        apiService.logout();
        setUser(null);
    };

    const hasRole = (role: string): boolean => {
        return user?.roles?.includes(role) ?? false;
    };

    const hasAnyRole = (roles: string[]): boolean => {
        return user?.roles?.some(role => roles.includes(role)) ?? false;
    };

    const value: AuthContextType = {
        user,
        isAuthenticated: !!user,
        isLoading,
        login,
        register,
        logout,
        hasRole,
        hasAnyRole,
    };

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};
