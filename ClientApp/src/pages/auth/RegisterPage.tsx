import React, { useState, useEffect } from 'react';
import { useNavigate, Link, useSearchParams } from 'react-router-dom';
import { User, Mail, Lock, Building2, FileText, UserPlus, ArrowLeft, Box, Truck } from 'lucide-react';
import apiClient from '../../api/apiClient';
import { enablePushNotifications } from '../../lib/firebaseMessaging';
import { getFirebaseIdToken, loginWithEmailPassword, loginWithGooglePopup, logoutFirebaseAuth, registerWithEmailPassword, resendVerificationEmail } from '../../lib/firebaseAuth';

const EMAIL_REGEX = /^[^\s@]+@(gmail\.com|hotmail\.com|outlook\.com|yahoo\.com)$/i;
const PHONE_REGEX = /^\d{10}$/;
const GST_REGEX = /^[0-9A-Z]{15}$/;
const PASSWORD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,20}$/;
const PENDING_VERIFICATION_STORAGE_KEY = 'navgatixPendingVerification';

type PendingVerificationState = {
    email: string;
    password: string;
    type: 'driver' | 'customer';
    role: 'Driver' | 'Transporter' | 'Customer';
    formData: {
        fullName: string;
        email: string;
        password: string;
        phoneNumber: string;
        companyName: string;
        gstNumber: string;
    };
};

