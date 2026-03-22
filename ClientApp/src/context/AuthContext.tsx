import React, { createContext, useContext, useState, useEffect } from 'react';
import type { ReactNode } from 'react';

interface UserData {
    userId: string;
    appUserId: number;
    email: string;
    roles: string[];
    driverId?: string;
    customerId?: string;
    transporterId?: string;
    [key: string]: any;
}

interface AuthContextType {
    user: UserData | null;
    token: string | null;
    isAuthenticated: boolean;
    login: (token: string, userData: any) => void;
    logout: () => void;
    loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<UserData | null>(null);
    const [token, setToken] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const storedToken = localStorage.getItem('token');
        const storedUserStr = localStorage.getItem('user');

        if (storedToken && storedUserStr) {
            try {
                const userData = JSON.parse(storedUserStr);
                setToken(storedToken);
                setUser({
                    userId: userData.UserId || userData.userId || localStorage.getItem('userId'),
                    appUserId: Number(userData.AppUserId || userData.appUserId || localStorage.getItem('appUserId')),
                    email: userData.Email || userData.email || '',
                    roles: userData.Roles || userData.roles || [],
                    driverId: userData.DriverId || userData.driverId,
                    customerId: userData.CustomerId || userData.customerId,
                    transporterId: userData.TransporterId || userData.transporterId,
                    ...userData
                });
            } catch (error) {
                console.error('Failed to parse stored user data', error);
                logout();
            }
        }
        setLoading(false);
    }, []);

    const login = (token: string, data: any) => {
        const userData: UserData = {
            userId: data.UserId ?? data.userId,
            appUserId: Number(data.AppUserId ?? data.appUserId),
            email: data.Email ?? data.email ?? '',
            roles: data.Roles ?? data.roles ?? [],
            driverId: data.DriverId ?? data.driverId,
            customerId: data.CustomerId ?? data.customerId,
            transporterId: data.TransporterId ?? data.transporterId,
            ...data
        };

        localStorage.setItem('token', token);
        localStorage.setItem('user', JSON.stringify(data));
        localStorage.setItem('userId', userData.userId);
        localStorage.setItem('appUserId', String(userData.appUserId));

        setToken(token);
        setUser(userData);
    };

    const logout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        localStorage.removeItem('userId');
        localStorage.removeItem('appUserId');
        setToken(null);
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, token, isAuthenticated: !!token, login, logout, loading }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuthContext = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuthContext must be used within an AuthProvider');
    }
    return context;
};
