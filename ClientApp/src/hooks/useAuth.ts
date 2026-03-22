import { useAuthContext } from '../context/AuthContext';

export const useAuth = () => {
    const { user, token, isAuthenticated, login, logout, loading } = useAuthContext();

    return {
        user,
        token,
        isAuthenticated,
        login,
        logout,
        loading,
        userId: user?.userId,
        appUserId: user?.appUserId,
        email: user?.email,
        roles: user?.roles,
        driverId: user?.driverId,
        customerId: user?.customerId,
        transporterId: user?.transporterId
    };
};
