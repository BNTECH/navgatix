import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { Mail, Lock, LogIn, ArrowLeft } from 'lucide-react';
import apiClient from '../../api/apiClient';
import { enablePushNotifications } from '../../lib/firebaseMessaging';
import {
    getFirebaseIdToken,
    loginWithEmailPassword,
    loginWithGooglePopup,
    sendForgotPasswordEmail,
    logoutFirebaseAuth,
} from '../../lib/firebaseAuth';

import { useAuth } from '../../hooks/useAuth';

const PENDING_VERIFICATION_STORAGE_KEY = 'navgatixPendingVerification';

const LoginPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    const { login } = useAuth();

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');
    const [infoMessage, setInfoMessage] = useState('');
    const [isSendingReset, setIsSendingReset] = useState(false);
    const [isGoogleLoading, setIsGoogleLoading] = useState(false);

    const normalizeRole = (value: any) => String(value || '').trim().toLowerCase();

    // Backend can return multiple identity roles (Roles[]) plus a resolved primary role (RoleName).
    // Use RoleName first (it is derived from AccountTypeId), then fall back to Roles[].
    const resolveEffectiveRole = (data: any) => {
        const explicit = data?.roleName ?? data?.RoleName ?? data?.role ?? data?.Role ?? '';
        const explicitNorm = normalizeRole(explicit);
        if (explicitNorm) return explicitNorm;

        const roles: string[] = data?.Roles ?? data?.roles ?? [];
        const normalized = roles.map(normalizeRole).filter(Boolean);
        // Prefer business routing roles if present (ordering in Roles[] is not reliable).
        for (const preferred of ['transporter', 'company', 'driver', 'customer']) {
            if (normalized.includes(preferred)) return preferred;
        }
        return normalized[0] || '';
    };

    const getDashboardRoute = (role: string, data?: any) => {
        const r = normalizeRole(role);
        const profileStatus = data?.profileStatus || data?.ProfileStatus;
        const roles: string[] = (data?.Roles ?? data?.roles ?? []).map(normalizeRole);
        
        // Transporter should win even if the user accidentally has multiple identity roles.
        if (r === 'transporter' || r === 'company' || roles.includes('transporter') || roles.includes('company') || data?.TransporterId || data?.transporterId) {
            return '/transporter-dashboard';
        }
        if (r === 'driver' || data?.DriverId || data?.driverId) {
            return profileStatus === 'Completed' ? '/driver-dashboard' : '/profile';
        }
        if (r === 'customer' || data?.CustomerId || data?.customerId) return '/customer-portal';
        return '/';
    };

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsLoading(true);
        setError('');
        setInfoMessage('');

        try {
            const firebaseUser = await loginWithEmailPassword(email.trim().toLowerCase(), password);
            if (!firebaseUser.emailVerified) {
                await logoutFirebaseAuth();
                const savedVerification = localStorage.getItem(PENDING_VERIFICATION_STORAGE_KEY);
                if (savedVerification) {
                    try {
                        const pendingState = JSON.parse(savedVerification) as { email?: string; type?: string };
                        if (pendingState.email?.toLowerCase() === email.trim().toLowerCase() &&
                            (pendingState.type === 'driver' || pendingState.type === 'customer')) {
                            navigate(`/register?type=${pendingState.type}`);
                            return;
                        }
                    } catch {
                        localStorage.removeItem(PENDING_VERIFICATION_STORAGE_KEY);
                    }
                }

                setError('Email is not verified yet. Open the registration page for this account and complete verification there.');
                return;
            }

            const firebaseIdToken = await getFirebaseIdToken(firebaseUser);
            const response = await apiClient.post('/User/firebaseLogin', {
                firebaseIdToken,
            });

            const data = response.data;
            const isAuthenticated = data?.IsAuthenticated ?? data?.isAuthenticated;
            const token = data?.Token ?? data?.token;
            const message = data?.Message ?? data?.message;

            if (!isAuthenticated || !token) {
                setError(message || 'Login failed. Please check your credentials.');
                return;
            }

            login(token, data);
            await enablePushNotifications(data);

            const effectiveRole = resolveEffectiveRole(data);
            navigate(getDashboardRoute(effectiveRole, data));
        } catch (err: any) {
            console.error(err);
            const msg =
                err.response?.data?.Message ||
                err.response?.data?.message ||
                err.code?.replace('auth/', '').replace(/-/g, ' ') ||
                err.response?.data;
            setError(typeof msg === 'string' ? msg : 'Login failed. Please check your email and password.');
        } finally {
            setIsLoading(false);
        }
    };

    const handleGoogleLogin = async () => {
        setIsGoogleLoading(true);
        setError('');
        setInfoMessage('');

        try {
            const firebaseUser = await loginWithGooglePopup();
            const firebaseIdToken = await getFirebaseIdToken(firebaseUser);
            const response = await apiClient.post('/User/firebaseLogin', {
                firebaseIdToken,
                provider: 'google',
            });

            const data = response.data;
            const isAuthenticated = data?.IsAuthenticated ?? data?.isAuthenticated;
            const token = data?.Token ?? data?.token;
            const message = data?.Message ?? data?.message;

            if (!isAuthenticated || !token) {
                setError(message || 'Google sign-in failed. If this is a new account, please register first.');
                return;
            }

            login(token, data);
            await enablePushNotifications(data);

            const effectiveRole = resolveEffectiveRole(data);
            navigate(getDashboardRoute(effectiveRole, data));
        } catch (err: any) {
            console.error(err);
            const msg =
                err.response?.data?.Message ||
                err.response?.data?.message ||
                err.code?.replace('auth/', '').replace(/-/g, ' ') ||
                'Google sign-in failed.';
            setError(msg);
        } finally {
            setIsGoogleLoading(false);
        }
    };

    const handleForgotPassword = async () => {
        if (!email.trim()) {
            setError('Enter your email first, then click Forgot password.');
            return;
        }

        setIsSendingReset(true);
        setError('');
        setInfoMessage('');
        try {
            await sendForgotPasswordEmail(email.trim().toLowerCase());
            setInfoMessage('Password reset email sent. Check your inbox.');
        } catch (err: any) {
            console.error(err);
            setError(err?.code?.replace('auth/', '').replace(/-/g, ' ') || 'Unable to send password reset email.');
        } finally {
            setIsSendingReset(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-slate-50 relative overflow-hidden">
            <div className="absolute top-0 left-0 w-full h-full overflow-hidden z-0">
                <div className="absolute -top-[20%] -right-[10%] w-[50%] h-[50%] rounded-full bg-primary-200/50 blur-[100px]" />
                <div className="absolute -bottom-[20%] -left-[10%] w-[50%] h-[50%] rounded-full bg-indigo-200/50 blur-[100px]" />
            </div>

            <div className="relative z-10 w-full max-w-md p-4">
                <Link to="/" className="inline-flex items-center gap-2 text-slate-500 hover:text-primary-600 font-medium mb-8 transition-colors">
                    <ArrowLeft className="h-4 w-4" />
                    Back to Home
                </Link>

                <div className="premium-card p-10 backdrop-blur-sm bg-white/90">
                    <div className="text-center mb-10">
                        <img src="/logo.png" alt="Navgatix" className="h-14 mx-auto mb-4 object-contain" />
                        <h1 className="text-3xl font-extrabold text-slate-900 mb-3 tracking-tight">Welcome Back</h1>
                        <p className="text-slate-500">Sign in to your Navgatix account.</p>
                    </div>

                    <form onSubmit={handleLogin} className="space-y-6">
                        {error && (
                            <div className="p-3 bg-red-50 text-red-700 text-sm rounded-lg border border-red-200">
                                {error}
                            </div>
                        )}
                        {infoMessage && (
                            <div className="p-3 bg-emerald-50 text-emerald-700 text-sm rounded-lg border border-emerald-200">
                                {infoMessage}
                            </div>
                        )}
                        <div>
                            <label className="block text-sm font-semibold text-slate-700 mb-2">Email Address</label>
                            <div className="relative group">
                                <Mail className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 h-5 w-5 transition-colors group-focus-within:text-primary-500" />
                                <input
                                    type="email"
                                    className="input-field pl-11"
                                    placeholder="name@company.com"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                />
                            </div>
                        </div>

                        <div>
                            <label className="block text-sm font-semibold text-slate-700 mb-2">Password</label>
                            <div className="relative group">
                                <Lock className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 h-5 w-5 transition-colors group-focus-within:text-primary-500" />
                                <input
                                    type="password"
                                    className="input-field pl-11"
                                    placeholder="********"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                />
                            </div>
                        </div>

                        <div className="flex items-center justify-between mt-4">
                            <label className="flex items-center gap-2 cursor-pointer">
                                <input type="checkbox" className="rounded border-slate-300 text-primary-600 focus:ring-primary-500" />
                                <span className="text-sm text-slate-600">Remember me</span>
                            </label>
                            <button type="button" onClick={handleForgotPassword} disabled={isSendingReset} className="text-sm text-primary-600 font-semibold hover:text-primary-700 transition-colors">
                                {isSendingReset ? 'Sending...' : 'Forgot password?'}
                            </button>
                        </div>

                        <div className="pt-2">
                            <button type="submit" disabled={isLoading} className="btn-primary w-full flex items-center justify-center gap-2 text-[15px] h-12 shadow-primary-500/25 disabled:opacity-75">
                                <LogIn className="h-5 w-5" />
                                {isLoading ? 'Signing In...' : 'Sign In'}
                            </button>
                        </div>
                        <button type="button" onClick={handleGoogleLogin} disabled={isGoogleLoading} className="w-full h-12 rounded-xl border border-slate-300 font-semibold text-slate-700 hover:bg-slate-50 transition-colors">
                            {isGoogleLoading ? 'Connecting...' : 'Continue with Google'}
                        </button>
                    </form>

                    <div className="mt-8 text-center text-sm">
                        <span className="text-slate-500">Don't have an account? </span>
                        <Link to="/register" className="text-primary-600 font-bold hover:text-primary-700 transition-colors">Create one now</Link>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default LoginPage;
