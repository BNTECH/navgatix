import { useEffect, useMemo, useState } from 'react';
import { DollarSign, Truck, Clock, Wallet, Send, MapPin, Route, History, MessageCircle } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../../api/apiClient';
import ChatPanel from '../../components/ChatPanel';

type RideStatus = 'request_for_ride' | 'driver_assigned' | 'driver_arriving' | 'ride_started' | 'ride_completed' | 'cancelled';

type RideItem = {
    id: number;
    customerName?: string;
    pickupAddress?: string;
    dropAddress?: string;
    goodsType?: string;
    estimatedFare?: number;
    finalFare?: number;
    rideStatus?: RideStatus;
    createdAt?: string;
    scheduledTime?: string;
};

const ACTIVE_STATUSES: RideStatus[] = ['driver_assigned', 'driver_arriving', 'ride_started'];
const STATUS_OPTIONS: RideStatus[] = ['driver_arriving', 'ride_started', 'ride_completed', 'cancelled'];

const DriverDashboard = () => {
    const navigate = useNavigate();
    const [user, setUser] = useState<any>(null);
    const [wallet, setWallet] = useState<any>(null);
    const [loadingWallet, setLoadingWallet] = useState(false);
    const [rideRequests, setRideRequests] = useState<RideItem[]>([]);
    const [rides, setRides] = useState<RideItem[]>([]);
    const [loadingRides, setLoadingRides] = useState(false);
    const [statusDrafts, setStatusDrafts] = useState<Record<number, RideStatus>>({});
    const [disputeDrafts, setDisputeDrafts] = useState<Record<number, string>>({});
    const [withdrawAmount, setWithdrawAmount] = useState('');
    const [isTracking, setIsTracking] = useState(false);
    const [currentPosition, setCurrentPosition] = useState<{ lat: number; lng: number } | null>(null);
    const [chatBookingId, setChatBookingId] = useState<number | null>(null);

    const driverUserId = user?.userId || user?.UserId || user?.id || '';
    const driverId = user?.driverId || user?.DriverId || '';
    const appUserId = Number(user?.appUserId || user?.AppUserId || 0);

    useEffect(() => {
        const userStr = localStorage.getItem('user');
        if (!userStr) {
            navigate('/login');
            return;
        }
        const userData = JSON.parse(userStr);
        setUser(userData);

        // Profile Completion Guard
        const profileStatus = userData?.profileStatus || userData?.ProfileStatus;
        if (profileStatus === 'Incomplete') {
            console.log('Driver profile is incomplete. Redirecting to profile page.');
            navigate('/profile', { state: { from: 'dashboard', reason: 'incomplete_profile' } });
        }
    }, [navigate]);

    useEffect(() => {
        if (!driverUserId) return;

        const loadWallet = async () => {
            setLoadingWallet(true);
            try {
                const res = await apiClient.get(`/DriverFinance/wallet/${driverUserId}`);
                setWallet(res.data || {});
            } catch (err) {
                console.error(err);
            } finally {
                setLoadingWallet(false);
            }
        };

        const loadRides = async () => {
            setLoadingRides(true);
            try {
                const requestRes = await apiClient.get(`/Vehicle/driverRideRequests/${driverUserId}`);
                const requestData = Array.isArray(requestRes.data) ? requestRes.data : [];
                setRideRequests(
                    requestData
                        .filter((item) => item?.Id || item?.id)
                        .map((item) => ({
                            id: Number(item.id ?? item.Id),
                            customerName: item.customerName ?? item.CustomerName,
                            pickupAddress: item.pickupAddress ?? item.PickupAddress,
                            dropAddress: item.dropAddress ?? item.DropAddress,
                            goodsType: item.goodsType ?? item.GoodsType,
                            estimatedFare: Number(item.estimatedFare ?? item.EstimatedFare ?? 0),
                            finalFare: Number(item.finalFare ?? item.FinalFare ?? 0),
                            rideStatus: (item.rideStatus ?? item.RideStatus ?? 'request_for_ride') as RideStatus,
                            createdAt: item.createdAt ?? item.CreatedAt,
                            scheduledTime: item.scheduledTime ?? item.ScheduledTime,
                        }))
                );

                const res = await apiClient.get(`/Vehicle/driverRides/${driverUserId}`);
                const data = Array.isArray(res.data) ? res.data : [];
                const normalized = data
                    .filter((item) => item?.Id || item?.id)
                    .map((item) => ({
                        id: Number(item.id ?? item.Id),
                        customerName: item.customerName ?? item.CustomerName,
                        pickupAddress: item.pickupAddress ?? item.PickupAddress,
                        dropAddress: item.dropAddress ?? item.DropAddress,
                        goodsType: item.goodsType ?? item.GoodsType,
                        estimatedFare: Number(item.estimatedFare ?? item.EstimatedFare ?? 0),
                        finalFare: Number(item.finalFare ?? item.FinalFare ?? 0),
                        rideStatus: (item.rideStatus ?? item.RideStatus ?? 'request_for_ride') as RideStatus,
                        createdAt: item.createdAt ?? item.CreatedAt,
                        scheduledTime: item.scheduledTime ?? item.ScheduledTime,
                    }));

                setRides(normalized);
                setStatusDrafts(
                    normalized.reduce<Record<number, RideStatus>>((acc, ride) => {
                        acc[ride.id] = ride.rideStatus || 'driver_arriving';
                        return acc;
                    }, {})
                );
            } catch (err) {
                console.error(err);
            } finally {
                setLoadingRides(false);
            }
        };

        loadWallet();
        loadRides();
    }, [driverUserId]);

    // Background Geolocation Tracking
    useEffect(() => {
        if (!driverUserId || !isTracking) return;

        const watchId = navigator.geolocation.watchPosition(
            (pos) => {
                setCurrentPosition({ lat: pos.coords.latitude, lng: pos.coords.longitude });
            },
            (err) => console.error('Geolocation error:', err),
            { enableHighAccuracy: true }
        );

        return () => navigator.geolocation.clearWatch(watchId);
    }, [driverUserId, isTracking]);

    // Periodic Backend Ping (every 5 seconds)
    useEffect(() => {
        if (!driverUserId || !isTracking || !currentPosition) return;

        const interval = setInterval(async () => {
            try {
                // Get vehicleId - in a real app this would come from the user's profile or a specific fetch
                // For now we assume the driver has one primary vehicle
                const vehicleId = user?.vehicleId || user?.VehicleId;
                if (!vehicleId) return;

                await apiClient.post('/Vehicle/saveLiveVehicleTracking', {
                    vehicleId: vehicleId,
                    deviceId: 'web-browser',
                    lastLatitude: currentPosition.lat,
                    lastLongitude: currentPosition.lng
                });
                console.log('Location ping successful');
            } catch (err) {
                console.error('Location ping failed:', err);
            }
        }, 5000);

        return () => clearInterval(interval);
    }, [driverUserId, isTracking, currentPosition, user]);

    const currentRide = useMemo(
        () => rides.find((ride) => ride.rideStatus && ACTIVE_STATUSES.includes(ride.rideStatus)) || null,
        [rides]
    );

    const rideHistory = useMemo(
        () => rides.filter((ride) => !currentRide || ride.id !== currentRide.id),
        [rides, currentRide]
    );

    const toCurrency = (value: any) => `Rs ${Number(value || 0).toLocaleString()}`;

    const refreshRides = async () => {
        if (!driverUserId) return;
        const requestRes = await apiClient.get(`/Vehicle/driverRideRequests/${driverUserId}`);
        const requestData = Array.isArray(requestRes.data) ? requestRes.data : [];
        setRideRequests(
            requestData
                .filter((item) => item?.Id || item?.id)
                .map((item) => ({
                    id: Number(item.id ?? item.Id),
                    customerName: item.customerName ?? item.CustomerName,
                    pickupAddress: item.pickupAddress ?? item.PickupAddress,
                    dropAddress: item.dropAddress ?? item.DropAddress,
                    goodsType: item.goodsType ?? item.GoodsType,
                    estimatedFare: Number(item.estimatedFare ?? item.EstimatedFare ?? 0),
                    finalFare: Number(item.finalFare ?? item.FinalFare ?? 0),
                    rideStatus: (item.rideStatus ?? item.RideStatus ?? 'request_for_ride') as RideStatus,
                    createdAt: item.createdAt ?? item.CreatedAt,
                    scheduledTime: item.scheduledTime ?? item.ScheduledTime,
                }))
        );

        const res = await apiClient.get(`/Vehicle/driverRides/${driverUserId}`);
        const data = Array.isArray(res.data) ? res.data : [];
        const normalized = data
            .filter((item) => item?.Id || item?.id)
            .map((item) => ({
                id: Number(item.id ?? item.Id),
                customerName: item.customerName ?? item.CustomerName,
                pickupAddress: item.pickupAddress ?? item.PickupAddress,
                dropAddress: item.dropAddress ?? item.DropAddress,
                goodsType: item.goodsType ?? item.GoodsType,
                estimatedFare: Number(item.estimatedFare ?? item.EstimatedFare ?? 0),
                finalFare: Number(item.finalFare ?? item.FinalFare ?? 0),
                rideStatus: (item.rideStatus ?? item.RideStatus ?? 'request_for_ride') as RideStatus,
                createdAt: item.createdAt ?? item.CreatedAt,
                scheduledTime: item.scheduledTime ?? item.ScheduledTime,
            }));
        setRides(normalized);
    };

    const acceptRide = async (ride: RideItem) => {
        if (!driverId) {
            alert('Driver id is missing for this account.');
            return;
        }

        try {
            const res = await apiClient.patch(`/Vehicle/${ride.id}/rideStatus`, null, {
                params: { status: 'driver_assigned', driverId },
            });
            alert(res.data?.message || res.data?.Message || 'Ride accepted.');
            await refreshRides();
        } catch (err: any) {
            alert(err?.response?.data?.message || err?.response?.data?.Message || 'Unable to accept ride.');
        }
    };

    const rejectRide = async (ride: RideItem) => {
        if (!driverUserId) {
            alert('Driver user id is missing for this account.');
            return;
        }

        try {
            const res = await apiClient.patch(`/Vehicle/${ride.id}/rideRequest/reject`, null, {
                params: { driverUserId },
            });
            alert(res.data?.message || res.data?.Message || 'Ride rejected.');
            await refreshRides();
        } catch (err: any) {
            alert(err?.response?.data?.message || err?.response?.data?.Message || 'Unable to reject ride.');
        }
    };

    const updateRideStatus = async (ride: RideItem) => {
        const nextStatus = statusDrafts[ride.id];
        if (!nextStatus) return;

        try {
            const params: any = { status: nextStatus };
            if (nextStatus === 'driver_assigned' && driverId) {
                params.driverId = driverId;
            }

            const res = await apiClient.patch(`/Vehicle/${ride.id}/rideStatus`, null, { params });
            alert(res.data?.message || res.data?.Message || 'Ride status updated.');
            await refreshRides();
        } catch (err: any) {
            alert(err?.response?.data?.message || err?.response?.data?.Message || 'Ride status update failed.');
        }
    };

    const requestWithdrawal = async () => {
        const amount = Number(withdrawAmount);
        if (!driverUserId || !amount || amount <= 0) {
            alert('Enter valid withdrawal amount.');
            return;
        }
        try {
            const payload = { driverUserId, amount, note: 'Driver dashboard withdrawal request' };
            const res = await apiClient.post('/DriverFinance/withdrawal/request', payload);
            alert(res.data?.message || res.data?.Message || 'Withdrawal requested.');
            setWithdrawAmount('');
            const refreshed = await apiClient.get(`/DriverFinance/wallet/${driverUserId}`);
            setWallet(refreshed.data || {});
        } catch (err: any) {
            alert(err?.response?.data?.message || err?.response?.data?.Message || 'Withdrawal request failed.');
        }
    };

    const reportRideIssue = async (ride: RideItem) => {
        const description = (disputeDrafts[ride.id] || '').trim();
        if (!description) {
            alert('Please write issue details first.');
            return;
        }

        try {
            const payload = {
                rideId: ride.id,
                issueType: 'ride_issue',
                description,
                createdBy: appUserId,
            };
            const res = await apiClient.post('/Dispute/reportRideIssue', payload);
            alert(res.data?.message || res.data?.Message || 'Ride issue reported.');
            setDisputeDrafts((prev) => ({ ...prev, [ride.id]: '' }));
        } catch (err: any) {
            alert(err?.response?.data?.message || err?.response?.data?.Message || 'Unable to report ride issue.');
        }
    };

    if (!user) return null;

    return (
        <>
            <div className="min-h-screen bg-slate-50 font-sans p-6">
            <div className="max-w-7xl mx-auto">
                <div className="mb-8">
                    <h1 className="text-3xl font-extrabold text-slate-900">Driver Control Panel</h1>
                    <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mt-2">
                        <p className="text-slate-500">Current ride controls, ride history, wallet summary, withdrawals, and issue reporting.</p>
                        <div className="flex items-center gap-3 bg-white border border-slate-200 rounded-2xl px-4 py-2 shadow-sm">
                            <div className="flex flex-col items-end">
                                <p className="text-xs font-bold text-slate-400 uppercase tracking-wider">Tracking Status</p>
                                <p className={`text-sm font-bold ${isTracking ? 'text-emerald-600' : 'text-slate-400'}`}>
                                    {isTracking ? 'LIVE & ONLINE' : 'OFFLINE'}
                                </p>
                            </div>
                            <button 
                                onClick={() => setIsTracking(!isTracking)}
                                className={`relative inline-flex h-6 w-11 flex-shrink-0 cursor-pointer rounded-full border-2 border-transparent transition-colors duration-200 ease-in-out focus:outline-none ${isTracking ? 'bg-emerald-500' : 'bg-slate-200'}`}
                            >
                                <span className={`pointer-events-none inline-block h-5 w-5 transform rounded-full bg-white shadow ring-0 transition duration-200 ease-in-out ${isTracking ? 'translate-x-5' : 'translate-x-0'}`} />
                            </button>
                        </div>
                    </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
                    <div className="bg-white p-6 rounded-2xl border border-slate-200 shadow-sm">
                        <p className="text-sm font-bold text-slate-500 mb-1">Current Balance</p>
                        <h3 className="text-2xl font-black text-slate-900">{toCurrency(wallet?.currentBalance)}</h3>
                        <Wallet className="h-5 w-5 text-emerald-600 mt-3" />
                    </div>
                    <div className="bg-white p-6 rounded-2xl border border-slate-200 shadow-sm">
                        <p className="text-sm font-bold text-slate-500 mb-1">Total Earnings</p>
                        <h3 className="text-2xl font-black text-slate-900">{toCurrency(wallet?.totalEarnings)}</h3>
                        <DollarSign className="h-5 w-5 text-blue-600 mt-3" />
                    </div>
                    <div className="bg-white p-6 rounded-2xl border border-slate-200 shadow-sm">
                        <p className="text-sm font-bold text-slate-500 mb-1">Ride Payments</p>
                        <h3 className="text-2xl font-black text-slate-900">{toCurrency(wallet?.totalRidePayments)}</h3>
                        <Truck className="h-5 w-5 text-indigo-600 mt-3" />
                    </div>
                    <div className="bg-white p-6 rounded-2xl border border-slate-200 shadow-sm">
                        <p className="text-sm font-bold text-slate-500 mb-1">Pending Withdrawals</p>
                        <h3 className="text-2xl font-black text-slate-900">{wallet?.pendingWithdrawalCount || 0}</h3>
                        <p className="text-xs text-slate-500 mt-1">{toCurrency(wallet?.pendingWithdrawalAmount)}</p>
                        <Clock className="h-5 w-5 text-amber-600 mt-2" />
                    </div>
                </div>

                {(loadingWallet || loadingRides) && <p className="text-sm text-slate-500 mb-6">Loading driver data...</p>}

                <div className="bg-white rounded-2xl border border-slate-200 shadow-sm p-6 mb-8">
                    <h3 className="text-lg font-bold text-slate-900 mb-4">Pending Ride Requests</h3>
                    {rideRequests.length === 0 ? (
                        <div className="rounded-2xl border border-dashed border-slate-300 bg-slate-50 p-6 text-sm text-slate-500">
                            No pending ride requests for you right now.
                        </div>
                    ) : (
                        <div className="space-y-4">
                            {rideRequests.map((ride) => (
                                <div key={ride.id} className="rounded-2xl border border-slate-200 p-5">
                                    <div className="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
                                        <div className="space-y-2">
                                            <div className="flex items-center gap-3 flex-wrap">
                                                <p className="text-base font-bold text-slate-900">Ride #{ride.id}</p>
                                                <span className="rounded-full bg-amber-50 px-3 py-1 text-xs font-semibold text-amber-700 border border-amber-200">
                                                    Pending acceptance
                                                </span>
                                            </div>
                                            <div className="text-sm text-slate-600 space-y-1">
                                                <p><span className="font-semibold text-slate-800">Pickup:</span> {ride.pickupAddress || 'N/A'}</p>
                                                <p><span className="font-semibold text-slate-800">Drop:</span> {ride.dropAddress || 'N/A'}</p>
                                                <p><span className="font-semibold text-slate-800">Goods:</span> {ride.goodsType || 'N/A'}</p>
                                                <p><span className="font-semibold text-slate-800">Estimated Fare:</span> {toCurrency(ride.estimatedFare)}</p>
                                            </div>
                                        </div>
                                        <div className="flex flex-col gap-3 sm:flex-row">
                                            <button
                                                onClick={() => acceptRide(ride)}
                                                className="rounded-xl bg-emerald-600 text-white font-semibold px-6 py-3"
                                            >
                                                Accept Ride
                                            </button>
                                            <button
                                                onClick={() => rejectRide(ride)}
                                                className="rounded-xl border border-red-300 text-red-700 font-semibold px-6 py-3"
                                            >
                                                Reject Ride
                                            </button>
                                            <button
                                                onClick={() => setChatBookingId(ride.id)}
                                                className="rounded-xl border border-slate-300 bg-white text-slate-700 font-semibold px-5 py-3 flex items-center gap-2 hover:bg-slate-50 transition-colors"
                                            >
                                                <MessageCircle className="h-4 w-4 text-emerald-600" />
                                                Chat
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-8">
                    <div className="bg-white rounded-2xl border border-slate-200 shadow-sm p-6">
                        <h3 className="text-lg font-bold text-slate-900 mb-4">Current Ride Status</h3>
                        {currentRide ? (
                            <div className="space-y-4">
                                <div className="rounded-2xl border border-primary-100 bg-primary-50 p-4">
                                    <div className="flex items-center justify-between gap-3">
                                        <div>
                                            <p className="text-sm font-bold text-slate-900">Ride #{currentRide.id}</p>
                                            <p className="text-xs text-slate-500 mt-1">Current status: {currentRide.rideStatus}</p>
                                        </div>
                                        <div className="flex items-center gap-2">
                                            <span className="rounded-full bg-white px-3 py-1 text-xs font-bold text-primary-700 border border-primary-200">
                                                {currentRide.customerName || 'Assigned Customer'}
                                            </span>
                                            <button
                                                onClick={() => setChatBookingId(currentRide.id)}
                                                className="flex items-center gap-1.5 rounded-full bg-emerald-600 text-white px-3 py-1 text-xs font-bold hover:bg-emerald-500 transition-colors"
                                            >
                                                <MessageCircle className="h-3 w-3" /> Chat
                                            </button>
                                        </div>
                                    </div>
                                    <div className="mt-4 space-y-2 text-sm text-slate-600">
                                        <div className="flex items-start gap-2">
                                            <MapPin className="h-4 w-4 text-emerald-600 mt-0.5" />
                                            <span>{currentRide.pickupAddress || 'Pickup location not available'}</span>
                                        </div>
                                        <div className="flex items-start gap-2">
                                            <Route className="h-4 w-4 text-rose-600 mt-0.5" />
                                            <span>{currentRide.dropAddress || 'Drop location not available'}</span>
                                        </div>
                                    </div>
                                </div>

                                <select
                                    value={statusDrafts[currentRide.id] || currentRide.rideStatus || 'driver_arriving'}
                                    onChange={(e) => setStatusDrafts((prev) => ({ ...prev, [currentRide.id]: e.target.value as RideStatus }))}
                                    className="w-full rounded-xl border border-slate-300 px-4 py-3 bg-white"
                                >
                                    {STATUS_OPTIONS.map((status) => (
                                        <option key={status} value={status}>{status}</option>
                                    ))}
                                </select>
                                <button onClick={() => updateRideStatus(currentRide)} className="w-full rounded-xl bg-slate-900 text-white font-semibold py-3">
                                    Update Current Ride Status
                                </button>
                            </div>
                        ) : (
                            <div className="rounded-2xl border border-dashed border-slate-300 bg-slate-50 p-6 text-sm text-slate-500">
                                Accept a pending ride request first. After acceptance, the current ride and its status controls will appear here automatically.
                            </div>
                        )}
                    </div>

                    <div className="bg-white rounded-2xl border border-slate-200 shadow-sm p-6">
                        <h3 className="text-lg font-bold text-slate-900 mb-4">Request Withdrawal</h3>
                        <div className="space-y-3">
                            <input
                                value={withdrawAmount}
                                onChange={(e) => setWithdrawAmount(e.target.value)}
                                className="w-full rounded-xl border border-slate-300 px-4 py-3"
                                placeholder="Amount"
                                type="number"
                            />
                            <button onClick={requestWithdrawal} className="w-full rounded-xl bg-emerald-600 text-white font-semibold py-3">
                                Submit Withdrawal Request
                            </button>
                        </div>
                    </div>
                </div>

                <div className="bg-white rounded-2xl border border-slate-200 shadow-sm p-6">
                    <h3 className="text-lg font-bold text-slate-900 mb-4 flex items-center gap-2">
                        <History className="h-5 w-5 text-slate-700" /> Ride History & Issue Reporting
                    </h3>

                    {rideHistory.length === 0 ? (
                        <div className="rounded-2xl border border-dashed border-slate-300 bg-slate-50 p-6 text-sm text-slate-500">
                            No previous rides found for this driver yet.
                        </div>
                    ) : (
                        <div className="space-y-4">
                            {rideHistory.map((ride) => (
                                <div key={ride.id} className="rounded-2xl border border-slate-200 p-5">
                                    <div className="flex flex-col gap-4 lg:flex-row lg:items-start lg:justify-between">
                                        <div className="space-y-2">
                                            <div className="flex items-center gap-3 flex-wrap">
                                                <p className="text-base font-bold text-slate-900">Ride #{ride.id}</p>
                                                <span className="rounded-full bg-slate-100 px-3 py-1 text-xs font-semibold text-slate-700">
                                                    {ride.rideStatus}
                                                </span>
                                                <span className="text-xs text-slate-500">
                                                    {ride.customerName || 'Customer not available'}
                                                </span>
                                            </div>
                                            <div className="text-sm text-slate-600 space-y-1">
                                                <p><span className="font-semibold text-slate-800">Pickup:</span> {ride.pickupAddress || 'N/A'}</p>
                                                <p><span className="font-semibold text-slate-800">Drop:</span> {ride.dropAddress || 'N/A'}</p>
                                                <p><span className="font-semibold text-slate-800">Goods:</span> {ride.goodsType || 'N/A'}</p>
                                                <p><span className="font-semibold text-slate-800">Fare:</span> {toCurrency(ride.finalFare || ride.estimatedFare)}</p>
                                            </div>
                                        </div>

                                        <div className="w-full lg:w-[420px] space-y-3">
                                            <textarea
                                                value={disputeDrafts[ride.id] || ''}
                                                onChange={(e) => setDisputeDrafts((prev) => ({ ...prev, [ride.id]: e.target.value }))}
                                                className="w-full rounded-xl border border-slate-300 px-4 py-3 min-h-[96px] resize-none"
                                                placeholder="Report issue for this ride"
                                            />
                                            <button
                                                onClick={() => reportRideIssue(ride)}
                                                className="w-full rounded-xl bg-red-600 text-white font-semibold py-3 flex items-center justify-center gap-2"
                                            >
                                                <Send className="h-4 w-4" /> Report Issue For Ride #{ride.id}
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>

        {/* Live Chat Panel */}
        {chatBookingId !== null && (
            <ChatPanel
                bookingId={chatBookingId as number}
                currentUserName={
                    user?.firstName ||
                    user?.name ||
                    user?.UserName ||
                    user?.userName ||
                    'Driver'
                }
                onClose={() => setChatBookingId(null)}
            />
        )}
        </>
    );
};

export default DriverDashboard;
