import { useState, useEffect, useMemo, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../../api/apiClient';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import {
    Users,
    Truck,
    Plus,
    LayoutDashboard,
    LogOut,
    Search,
    CheckCircle2,
    Clock,
    MoreVertical,
    FileText,
    Settings,
    Bell,
    AlertTriangle,
    CheckCircle,
    XCircle,
    Package
} from 'lucide-react';
import VehicleModal from '../../components/VehicleModal';
import DriverModal from '../../components/DriverModal';
import LiveFleetMap from '../../components/LiveFleetMap';
import TransporterRideRequests from '../../components/TransporterRideRequests';
import TransporterReports from '../../components/TransporterReports';

type TransporterSummary = {
    totalFleet: number;
    activeDrivers: number;
    ongoingTrips: number;
    pendingApprovals: number;
    totalRides: number;
    totalEarnings: number;
};

const emptySummary: TransporterSummary = {
    totalFleet: 0,
    activeDrivers: 0,
    ongoingTrips: 0,
    pendingApprovals: 0,
    totalRides: 0,
    totalEarnings: 0,
};

type FleetRow = {
    vehicleId: string;
    vehicleNumber?: string;
    vehicleName?: string;
    vehicleTypeName?: string;
    driverId?: string;
    driverName?: string;
    driverPhone?: string;
    rideStatus?: string;
    routeSummary?: string;
    vehicleCompletedRides?: number;
    vehicleEarnings?: number;
    driverCompletedRides?: number;
    driverEarnings?: number;
    latitude?: number;
    longitude?: number;
    liveUpdatedAt?: string;
    liveStatus?: string;
};

