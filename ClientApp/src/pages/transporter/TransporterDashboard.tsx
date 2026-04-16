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
    const [isVehicleModalOpen, setIsVehicleModalOpen] = useState(false);
    const [isDriverModalOpen, setIsDriverModalOpen] = useState(false);
    const [drivers, setDrivers] = useState<any[]>([]);
    const [vehicles, setVehicles] = useState<any[]>([]);
    const [isLoadingFleet, setIsLoadingFleet] = useState(false);
    
    const navigate = useNavigate();
    const formatCurrency = (value: number = 0) => `₹ ${Number(value).toLocaleString('en-IN')}`;

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

    const fetchFleetLists = useCallback(async () => {
        const userId = currentUser?.userId || currentUser?.UserId;
        if (!userId) return;

        setIsLoadingFleet(true);
        try {
            const [driversRes, vehiclesRes] = await Promise.all([
                apiClient.get('/Transport/getDriversList', { params: { userId } }),
                apiClient.get('/Transport/getVehiclesList', { params: { userId } }),
            ]);
            setDrivers(Array.isArray(driversRes.data) ? driversRes.data : []);
            setVehicles(Array.isArray(vehiclesRes.data) ? vehiclesRes.data : []);
        } catch (err) {
            console.error('Error fetching fleet lists:', err);
        } finally {
            setIsLoadingFleet(false);
        }
    }, [currentUser]);

    useEffect(() => {
        if (currentUser) {
            fetchDashboardData();
            fetchFleetLists();
        }
    }, [currentUser, fetchDashboardData, fetchFleetLists]);

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

    const stats = [
        { label: 'Total Fleet', value: fleetSummary.totalFleet, icon: Truck, color: 'text-blue-600', bg: 'bg-blue-50', border: 'border-blue-100' },
        { label: 'Active Drivers', value: fleetSummary.activeDrivers, icon: Users, color: 'text-indigo-600', bg: 'bg-indigo-50', border: 'border-indigo-100' },
        { label: 'Ongoing Trips', value: fleetSummary.ongoingTrips, icon: CheckCircle2, color: 'text-emerald-600', bg: 'bg-emerald-50', border: 'border-emerald-100' },
        { label: 'Pending Approvals', value: fleetSummary.pendingApprovals, icon: Clock, color: 'text-amber-600', bg: 'bg-amber-50', border: 'border-amber-100' },
    ];

    const getStatusStyles = (status?: string) => {
        const s = (status || 'Available').toLowerCase();
        if (s.includes('trip') || s.includes('started') || s.includes('arriving') || s.includes('progress')) return 'bg-emerald-50 text-emerald-700 border-emerald-200';
        if (s.includes('transit') || s.includes('assigned')) return 'bg-blue-50 text-blue-700 border-blue-200';
        if (s.includes('maintenance')) return 'bg-orange-50 text-orange-700 border-orange-200';
        if (s.includes('cancel') || s.includes('reject')) return 'bg-red-50 text-red-700 border-red-200';
        return 'bg-slate-100 text-slate-700 border-slate-300';
    };

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
                </div>

                <div className="p-6">
                    <p className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-4 px-2">Menu</p>
                    <nav className="space-y-2">
                        {[
                            { id: 'overview', label: 'Overview', icon: LayoutDashboard },
                            { id: 'drivers', label: 'Manage Drivers', icon: Users },
                            { id: 'vehicles', label: 'Vehicle Fleet', icon: Truck },
                            { id: 'requests', label: 'Ride Requests', icon: Package },
                            { id: 'reports', label: 'Reports', icon: FileText },
                        ].map((item) => (
                            <button
                                key={item.id}
                                onClick={() => setActiveTab(item.id as any)}
                                className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 ${activeTab === item.id ? 'bg-primary-50 text-primary-700 font-semibold shadow-sm' : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}`}
                            >
                                <item.icon className={`h-5 w-5 ${activeTab === item.id ? 'text-primary-600' : ''}`} />
                                {item.label}
                            </button>
                        ))}
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
                <div className="absolute top-0 left-0 w-full h-64 bg-slate-900 text-white z-0">
                    <div className="absolute inset-0 bg-[url('https://images.unsplash.com/photo-1586528116311-ad8ed7c159bf?q=80&w=2670&auto=format&fit=crop')] bg-cover bg-center opacity-10 mix-blend-overlay"></div>
                    <div className="absolute inset-0 bg-gradient-to-b from-transparent to-slate-900"></div>
                </div>

                <div className="relative z-10 p-8 max-w-7xl mx-auto">
                    <header className="flex justify-between items-end mb-10 h-24">
                        <div className="text-white">
                            <p className="text-indigo-200 font-medium tracking-wide text-sm mb-1 uppercase">{activeTab}</p>
                            <h1 className="text-3xl font-extrabold tracking-tight">Welcome back, {currentUser?.firstName || 'Satguru Logistics'}</h1>
                        </div>
                        <div className="flex gap-4 items-center">
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
                            <div key={i} className={`premium-card p-6 flex items-start justify-between border-b-4 ${stat.border} bg-white rounded-2xl shadow-sm`}>
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

                    {activeTab === 'overview' && (
                        <>
                            {/* Map & Live Signals */}
                            <div className="grid gap-6 lg:grid-cols-[2fr,1fr] mb-8">
                                <div className="premium-card p-6 bg-white shadow-sm border border-slate-100 rounded-2xl">
                                    <div className="flex justify-between items-start gap-4 mb-4">
                                        <div>
                                            <h3 className="text-lg font-bold text-slate-900">Live Fleet Map</h3>
                                            <p className="text-sm text-slate-500">Real-time coordinates from active vehicles.</p>
                                        </div>
                                        <span className="text-xs font-bold bg-slate-100 text-slate-600 px-3 py-1 rounded-full uppercase tracking-wider">{liveVehiclesReporting} Live</span>
                                    </div>
                                    <div className="h-[400px] rounded-xl overflow-hidden border border-slate-200">
                                        <LiveFleetMap vehicles={fleetRows} />
                                    </div>
                                </div>

                                <div className="space-y-6">
                                    <div className="premium-card p-6 bg-white shadow-sm border border-slate-100 rounded-2xl">
                                        <h3 className="text-lg font-bold text-slate-900 mb-4 flex items-center gap-2">
                                            <Clock className="h-5 w-5 text-indigo-600" />
                                            Signals
                                        </h3>
                                        <div className="space-y-4">
                                            <div className="flex justify-between items-center p-3 bg-slate-50 rounded-xl">
                                                <span className="text-sm text-slate-500">Drivers Assigned</span>
                                                <span className="font-bold text-slate-900">{assignedDriversCount}</span>
                                            </div>
                                            <div className="flex justify-between items-center p-3 bg-emerald-50 rounded-xl text-emerald-700">
                                                <span className="text-sm">Last Ping</span>
                                                <span className="font-bold">{lastPingLabel}</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="premium-card p-6 bg-white shadow-sm border border-slate-100 rounded-2xl">
                                        <h3 className="text-lg font-bold text-slate-900 mb-4">Financial Summary</h3>
                                        <div className="space-y-3">
                                            <div>
                                                <p className="text-xs text-slate-400 font-bold uppercase mb-1">Total Earnings</p>
                                                <p className="text-2xl font-black text-slate-900">{formatCurrency(fleetSummary.totalEarnings)}</p>
                                            </div>
                                            <div>
                                                <p className="text-xs text-slate-400 font-bold uppercase mb-1">Resolved Rides</p>
                                                <p className="text-2xl font-black text-slate-900">{fleetSummary.totalRides}</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            {/* Operations Table */}
                            <div className="premium-card overflow-hidden bg-white rounded-2xl shadow-sm border border-slate-100">
                                <div className="p-6 border-b border-slate-100 flex justify-between items-center">
                                    <h2 className="text-lg font-bold text-slate-900">Current Fleet Operations</h2>
                                    <div className="relative">
                                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 h-4 w-4" />
                                        <input type="text" placeholder="Filter fleet..." className="pl-10 pr-4 py-2 bg-slate-50 border border-slate-200 rounded-lg text-sm w-64" />
                                    </div>
                                </div>
                                <div className="overflow-x-auto">
                                    <table className="w-full text-left">
                                        <thead className="bg-slate-50 text-slate-500 text-xs font-bold uppercase">
                                            <tr>
                                                <th className="px-6 py-4">Vehicle</th>
                                                <th className="px-6 py-4">Driver</th>
                                                <th className="px-6 py-4">Route</th>
                                                <th className="px-6 py-4">Status</th>
                                                <th className="px-6 py-4 text-right">Action</th>
                                            </tr>
                                        </thead>
                                        <tbody className="divide-y divide-slate-100">
                                            {fleetRows.length > 0 ? fleetRows.map((row, i) => (
                                                <tr key={i} className="hover:bg-slate-50 transition-colors">
                                                    <td className="px-6 py-4 font-bold text-slate-900">{row.vehicleNumber}</td>
                                                    <td className="px-6 py-4 text-slate-600">{row.driverName || 'Unassigned'}</td>
                                                    <td className="px-6 py-4 text-slate-500 text-sm">{row.routeSummary || 'Waiting...'}</td>
                                                    <td className="px-6 py-4">
                                                        <span className={`px-2 py-1 rounded-full text-[10px] font-black uppercase border ${getStatusStyles(row.rideStatus)}`}>
                                                            {row.rideStatus || 'Available'}
                                                        </span>
                                                    </td>
                                                    <td className="px-6 py-4 text-right">
                                                        <button className="text-slate-400 hover:text-primary-600"><MoreVertical className="h-5 w-5" /></button>
                                                    </td>
                                                </tr>
                                            )) : (
                                                <tr><td colSpan={5} className="px-6 py-10 text-center text-slate-400 italic">No vehicles in fleet</td></tr>
                                            )}
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </>
                    )}

                    {activeTab === 'drivers' && (
                        <div className="premium-card overflow-hidden bg-white rounded-2xl shadow-sm border border-slate-100">
                            <div className="p-6 border-b border-slate-100 flex justify-between items-center">
                                <div>
                                    <h2 className="text-xl font-bold text-slate-900">Manage Drivers</h2>
                                    <p className="text-sm text-slate-500">Onboard and monitor your dedicated driver team.</p>
                                </div>
                                <button onClick={() => setIsDriverModalOpen(true)} className="bg-primary-600 hover:bg-primary-500 text-white px-4 py-2 rounded-lg text-sm font-semibold transition-colors flex items-center gap-2">
                                    <Plus className="h-4 w-4" /> Add Driver
                                </button>
                            </div>
                            <div className="overflow-x-auto text-sm">
                                <table className="w-full text-left">
                                    <thead className="bg-slate-50 text-slate-500 font-bold uppercase text-[10px] tracking-wider">
                                        <tr>
                                            <th className="px-6 py-4">Driver Name</th>
                                            <th className="px-6 py-4">Phone</th>
                                            <th className="px-6 py-4">License No.</th>
                                            <th className="px-6 py-4">Status</th>
                                            <th className="px-6 py-4 text-right">Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody className="divide-y divide-slate-100 text-slate-600 font-medium">
                                        {drivers.length > 0 ? drivers.map((d) => (
                                            <tr key={d.id} className="hover:bg-slate-50 transition-colors">
                                                <td className="px-6 py-4 flex items-center gap-3">
                                                    <div className="w-8 h-8 rounded-full bg-slate-100 flex items-center justify-center overflow-hidden">
                                                        {d.profilePic ? <img src={d.profilePic} alt="" className="w-full h-full object-cover" /> : <Users className="h-4 w-4 text-slate-400" />}
                                                    </div>
                                                    <span className="text-slate-900 font-bold">{d.name}</span>
                                                </td>
                                                <td className="px-6 py-4">{d.phone || d.mobile || 'N/A'}</td>
                                                <td className="px-6 py-4">{d.licenseNumber || 'None'}</td>
                                                <td className="px-6 py-4">
                                                    <span className={`px-2 py-0.5 rounded-full text-[10px] uppercase font-black border ${d.profileStatus === 'Approved' ? 'bg-emerald-50 text-emerald-700 border-emerald-200' : 'bg-amber-50 text-amber-700 border-amber-200'}`}>
                                                        {d.profileStatus || 'Pending'}
                                                    </span>
                                                </td>
                                                <td className="px-6 py-4 text-right">
                                                    <button className="text-slate-400 hover:text-primary-600 transition-colors"><MoreVertical className="h-5 w-5" /></button>
                                                </td>
                                            </tr>
                                        )) : (
                                            <tr>
                                                <td colSpan={5} className="px-6 py-12 text-center text-slate-400 italic">
                                                    {isLoadingFleet ? 'Loading drivers...' : 'No drivers onboarded yet.'}
                                                </td>
                                            </tr>
                                        )}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    )}

                    {activeTab === 'vehicles' && (
                        <div className="premium-card overflow-hidden bg-white rounded-2xl shadow-sm border border-slate-100">
                            <div className="p-6 border-b border-slate-100 flex justify-between items-center">
                                <div>
                                    <h2 className="text-xl font-bold text-slate-900">Vehicle Fleet</h2>
                                    <p className="text-sm text-slate-500">Track and maintain your logistics assets.</p>
                                </div>
                                <button onClick={() => setIsVehicleModalOpen(true)} className="bg-primary-600 hover:bg-primary-500 text-white px-4 py-2 rounded-lg text-sm font-semibold transition-colors flex items-center gap-2">
                                    <Plus className="h-4 w-4" /> New Vehicle
                                </button>
                            </div>
                            <div className="overflow-x-auto text-sm">
                                <table className="w-full text-left">
                                    <thead className="bg-slate-50 text-slate-500 font-bold uppercase text-[10px] tracking-wider">
                                        <tr>
                                            <th className="px-6 py-4">Vehicle No.</th>
                                            <th className="px-6 py-4">Model/Name</th>
                                            <th className="px-6 py-4">Type</th>
                                            <th className="px-6 py-4">Capacity</th>
                                            <th className="px-6 py-4">Availability</th>
                                            <th className="px-6 py-4 text-right">Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody className="divide-y divide-slate-100 text-slate-600 font-medium">
                                        {vehicles.length > 0 ? vehicles.map((v) => (
                                            <tr key={v.id} className="hover:bg-slate-50 transition-colors">
                                                <td className="px-6 py-4 font-bold text-slate-900">{v.vehicleNumber}</td>
                                                <td className="px-6 py-4">{v.vehicleName}</td>
                                                <td className="px-6 py-4">{v.vehicleTypeName}</td>
                                                <td className="px-6 py-4">{v.capacityTons} Tons</td>
                                                <td className="px-6 py-4">
                                                    <span className={`px-2 py-0.5 rounded-full text-[10px] uppercase font-black border ${v.isAvailable ? 'bg-emerald-50 text-emerald-700 border-emerald-200' : 'bg-slate-100 text-slate-600 border-slate-200'}`}>
                                                        {v.isAvailable ? 'Available' : 'Busy'}
                                                    </span>
                                                </td>
                                                <td className="px-6 py-4 text-right">
                                                    <button className="text-slate-400 hover:text-primary-600 transition-colors"><MoreVertical className="h-5 w-5" /></button>
                                                </td>
                                            </tr>
                                        )) : (
                                            <tr>
                                                <td colSpan={6} className="px-6 py-12 text-center text-slate-400 italic">
                                                    {isLoadingFleet ? 'Loading vehicles...' : 'No vehicles in fleet.'}
                                                </td>
                                            </tr>
                                        )}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    )}

                    {activeTab === 'requests' && (
                        <div className="premium-card p-6 bg-white rounded-2xl shadow-sm border border-slate-100">
                            <h2 className="text-lg font-bold text-slate-900 mb-6">Nearby Ride Requests</h2>
                            <TransporterRideRequests userId={currentUser?.userId || currentUser?.UserId} fleetRows={fleetRows} onAssignmentSuccess={() => { fetchDashboardData(); fetchFleetLists(); }} />
                        </div>
                    )}

                    {activeTab === 'reports' && (
                        <div className="premium-card p-6 bg-white rounded-2xl shadow-sm border border-slate-100">
                            <h2 className="text-lg font-bold text-slate-900 mb-6">Analytics & Activity Reports</h2>
                            <TransporterReports userId={currentUser?.userId || currentUser?.UserId} />
                        </div>
                    )}
                </div>
            </main>

            <DriverModal isOpen={isDriverModalOpen} onClose={() => setIsDriverModalOpen(false)} onSuccess={() => { fetchDashboardData(); fetchFleetLists(); }} transporterId={currentUser?.userId || currentUser?.id || ''} transporterUserId={currentUser?.userId || currentUser?.id || ''} />
            <VehicleModal isOpen={isVehicleModalOpen} onClose={() => setIsVehicleModalOpen(false)} onSuccess={() => { fetchDashboardData(); fetchFleetLists(); }} userId={currentUser?.userId || currentUser?.id || ''} />
        </div>
    );
};

export default TransporterDashboard;