const RegisterPage = () => {
    const [searchParams, setSearchParams] = useSearchParams();
    const type = searchParams.get('type');

    const [role, setRole] = useState<'Driver' | 'Transporter' | 'Customer'>('Driver');
    const [formData, setFormData] = useState({
        fullName: '',
        email: '',
        password: '',
        phoneNumber: '',
        companyName: '',
        gstNumber: '',
    });
    const navigate = useNavigate();

    useEffect(() => {
        if (type === 'customer') {
            setRole('Customer');
        } else if (type === 'driver') {
            setRole('Driver');
        }
    }, [type]);

    useEffect(() => {
        const savedVerification = localStorage.getItem(PENDING_VERIFICATION_STORAGE_KEY);
        if (!savedVerification) {
            return;
        }

        try {
            const pendingState = JSON.parse(savedVerification) as PendingVerificationState;
            if (!pendingState?.email || !pendingState?.password) {
                return;
            }

            setPendingVerification({ email: pendingState.email, password: pendingState.password });
            if (pendingState.formData) {
                setFormData(pendingState.formData);
            } else {
                setFormData((current) => ({
                    ...current,
                    email: current.email || pendingState.email,
                    password: current.password || pendingState.password,
                }));
            }
            if (pendingState.role) {
                setRole(pendingState.role);
            }

            if (!type && (pendingState.type === 'driver' || pendingState.type === 'customer')) {
                setSearchParams({ type: pendingState.type });
            }
        } catch {
            localStorage.removeItem(PENDING_VERIFICATION_STORAGE_KEY);
        }
    }, [setSearchParams, type]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');
    const [infoMessage, setInfoMessage] = useState('');
    const [isGoogleLoading, setIsGoogleLoading] = useState(false);
    const [pendingVerification, setPendingVerification] = useState<{ email: string; password: string } | null>(null);
    const [isCheckingVerification, setIsCheckingVerification] = useState(false);
    const [isResendingVerification, setIsResendingVerification] = useState(false);

    const validateForm = (requirePassword: boolean) => {
        if (formData.fullName.trim().length < 2) {
            return 'Full name must be at least 2 characters.';
        }
        if (!PHONE_REGEX.test(formData.phoneNumber.trim())) {
            return 'Phone number must be exactly 10 digits.';
        }
        if (!EMAIL_REGEX.test(formData.email.trim())) {
            return 'Email must use gmail.com, hotmail.com, outlook.com, or yahoo.com.';
        }
        if (requirePassword && !PASSWORD_REGEX.test(formData.password)) {
            return 'Password must be 8-20 characters and include uppercase, lowercase, number, and special character.';
        }
        if ((role === 'Transporter' || role === 'Customer') && formData.gstNumber && !GST_REGEX.test(formData.gstNumber.trim().toUpperCase())) {
            return 'GST number must be exactly 15 uppercase letters/numbers.';
        }

        return '';
    };

    const buildFirebasePayload = (
        firebaseIdToken: string,
        provider: 'password' | 'google',
        overrides?: { email?: string; firstName?: string; lastName?: string; userName?: string }
    ) => ({
        firebaseIdToken,
        provider,
        RoleName: role,
        UserName: overrides?.userName || overrides?.email || formData.email.trim().toLowerCase(),
        Email: overrides?.email || formData.email.trim().toLowerCase(),
        FirstName: overrides?.firstName || formData.fullName.split(' ')[0] || '',
        LastName: overrides?.lastName || formData.fullName.split(' ').slice(1).join(' ') || '',
        PhoneNumber: formData.phoneNumber.trim(),
        Company: (role === 'Transporter' || role === 'Customer') ? formData.companyName.trim() : undefined,
        GSTNumber: (role === 'Transporter' || role === 'Customer') ? formData.gstNumber.trim().toUpperCase() : undefined,
        DOB: new Date().toISOString(),
    });

    const persistPendingVerification = (email: string, password: string) => {
        const pendingState: PendingVerificationState = {
            email,
            password,
            type: type === 'customer' ? 'customer' : 'driver',
            role,
            formData: {
                ...formData,
                email,
                password,
            },
        };
        localStorage.setItem(PENDING_VERIFICATION_STORAGE_KEY, JSON.stringify(pendingState));
    };

    const clearPendingVerification = () => {
        localStorage.removeItem(PENDING_VERIFICATION_STORAGE_KEY);
        setPendingVerification(null);
    };

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsLoading(true);
        setError('');
        setInfoMessage('');

        const validationMessage = validateForm(true);
        if (validationMessage) {
            setError(validationMessage);
            setIsLoading(false);
            return;
        }

        try {
            const firebaseUser = await registerWithEmailPassword(formData.email.trim().toLowerCase(), formData.password);
            const firebaseIdToken = await getFirebaseIdToken(firebaseUser);

            await apiClient.post('/User/firebaseRegister', buildFirebasePayload(firebaseIdToken, 'password'));
            setPendingVerification({
                email: formData.email.trim().toLowerCase(),
                password: formData.password,
            });
            persistPendingVerification(formData.email.trim().toLowerCase(), formData.password);
            setInfoMessage('Account created. Verification email sent. Please verify your email here before continuing.');
            await logoutFirebaseAuth();
        } catch (err: any) {
            console.error(err);
            const msg =
                err.response?.data?.Message ||
                err.response?.data?.message ||
                err.code?.replace('auth/', '').replace(/-/g, ' ') ||
                err.response?.data;
            const normalizedMessage = typeof msg === 'string' ? msg.toLowerCase() : '';
            const isExistingEmailError =
                normalizedMessage.includes('email already in use') ||
                normalizedMessage.includes('email-already-in-use') ||
                normalizedMessage.includes('already registered');

            if (isExistingEmailError && formData.email.trim() && formData.password) {
                const email = formData.email.trim().toLowerCase();
                setPendingVerification({
                    email,
                    password: formData.password,
                });
                persistPendingVerification(email, formData.password);
                setError('This email is already registered. If it is your account, verify it here or resend the verification email below.');
            } else {
                setError(typeof msg === 'string' ? msg : 'Registration failed. Please try again.');
            }
        } finally {
            setIsLoading(false);
        }
    };

    const handleResendVerificationFromRegister = async () => {
        if (!pendingVerification) {
            return;
        }

        setIsResendingVerification(true);
        setError('');
        setInfoMessage('');
        try {
            const firebaseUser = await loginWithEmailPassword(pendingVerification.email, pendingVerification.password);
            await resendVerificationEmail(firebaseUser);
            await logoutFirebaseAuth();
            setInfoMessage('Verification email sent again. Check inbox and spam folder.');
        } catch (err: any) {
            console.error(err);
            setError(err?.code?.replace('auth/', '').replace(/-/g, ' ') || 'Unable to resend verification email.');
        } finally {
            setIsResendingVerification(false);
        }
    };

    const handleCheckVerification = async () => {
        if (!pendingVerification) {
            return;
        }

        setIsCheckingVerification(true);
        setError('');
        setInfoMessage('');
        try {
            const firebaseUser = await loginWithEmailPassword(pendingVerification.email, pendingVerification.password);
            if (!firebaseUser.emailVerified) {
                await logoutFirebaseAuth();
                setInfoMessage('Email is still not verified. Open your inbox, click the verification link, then check again.');
                return;
            }

            const firebaseIdToken = await getFirebaseIdToken(firebaseUser);
            const response = await apiClient.post('/User/firebaseRegister', buildFirebasePayload(firebaseIdToken, 'password'));
            const data = response.data;
            const token = data?.Token ?? data?.token;
            const userId = data?.UserId ?? data?.userId;
            const appUserId = data?.AppUserId ?? data?.appUserId;
            const roles: string[] = data?.Roles ?? data?.roles ?? [];

            if (!token) {
                setError(data?.Message || data?.message || 'Unable to complete verified login.');
                return;
            }

            localStorage.setItem('token', token);
            localStorage.setItem('userId', userId || '');
            localStorage.setItem('appUserId', String(appUserId || ''));
            localStorage.setItem('user', JSON.stringify(data));
            clearPendingVerification();
            await enablePushNotifications(data);

            const primaryRole = roles.length > 0 ? roles[0] : (data?.roleName ?? data?.RoleName ?? '');
            navigate(
                primaryRole?.toLowerCase() === 'driver'
                    ? '/profile'
                    : primaryRole?.toLowerCase() === 'transporter'
                        ? '/transporter-dashboard'
                        : '/customer-portal'
            );
        } catch (err: any) {
            console.error(err);
            setError(err?.response?.data?.Message || err?.response?.data?.message || err?.code?.replace('auth/', '').replace(/-/g, ' ') || 'Unable to verify email status.');
        } finally {
            setIsCheckingVerification(false);
        }
    };

    const handleGoogleRegister = async () => {
        setError('');
        setInfoMessage('');
        setIsGoogleLoading(true);

        if (formData.fullName.trim().length < 2) {
            setError('Full name must be at least 2 characters.');
            setIsGoogleLoading(false);
            return;
        }
        if (!PHONE_REGEX.test(formData.phoneNumber.trim())) {
            setError('Phone number must be exactly 10 digits.');
            setIsGoogleLoading(false);
            return;
        }
        if ((role === 'Transporter' || role === 'Customer') && formData.gstNumber && !GST_REGEX.test(formData.gstNumber.trim().toUpperCase())) {
            setError('GST number must be exactly 15 uppercase letters/numbers.');
            setIsGoogleLoading(false);
            return;
        }

        const fullNameParts = formData.fullName.trim().split(/\s+/);

        try {
            const firebaseUser = await loginWithGooglePopup();
            const firebaseIdToken = await getFirebaseIdToken(firebaseUser);
            const response = await apiClient.post('/User/firebaseRegister', buildFirebasePayload(firebaseIdToken, 'google', {
                email: firebaseUser.email || formData.email.trim().toLowerCase(),
                userName: firebaseUser.email || formData.email.trim().toLowerCase(),
                firstName: fullNameParts[0] || firebaseUser.displayName?.split(' ')[0] || '',
                lastName: fullNameParts.slice(1).join(' ') || firebaseUser.displayName?.split(' ').slice(1).join(' ') || '',
            }));
            const data = response.data;
            const token = data?.Token ?? data?.token;
            const userId = data?.UserId ?? data?.userId;
            const appUserId = data?.AppUserId ?? data?.appUserId;
            const roles: string[] = data?.Roles ?? data?.roles ?? [];

            if (token) {
                clearPendingVerification();
                localStorage.setItem('token', token);
                localStorage.setItem('userId', userId || '');
                localStorage.setItem('appUserId', String(appUserId || ''));
                localStorage.setItem('user', JSON.stringify(data));
                await enablePushNotifications(data);
                const primaryRole = roles.length > 0 ? roles[0] : (data?.roleName ?? data?.RoleName ?? '');
                navigate(
                    primaryRole?.toLowerCase() === 'driver'
                        ? '/profile'
                        : primaryRole?.toLowerCase() === 'transporter'
                            ? '/transporter-dashboard'
                            : '/customer-portal'
                );
                return;
            }

            setInfoMessage(data?.message || data?.Message || 'Google account linked. Please continue to login.');
            navigate('/login');
        } catch (err: any) {
            console.error(err);
            const msg =
                err.response?.data?.Message ||
                err.response?.data?.message ||
                err.code?.replace('auth/', '').replace(/-/g, ' ') ||
                'Google sign-up failed.';
            setError(msg);
        } finally {
            setIsGoogleLoading(false);
        }
    };

    if (!type || (type !== 'driver' && type !== 'customer')) {
        return (
            <div className="min-h-screen bg-slate-50 flex flex-col items-center justify-center p-4">
                <div className="text-center mb-10">
                    <div className="w-16 h-16 bg-primary-600 rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-lg shadow-primary-500/30">
                        <Truck className="h-8 w-8 text-white" />
                    </div>
                    <h2 className="text-3xl font-extrabold text-slate-900 mb-3">Join Navgatix</h2>
                    <p className="text-slate-500 max-w-sm mx-auto">Select how you want to use our platform to get started.</p>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 gap-6 max-w-2xl w-full">
                    <button
                        onClick={() => setSearchParams({ type: 'driver' })}
                        className="bg-white p-8 rounded-3xl border border-slate-200 shadow-xl shadow-slate-200/50 hover:-translate-y-1 hover:border-primary-500 transition-all text-left group"
                    >
                        <div className="w-14 h-14 bg-slate-900 rounded-2xl flex items-center justify-center mb-6 group-hover:bg-primary-600 transition-colors">
                            <Truck className="h-6 w-6 text-white" />
                        </div>
                        <h3 className="text-2xl font-bold text-slate-900 mb-2">I am a Driver</h3>
                        <p className="text-slate-500 text-sm">Join our network, get regular loads, and manage your fleet.</p>
                    </button>

                    <button
                        onClick={() => setSearchParams({ type: 'customer' })}
                        className="bg-white p-8 rounded-3xl border border-slate-200 shadow-xl shadow-slate-200/50 hover:-translate-y-1 hover:border-emerald-500 transition-all text-left group"
                    >
                        <div className="w-14 h-14 bg-emerald-50 rounded-2xl flex items-center justify-center mb-6 group-hover:bg-emerald-100 transition-colors">
                            <Box className="h-6 w-6 text-emerald-600" />
                        </div>
                        <h3 className="text-2xl font-bold text-slate-900 mb-2">I need to Deliver</h3>
                        <p className="text-slate-500 text-sm">Book vehicles on-demand for home shifting or business supply.</p>
                    </button>
                </div>

                <Link to="/" className="mt-10 text-slate-500 hover:text-slate-900 flex items-center gap-2 font-medium">
                    <ArrowLeft className="h-4 w-4" /> Back to Home
                </Link>
            </div>
        );
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-slate-50 relative overflow-hidden py-12">
            <div className="absolute top-0 left-0 w-full h-full overflow-hidden z-0">
                <div className="absolute top-0 right-0 w-[40%] h-[40%] rounded-full bg-primary-200/40 blur-[100px]" />
                <div className="absolute bottom-0 left-0 w-[40%] h-[40%] rounded-full bg-emerald-200/40 blur-[100px]" />
            </div>

            <div className="relative z-10 w-full max-w-2xl p-4">
                <Link to="/" className="inline-flex items-center gap-2 text-slate-500 hover:text-primary-600 font-medium mb-6 transition-colors">
                    <ArrowLeft className="h-4 w-4" />
                    Back to Home
                </Link>

                <div className="premium-card p-8 md:p-12 backdrop-blur-sm bg-white/95">
                    <div className="text-center mb-10">
                        <h1 className="text-3xl font-extrabold text-slate-900 mb-3 tracking-tight">Create an Account</h1>
                        <p className="text-slate-500">Join Navgatix and streamline your fleet operations.</p>
                    </div>

                    {type === 'driver' && (
                        <div className="flex gap-4 mb-8">
                            <button
                                type="button"
                                onClick={() => setRole('Driver')}
                                className={`flex-1 p-5 rounded-xl border-2 transition-all flex flex-col items-center gap-3 ${role === 'Driver' ? 'border-primary-600 bg-primary-50 text-primary-700 shadow-sm' : 'border-slate-200 text-slate-500 hover:border-slate-300 hover:bg-slate-50'}`}
                            >
                                <User className={`h-7 w-7 ${role === 'Driver' ? 'text-primary-600' : 'text-slate-400'}`} />
                                <div className="text-center">
                                    <span className="font-bold block">Driver</span>
                                    <span className="text-[10px] opacity-70">I drive vehicle</span>
                                </div>
                            </button>
                            <button
                                type="button"
                                onClick={() => setRole('Transporter')}
                                className={`flex-1 p-5 rounded-xl border-2 transition-all flex flex-col items-center gap-3 ${role === 'Transporter' ? 'border-primary-600 bg-primary-50 text-primary-700 shadow-sm' : 'border-slate-200 text-slate-500 hover:border-slate-300 hover:bg-slate-50'}`}
                            >
                                <Building2 className={`h-7 w-7 ${role === 'Transporter' ? 'text-primary-600' : 'text-slate-400'}`} />
                                <div className="text-center">
                                    <span className="font-bold block">Transporter</span>
                                    <span className="text-[10px] opacity-70">Head of drivers</span>
                                </div>
                            </button>
                        </div>
                    )}

                    <form onSubmit={handleRegister} className="grid grid-cols-1 md:grid-cols-2 gap-x-6 gap-y-5">
                        {error && (
                            <div className="md:col-span-2 p-3 bg-red-50 text-red-700 text-sm rounded-lg border border-red-200">
                                {error}
                            </div>
                        )}
                        {infoMessage && (
                            <div className="md:col-span-2 p-3 bg-emerald-50 text-emerald-700 text-sm rounded-lg border border-emerald-200">
                                {infoMessage}
                            </div>
                        )}
                        <div className="md:col-span-2">
                            <label className="block text-sm font-semibold text-slate-700 mb-2">Full Name</label>
                            <div className="relative group">
                                <User className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 h-5 w-5 transition-colors group-focus-within:text-primary-500" />
                                <input type="text" name="fullName" className="input-field pl-11" placeholder="John Doe" onChange={handleInputChange} maxLength={50} required />
                            </div>
                        </div>

                        <div className="md:col-span-2">
                            <label className="block text-sm font-semibold text-slate-700 mb-2">Phone Number</label>
                            <div className="relative group">
                                <span className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 font-bold">+91</span>
                                <input type="tel" name="phoneNumber" className="input-field pl-14" placeholder="9876543210" onChange={(e) => setFormData({ ...formData, phoneNumber: e.target.value.replace(/\D/g, '').slice(0, 10) })} maxLength={10} required />
                            </div>
                        </div>

                        <div>
                            <label className="block text-sm font-semibold text-slate-700 mb-2">Email Address</label>
                            <div className="relative group">
                                <Mail className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 h-5 w-5 transition-colors group-focus-within:text-primary-500" />
                                <input type="email" name="email" className="input-field pl-11" placeholder="name@gmail.com" onChange={handleInputChange} maxLength={60} required />
                            </div>
                        </div>

                        <div>
                            <label className="block text-sm font-semibold text-slate-700 mb-2">Password</label>
                            <div className="relative group">
                                <Lock className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 h-5 w-5 transition-colors group-focus-within:text-primary-500" />
                                <input type="password" name="password" className="input-field pl-11" placeholder="Strong password" onChange={handleInputChange} maxLength={20} required />
                            </div>
                            <p className="text-xs text-slate-400 mt-2">Use 8-20 characters with uppercase, lowercase, number, and special character.</p>
                        </div>

                        {(role === 'Transporter' || role === 'Customer') && (
                            <div className="md:col-span-2 grid grid-cols-1 md:grid-cols-2 gap-x-6 gap-y-5 mt-2 p-6 bg-slate-50 rounded-xl border border-slate-100">
                                <div className="md:col-span-2">
                                    <h3 className="text-[15px] font-bold text-slate-800 flex items-center gap-2">
                                        <Building2 className="h-4 w-4 text-primary-600" />
                                        {role === 'Transporter' ? 'Transport Business Details' : 'Logistics Company Details'}
                                    </h3>
                                    <p className="text-xs text-slate-500 mt-1">
                                        {role === 'Transporter' ? 'Required for fleet verification.' : 'Help us understand your shipping needs.'}
                                    </p>
                                </div>
                                <div className="md:col-span-2">
                                    <label className="block text-sm font-semibold text-slate-700 mb-2">
                                        {role === 'Transporter' ? 'Company Name' : 'Company / Store Name'}
                                    </label>
                                    <div className="relative group">
                                        <Building2 className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 h-5 w-5 transition-colors group-focus-within:text-primary-500" />
                                        <input type="text" name="companyName" className="input-field pl-11 bg-white" placeholder="Logistics Inc." onChange={handleInputChange} maxLength={80} required />
                                    </div>
                                </div>
                                <div>
                                <label className="block text-sm font-semibold text-slate-700 mb-2">
                                    GST Number <span className="text-[10px] text-slate-400 font-normal">(Optional)</span>
                                </label>
                                    <div className="relative group">
                                        <FileText className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 h-5 w-5 transition-colors group-focus-within:text-primary-500" />
                                        <input type="text" name="gstNumber" className="input-field pl-11 bg-white" placeholder="27AAAAA0000A1Z5" onChange={(e) => setFormData({ ...formData, gstNumber: e.target.value.toUpperCase().replace(/[^A-Z0-9]/g, '').slice(0, 15) })} maxLength={15} />
                                    </div>
                                </div>
                            </div>
                        )}

                        <div className="md:col-span-2 mt-6">
                            <button type="submit" disabled={isLoading} className="btn-primary w-full flex items-center justify-center gap-2 text-[15px] h-12 shadow-primary-500/25 disabled:opacity-75">
                                <UserPlus className="h-5 w-5" />
                                {isLoading ? 'Creating Account...' : 'Create Account'}
                            </button>
                        </div>
                        {pendingVerification && (
                            <div className="md:col-span-2 grid gap-3">
                                <button type="button" onClick={handleCheckVerification} disabled={isCheckingVerification} className="w-full h-12 rounded-xl bg-emerald-600 text-white font-semibold hover:bg-emerald-500 transition-colors">
                                    {isCheckingVerification ? 'Checking...' : 'I Have Verified My Email'}
                                </button>
                                <button type="button" onClick={handleResendVerificationFromRegister} disabled={isResendingVerification} className="w-full h-12 rounded-xl border border-slate-300 font-semibold text-slate-700 hover:bg-slate-50 transition-colors">
                                    {isResendingVerification ? 'Sending...' : 'Resend Verification Email'}
                                </button>
                            </div>
                        )}
                        <div className="md:col-span-2">
                            <button type="button" onClick={handleGoogleRegister} disabled={isGoogleLoading} className="w-full h-12 rounded-xl border border-slate-300 font-semibold text-slate-700 hover:bg-slate-50 transition-colors">
                                {isGoogleLoading ? 'Connecting...' : 'Continue with Google'}
                            </button>
                        </div>
                    </form>

                    <div className="mt-8 text-center text-sm">
                        <span className="text-slate-500">Already have an account? </span>
                        <Link to="/login" className="text-primary-600 font-bold hover:text-primary-700 transition-colors">Sign in here</Link>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RegisterPage;