const TransporterDashboard = () => {
    const [activeTab, setActiveTab] = useState<'overview' | 'drivers' | 'vehicles' | 'requests' | 'reports'>('overview');
    const [currentUser, setCurrentUser] = useState<any>(null);
    const [fleetSummary, setFleetSummary] = useState<TransporterSummary>(emptySummary);
    const [fleetRows, setFleetRows] = useState<FleetRow[]>([]);
    const [withdrawPaymentId, setWithdrawPaymentId] = useState('');
    const [rideIdForDisputes, setRideIdForDisputes] = useState('');
    const [disputes, setDisputes] = useState<any[]>([]);
    const [isVehicleModalOpen, setIsVehicleModalOpen] = useState(false);
    const [isDriverModalOpen, setIsDriverModalOpen] = useState(false);
    const formatCurrency = (value: number = 0) => `₹ ${Number(value).toLocaleString('en-IN')}`;
   // const formatLastSeen = (value?: string) => {
     //   if (!value) return 'No data';
     //   const parsed = new Date(value);
     //   if (isNaN(parsed.getTime())) return 'No data';
     //   return parsed.toLocaleString('en-IN', { hour: '2-digit', minute: '2-digit', day: 'numeric', month: 'short' });
   // };

    const getStatusStyles = (status?: string) => {
        const s = (status || 'Available').toLowerCase();
        if (s.includes('trip') || s.includes('started') || s.includes('arriving') || s.includes('progress')) return 'bg-emerald-50 text-emerald-700 border-emerald-200';
        if (s.includes('transit') || s.includes('assigned')) return 'bg-blue-50 text-blue-700 border-blue-200';
        if (s.includes('maintenance')) return 'bg-orange-50 text-orange-700 border-orange-200';
        if (s.includes('cancel') || s.includes('reject')) return 'bg-red-50 text-red-700 border-red-200';
        return 'bg-slate-100 text-slate-700 border-slate-300';
    };

    const navigate = useNavigate();


    useEffect(() => {
        const userStr = localStorage.getItem('user');
        if (userStr) {
            setCurrentUser(JSON.parse(userStr));
        }
    }, []);

    const fetchDashboardData = useCallback(async () => {
        const userId = currentUser?.userId || currentUser?.UserId;
        if (!userId) return;

        try {
            const [summaryRes, fleetRes] = await Promise.all([
                apiClient.get('/Transport/getDashboardSummary', { params: { userId } }),
                apiClient.get('/Transport/getFleetOverview', { params: { userId } }),
            ]);

            const summaryPayload = summaryRes.data ?? {};
            setFleetSummary({
                totalFleet: summaryPayload.TotalFleet ?? summaryPayload.totalFleet ?? 0,
                activeDrivers: summaryPayload.ActiveDrivers ?? summaryPayload.activeDrivers ?? 0,
                ongoingTrips: summaryPayload.OngoingTrips ?? summaryPayload.ongoingTrips ?? 0,
                pendingApprovals: summaryPayload.PendingApprovals ?? summaryPayload.pendingApprovals ?? 0,
                totalRides: summaryPayload.TotalRides ?? summaryPayload.totalRides ?? 0,
                totalEarnings: summaryPayload.TotalEarnings ?? summaryPayload.totalEarnings ?? 0,
            });

            const rawFleet = Array.isArray(fleetRes.data) ? fleetRes.data : [];
            setFleetRows(
                rawFleet.map((item: any) => ({
                    vehicleId: item.vehicleId ?? item.VehicleId ?? '',
                    vehicleNumber: item.vehicleNumber ?? item.VehicleNumber,
                    vehicleName: item.vehicleName ?? item.VehicleName,
                    vehicleTypeName: item.vehicleTypeName ?? item.VehicleTypeName,
                    driverId: item.driverId ?? item.DriverId,
                    driverName: item.driverName ?? item.DriverName,
                    driverPhone: item.driverPhone ?? item.DriverPhone,
                    rideStatus: item.rideStatus ?? item.RideStatus,
                    routeSummary: item.routeSummary ?? item.RouteSummary,
                    vehicleCompletedRides: item.vehicleCompletedRides ?? item.VehicleCompletedRides ?? 0,
                    vehicleEarnings: item.vehicleEarnings ?? item.VehicleEarnings ?? 0,
                    driverCompletedRides: item.driverCompletedRides ?? item.DriverCompletedRides ?? 0,
                    driverEarnings: item.driverEarnings ?? item.DriverEarnings ?? 0,
                    latitude: item.latitude ?? item.Latitude,
                    longitude: item.longitude ?? item.Longitude,
                    liveUpdatedAt: item.liveUpdatedAt ?? item.LiveUpdatedAt,
                    liveStatus: item.liveStatus ?? item.LiveStatus,
                }))
            );
        } catch (err) {
            console.error('Error fetching transporter dashboard data:', err);
        }
    }, [currentUser]);

    useEffect(() => {
        if (currentUser) {
            fetchDashboardData();
        }
    }, [currentUser, fetchDashboardData]);

    useEffect(() => {
        const userId = currentUser?.userId || currentUser?.UserId;
        if (!userId) return;

        const connection = new HubConnectionBuilder()
            .withUrl(`${apiClient.defaults.baseURL?.replace('/api', '')}/hubs/location`)
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(() => {
                console.log('Connected to fleet location hub');
                connection.invoke('JoinTransporter', userId);
            })
            .catch(err => console.error('SignalR Connection Error: ', err));

        connection.on('fleetLocationUpdated', (tracking: any) => {
            setFleetRows(prevRows => prevRows.map(row => 
                row.vehicleId === tracking.vehicleId || row.vehicleId === tracking.VehicleId
                    ? { ...row, latitude: tracking.latitude || tracking.Latitude, longitude: tracking.longitude || tracking.Longitude, liveUpdatedAt: new Date().toISOString() }
                    : row
            ));
        });

        return () => {
            if (connection) {
                connection.invoke('LeaveTransporter', userId).then(() => connection.stop()).catch(() => connection.stop());
            }
        };
    }, [currentUser]);

    const liveVehiclesReporting = useMemo(
        () => fleetRows.filter((row) => typeof row.latitude === 'number' && typeof row.longitude === 'number').length,
        [fleetRows]
    );

    const assignedDriversCount = useMemo(
        () => fleetRows.filter((row) => row.driverName && row.driverName !== 'Unassigned').length,
        [fleetRows]
    );

    const latestLivePing = useMemo<Date | null>(() => {
        let latest: Date | null = null;
        fleetRows.forEach((row) => {
            if (!row.liveUpdatedAt) return;
            const parsed = new Date(row.liveUpdatedAt);
            if (isNaN(parsed.getTime())) return;
            if (latest === null || parsed > latest) {
                latest = parsed;
            }
        });
        return latest;
    }, [fleetRows]);

    const lastPingLabel = latestLivePing
        ? latestLivePing.toLocaleString('en-IN', { hour: '2-digit', minute: '2-digit', day: 'numeric', month: 'short' })
        : 'No live ping yet';


    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        navigate('/login');
    };


    const processWithdrawal = async (action: 'approve' | 'reject') => {
        if (!withdrawPaymentId.trim()) {
            alert('Enter withdrawal payment id.');
            return;
        }
        try {
            const payload = { paymentId: withdrawPaymentId.trim(), action };
            const res = await apiClient.post('/DriverFinance/withdrawal/process', payload);
            alert(res.data?.message || res.data?.Message || `Withdrawal ${action}d.`);
            setWithdrawPaymentId('');
        } catch (err: any) {
            alert(err?.response?.data?.message || err?.response?.data?.Message || 'Unable to process withdrawal.');
        }
    };

    const fetchRideDisputes = async () => {
        if (!rideIdForDisputes.trim()) {
            alert('Enter ride id.');
            return;
        }
        try {
            const res = await apiClient.get(`/Dispute/ride/${rideIdForDisputes.trim()}`);
            setDisputes(Array.isArray(res.data) ? res.data : []);
        } catch (err) {
            console.error(err);
            setDisputes([]);
            alert('Unable to fetch disputes for this ride.');
        }
    };

    const stats = [
        { label: 'Total Fleet', value: fleetSummary.totalFleet, icon: Truck, color: 'text-blue-600', bg: 'bg-blue-50', border: 'border-blue-100' },
        { label: 'Active Drivers', value: fleetSummary.activeDrivers, icon: Users, color: 'text-indigo-600', bg: 'bg-indigo-50', border: 'border-indigo-100' },
        { label: 'Ongoing Trips', value: fleetSummary.ongoingTrips, icon: CheckCircle2, color: 'text-emerald-600', bg: 'bg-emerald-50', border: 'border-emerald-100' },
        { label: 'Pending Approvals', value: fleetSummary.pendingApprovals, icon: Clock, color: 'text-amber-600', bg: 'bg-amber-50', border: 'border-amber-100' },
    ];

    return (
        <div className="flex h-screen bg-slate-50 overflow-hidden font-sans">
            {/* Sidebar */}
            <aside className="w-72 bg-white border-r border-slate-200 flex flex-col shadow-sm relative z-20">
                <div className="h-20 flex items-center px-8 border-b border-slate-100">
                    <div className="flex items-center gap-3">
                        <div className="w-10 h-10 bg-gradient-to-br from-primary-600 to-indigo-600 rounded-xl flex items-center justify-center shadow-lg shadow-primary-500/30">
                            <Truck className="text-white h-5 w-5" />
                        </div>
                        <span className="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-slate-900 to-slate-700">Navgatix</span>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
                        <div className="premium-card p-6 border border-slate-100 bg-white">
                            <p className="text-xs font-semibold text-slate-500 uppercase tracking-wide mb-2">Total Rides</p>
                            <p className="text-3xl font-black text-slate-900">{fleetSummary.totalRides}</p>
                            <p className="text-sm text-slate-500 mt-1">Completed trips across your fleet</p>
                        </div>
                        <div className="premium-card p-6 border border-slate-100 bg-white">
                            <p className="text-xs font-semibold text-slate-500 uppercase tracking-wide mb-2">Total Earnings</p>
                            <p className="text-3xl font-black text-slate-900">{formatCurrency(fleetSummary.totalEarnings)}</p>
                            <p className="text-sm text-slate-500 mt-1">Verified generation from completed rides</p>
                        </div>
                    </div>

                    <div className="grid gap-6 lg:grid-cols-[2fr,1fr] mb-8">
                        <div className="premium-card p-6 bg-white shadow-lg border border-slate-100">
                            <div className="flex justify-between items-start gap-4 mb-4">
                                <div>
                                    <h3 className="text-lg font-bold text-slate-900">Live Fleet Map</h3>
                                    <p className="text-sm text-slate-500">Track vehicle locations and driver status.</p>
                                </div>
                                <span className="text-xs font-semibold uppercase tracking-wide text-slate-400">{liveVehiclesReporting} reporting</span>
                            </div>
                            <LiveFleetMap vehicles={fleetRows} />
                        </div>
                        <div className="premium-card p-6 bg-white shadow-lg border border-slate-100 space-y-4">
                            <h3 className="text-lg font-bold text-slate-900">Live Signals</h3>
                            <div className="space-y-3 text-sm">
                                <div className="flex justify-between items-center text-slate-500">
                                    <span>Vehicles with location</span>
                                    <span className="font-semibold text-slate-900">{liveVehiclesReporting}</span>
                                </div>
                                <div className="flex justify-between items-center text-slate-500">
                                    <span>Drivers assigned</span>
                                    <span className="font-semibold text-slate-900">{assignedDriversCount}</span>
                                </div>
                                <div className="flex justify-between items-center text-slate-500">
                                    <span>Last ping</span>
                                    <span className="font-semibold text-slate-900">{lastPingLabel}</span>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div className="p-6">
                    <p className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-4 px-2">Menu</p>
                    <nav className="space-y-2">
                        <button
                            onClick={() => setActiveTab('overview')}
                            className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 ${activeTab === 'overview' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}`}
                        >
                            <LayoutDashboard className={`h-5 w-5 ${activeTab === 'overview' ? 'text-primary-600' : ''}`} />
                            Overview
                        </button>
                        <button
                            onClick={() => setActiveTab('drivers')}
                            className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 ${activeTab === 'drivers' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}`}
                        >
                            <Users className={`h-5 w-5 ${activeTab === 'drivers' ? 'text-primary-600' : ''}`} />
                            Manage Drivers
                        </button>
                        <button
                            onClick={() => setActiveTab('vehicles')}
                            className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 ${activeTab === 'vehicles' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}`}
                        >
                            <Truck className={`h-5 w-5 ${activeTab === 'vehicles' ? 'text-primary-600' : ''}`} />
                            Vehicle Fleet
                        </button>
                        <button
                            onClick={() => setActiveTab('requests')}
                            className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 ${activeTab === 'requests' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}`}
                        >
                            <Package className={`h-5 w-5 ${activeTab === 'requests' ? 'text-primary-600' : ''}`} />
                            Ride Requests
                        </button>
                        <button
                            onClick={() => setActiveTab('reports')}
                            className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 ${activeTab === 'reports' ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}`}
                        >
                            <FileText className={`h-5 w-5 ${activeTab === 'reports' ? 'text-primary-600' : ''}`} />
                            Reports
                        </button>
                    </nav>
                </div>

                <div className="mt-auto p-6 border-t border-slate-100">
                    <nav className="space-y-2 mb-6">
                        <button className="w-full flex items-center gap-3 px-4 py-3 rounded-xl text-slate-500 hover:bg-slate-50 hover:text-slate-900 transition-all duration-200">
                            <Settings className="h-5 w-5" />
                            Settings
                        </button>
                    </nav>

                    <div className="bg-slate-50 rounded-2xl p-4 border border-slate-100 flex items-center gap-3">
                        <div className="w-10 h-10 rounded-full bg-indigo-100 flex items-center justify-center text-indigo-700 font-bold border border-indigo-200">
                            SL
                        </div>
                        <div className="flex-1 min-w-0">
                            <p className="text-sm font-bold text-slate-900 truncate">{currentUser?.company || currentUser?.firstName || 'Satguru Logistics'}</p>
                            <p className="text-xs text-slate-500 truncate">{currentUser?.roleName || 'Admin Account'}</p>
                        </div>
                        <button onClick={handleLogout} className="text-slate-400 hover:text-red-500 transition-colors h-8 w-8 flex items-center justify-center rounded-lg hover:bg-red-50">
                            <LogOut className="h-4 w-4" />
                        </button>
                    </div>
                </div>
            </aside>

            {/* Main Content */}
            <main className="flex-1 overflow-y-auto relative z-10">
                {/* Header Backdrop */}
                <div className="absolute top-0 left-0 w-full h-64 bg-slate-900 text-white z-0">
                    <div className="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1586528116311-ad8ed7c159bf?q=80&w=2670&auto=format&fit=crop')] bg-cover bg-center opacity-10 mix-blend-overlay"></div>
                    <div className="absolute inset-0 bg-gradient-to-b from-transparent to-slate-900"></div>
                </div>

                <div className="relative z-10 p-8 max-w-7xl mx-auto">
                    {/* Topbar */}
                    <header className="flex justify-between items-end mb-10 h-24">
                        <div className="text-white">
                            <p className="text-indigo-200 font-medium tracking-wide text-sm mb-1 uppercase">{activeTab}</p>
                            <h1 className="text-3xl font-extrabold tracking-tight">Welcome back, {currentUser?.firstName || 'Satguru Logistics'}</h1>
                        </div>
                        <div className="flex gap-4 items-center">
                            <button className="w-10 h-10 rounded-full bg-white/10 hover:bg-white/20 backdrop-blur-md flex items-center justify-center text-white border border-white/10 transition-colors">
                                <Bell className="h-5 w-5" />
                            </button>
                            <button onClick={() => setIsDriverModalOpen(true)} className="bg-white text-slate-900 hover:bg-slate-50 px-5 py-2.5 rounded-lg border border-white/20 font-semibold shadow-lg shadow-black/10 flex items-center gap-2 transition-colors">
                                <Plus className="h-5 w-5" />
                                Add Driver
                            </button>
                            <button 
                                onClick={() => setIsVehicleModalOpen(true)}
                                className="bg-primary-600 hover:bg-primary-500 text-white px-5 py-2.5 rounded-lg font-semibold shadow-lg shadow-primary-600/30 flex items-center gap-2 transition-colors border border-primary-500"
                            >
                                <Plus className="h-5 w-5" />
                                New Vehicle
                            </button>
                        </div>
                    </header>

                    {/* Stats Grid */}
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
                        {stats.map((stat, i) => (
                            <div key={i} className={`premium-card p-6 flex items-start justify-between border-b-4 ${stat.border}`}>
                                <div>
                                    <p className="text-sm font-semibold text-slate-500 mb-1">{stat.label}</p>
                                    <h3 className="text-3xl font-black text-slate-900 tracking-tight">{stat.value}</h3>
                                </div>
                                <div className={`${stat.bg} ${stat.color} p-3.5 rounded-2xl`}>
                                    <stat.icon className="h-6 w-6" />
                                </div>
                            </div>
                        ))}
                    </div>

                    <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
                        <div className="premium-card p-6">
                            <h3 className="text-lg font-bold text-slate-900 mb-4">Withdrawal Processing</h3>
                            <div className="flex gap-3">
                                <input
                                    value={withdrawPaymentId}
                                    onChange={(e) => setWithdrawPaymentId(e.target.value)}
                                    className="flex-1 rounded-xl border border-slate-300 px-4 py-2.5 text-sm"
                                    placeholder="Withdrawal Payment ID"
                                />
                                <button onClick={() => processWithdrawal('approve')} className="px-4 py-2.5 rounded-xl bg-emerald-600 text-white text-sm font-semibold flex items-center gap-2">
                                    <CheckCircle className="h-4 w-4" /> Approve
                                </button>
                                <button onClick={() => processWithdrawal('reject')} className="px-4 py-2.5 rounded-xl bg-red-600 text-white text-sm font-semibold flex items-center gap-2">
                                    <XCircle className="h-4 w-4" /> Reject
                                </button>
                            </div>
                        </div>

                        <div className="premium-card p-6">
                            <h3 className="text-lg font-bold text-slate-900 mb-4 flex items-center gap-2">
                                <AlertTriangle className="h-5 w-5 text-red-600" /> Ride Disputes
                            </h3>
                            <div className="flex gap-3 mb-4">
                                <input
                                    value={rideIdForDisputes}
                                    onChange={(e) => setRideIdForDisputes(e.target.value)}
                                    className="flex-1 rounded-xl border border-slate-300 px-4 py-2.5 text-sm"
                                    placeholder="Ride ID"
                                />
                                <button onClick={fetchRideDisputes} className="px-4 py-2.5 rounded-xl bg-slate-900 text-white text-sm font-semibold">
                                    Fetch
                                </button>
                            </div>
                            <div className="space-y-2 max-h-40 overflow-auto">
                                {disputes.length === 0 ? (
                                    <p className="text-sm text-slate-500">No disputes loaded.</p>
                                ) : disputes.map((d) => (
                                    <div key={d.id} className="rounded-lg border border-slate-200 px-3 py-2 text-sm">
                                        <p className="font-semibold text-slate-900">#{d.id} • {d.issueType || d.IssueType}</p>
                                        <p className="text-slate-600">{d.description || d.Description}</p>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>

                    {/* Main Section */}
                    {(activeTab === 'overview' || activeTab === 'vehicles') && (
                    <div className="premium-card overflow-hidden">
                        <div className="p-6 border-b border-slate-100 flex sm:flex-row flex-col sm:justify-between items-start sm:items-center gap-4 bg-white">
                            <div>
                                <h2 className="text-lg font-bold text-slate-900">Current Fleet Operations</h2>
                                <p className="text-sm text-slate-500 mt-1">Real-time status of your vehicles and drivers.</p>
                            </div>
                            <div className="relative">
                                <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 h-4 w-4" />
                                <input
                                    type="text"
                                    placeholder="Search vehicles, drivers..."
                                    className="pl-10 pr-4 py-2 bg-slate-50 border border-slate-200 rounded-lg text-sm outline-none focus:border-primary-500 focus:bg-white focus:ring-2 focus:ring-primary-500/20 w-full sm:w-72 transition-all font-medium text-slate-700"
                                />
                            </div>
                        </div>
                        <div className="overflow-x-auto bg-white">
                            <table className="w-full text-left whitespace-nowrap">
                                <thead className="bg-slate-50 border-b border-slate-200 text-slate-500 text-xs font-bold uppercase tracking-wider">
                                    <tr>
                                        <th className="px-6 py-4">Vehicle Details</th>
                                        <th className="px-6 py-4">Assigned Driver</th>
                                        <th className="px-6 py-4">Current Route</th>
                                        <th className="px-6 py-4">Status</th>
                                        <th className="px-6 py-4 text-right">Actions</th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-slate-100">
                                    {fleetRows.length > 0 ? (
                                        fleetRows.map((item, index) => (
                                            <tr key={index} className="hover:bg-slate-50/80 transition-colors group cursor-pointer">
                                                <td className="px-6 py-4">
                                                    <div className="flex items-center gap-3">
                                                        <div className="w-10 h-10 rounded-lg bg-slate-100 flex items-center justify-center border border-slate-200">
                                                            <Truck className="h-5 w-5 text-slate-500" />
                                                        </div>
                                                        <div>
                                                            <div className="font-bold text-slate-900 group-hover:text-primary-600 transition-colors">{item.vehicleNumber || 'Unknown ID'}</div>
                                                            <div className="text-xs font-medium text-slate-500">{item.vehicleName || 'Unknown Model'}</div>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className="px-6 py-4">
                                                    <div className="flex items-center gap-3">
                                                        <div className="w-8 h-8 bg-indigo-100 rounded-full flex items-center justify-center text-xs font-bold text-indigo-700 border border-indigo-200">
                                                            --
                                                        </div>
                                                        <span className="text-sm font-semibold text-slate-700">
                                                            {item.driverName || 'Unassigned'}
                                                        </span>
                                                    </div>
                                                </td>
                                                <td className="px-6 py-4">
                                                    <div className="text-sm font-medium text-slate-600">{item.routeSummary || 'N/A'}</div>
                                                </td>
                                                <td className="px-6 py-4">
                                                    <span className={`px-2.5 py-1 border rounded-full text-xs font-bold ${getStatusStyles(item.rideStatus)}`}>
                                                        {item.rideStatus || 'Available'}
                                                    </span>
                                                </td>
                                                <td className="px-6 py-4 text-right">
                                                    <button className="p-2 text-slate-400 hover:text-primary-600 hover:bg-primary-50 rounded-lg transition-colors">
                                                        <MoreVertical className="h-5 w-5" />
                                                    </button>
                                                </td>
                                            </tr>
                                        ))
                                    ) : (
                                        /* Fallback mock data if API returns empty */
                                        [
                                            { id: 'MH-12-AB-1234', model: 'Tata Prima 40 Tons', driver: 'John Smith', initials: 'JS', route: 'Mumbai → Pune', status: 'On Trip', color: 'text-emerald-700', bg: 'bg-emerald-50', border: 'border-emerald-200' },
                                            { id: 'MH-04-CD-5678', model: 'Ashok Leyland 2518', driver: 'Rahul Kumar', initials: 'RK', route: 'Delhi → Jaipur', status: 'In Transit', color: 'text-blue-700', bg: 'bg-blue-50', border: 'border-blue-200' },
                                            { id: 'MH-14-EF-9012', model: 'BharatBenz 2823', driver: 'Unassigned', initials: '--', route: 'N/A', status: 'Available', color: 'text-slate-700', bg: 'bg-slate-100', border: 'border-slate-300' },
                                        ].map((item, index) => (
                                            <tr key={index} className="hover:bg-slate-50/80 transition-colors group cursor-pointer">
                                                <td className="px-6 py-4">
                                                    <div className="flex items-center gap-3">
                                                        <div className="w-10 h-10 rounded-lg bg-slate-100 flex items-center justify-center border border-slate-200">
                                                            <Truck className="h-5 w-5 text-slate-500" />
                                                        </div>
                                                        <div>
                                                            <div className="font-bold text-slate-900 group-hover:text-primary-600 transition-colors">{item.id}</div>
                                                            <div className="text-xs font-medium text-slate-500">{item.model}</div>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className="px-6 py-4">
                                                    <div className="flex items-center gap-3">
                                                        {item.initials !== '--' ? (
                                                            <div className="w-8 h-8 bg-indigo-100 rounded-full flex items-center justify-center text-xs font-bold text-indigo-700 border border-indigo-200">
                                                                {item.initials}
                                                            </div>
                                                        ) : (
                                                            <div className="w-8 h-8 bg-slate-100 rounded-full flex items-center justify-center border border-slate-200 border-dashed">
                                                                <Users className="h-3.5 w-3.5 text-slate-400" />
                                                            </div>
                                                        )}
                                                        <span className={`text-sm font-semibold ${item.driver === 'Unassigned' ? 'text-slate-400 italic' : 'text-slate-700'}`}>
                                                            {item.driver}
                                                        </span>
                                                    </div>
                                                </td>
                                                <td className="px-6 py-4">
                                                    <div className="text-sm font-medium text-slate-600">{item.route}</div>
                                                </td>
                                                <td className="px-6 py-4">
                                                    <span className={`px-2.5 py-1 ${item.bg} ${item.color} border ${item.border} rounded-full text-xs font-bold`}>
                                                        {item.status}
                                                    </span>
                                                </td>
                                                <td className="px-6 py-4 text-right">
                                                    <button className="p-2 text-slate-400 hover:text-primary-600 hover:bg-primary-50 rounded-lg transition-colors">
                                                        <MoreVertical className="h-5 w-5" />
                                                    </button>
                                                </td>
                                            </tr>
                                        )))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                    )}
                    {activeTab === 'drivers' && (
                        <div className="premium-card overflow-hidden">
                            <div className="p-6 bg-white border-b border-slate-100">
                                <h2 className="text-lg font-bold text-slate-900">Manage Drivers</h2>
                                <p className="text-sm text-slate-500 mt-1">View and manage your onboarded drivers.</p>
                            </div>
                            <div className="p-6 bg-white">
                                <p className="text-slate-500 text-sm">Driver list synced. Use the <strong className="text-slate-700">Add Driver</strong> button in the top right to onboard new drivers to your fleet.</p>
                            </div>
                        </div>
                    )}
                    {activeTab === 'requests' && (
                        <div className="premium-card overflow-hidden">
                            <div className="p-6 bg-white border-b border-slate-100 mb-4">
                                <h2 className="text-lg font-bold text-slate-900">Ride Requests</h2>
                                <p className="text-sm text-slate-500 mt-1">Manage and assign incoming orders matching your fleet radius.</p>
                            </div>
                            <div className="px-6 pb-6">
                                <TransporterRideRequests userId={currentUser?.userId || currentUser?.UserId} fleetRows={fleetRows} />
                            </div>
                        </div>
                    )}
                    {activeTab === 'reports' && (
                        <div className="premium-card overflow-hidden">
                            <div className="p-6 bg-white border-b border-slate-100 mb-4">
                                <h2 className="text-lg font-bold text-slate-900">Analytics & Reports</h2>
                                <p className="text-sm text-slate-500 mt-1">Real-time performance metrics and fleet activity reports.</p>
                            </div>
                            <div className="px-6 pb-6">
                                <TransporterReports userId={currentUser?.userId || currentUser?.UserId} />
                            </div>
                        </div>
                    )}
                </div>
            </main>

            <DriverModal isOpen={isDriverModalOpen} onClose={() => setIsDriverModalOpen(false)} onSuccess={fetchDashboardData} transporterId={currentUser?.userId || currentUser?.id || ''} transporterUserId={currentUser?.userId || currentUser?.id || ''} />
            <VehicleModal 
                isOpen={isVehicleModalOpen} 
                onClose={() => setIsVehicleModalOpen(false)} 
                onSuccess={fetchDashboardData}
                userId={currentUser?.userId || currentUser?.id || ''}
            />
        </div>

    );
};

export default TransporterDashboard;

