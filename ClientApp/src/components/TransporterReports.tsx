import { useState, useEffect } from 'react';
import apiClient from '../api/apiClient';
import { 
    Loader2, 
    TrendingUp, 
    TrendingDown, 
    Zap, 
    Fuel, 
    Navigation, 
    Calendar,
    BarChart3,
    ArrowUpRight,
    Trophy
} from 'lucide-react';

interface AnalyticsData {
    dailyTrips: number;
    totalTrips: number;
    totalDistanceKm: number;
    dailyDistanceKm: number;
    estimatedFuelLiters: number;
    dailyEarnings: number;
    totalEarnings: number;
    performanceScore: number;
}

const TransporterReports = ({ userId }: { userId: string }) => {
    const [data, setData] = useState<AnalyticsData | null>(null);
    const [loading, setLoading] = useState(true);

    const fetchAnalytics = async () => {
        setLoading(true);
        try {
            const res = await apiClient.get(`/Transport/getTransporterAnalytics?userId=${userId}`);
            setData(res.data);
        } catch (err) {
            console.error('Failed to fetch analytics', err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (userId) fetchAnalytics();
    }, [userId]);

    if (loading) {
        return (
            <div className="p-12 flex flex-col items-center justify-center text-slate-500">
                <Loader2 className="animate-spin h-8 w-8 mb-4 text-primary-500" />
                <p className="font-medium">Aggregating fleet performance data...</p>
            </div>
        );
    }

    if (!data) return null;

    const stats = [
        { 
            label: 'Daily Distance', 
            value: `${data.dailyDistanceKm} km`, 
            icon: Navigation, 
            color: 'bg-blue-50 text-blue-600',
            sub: 'Distance covered today'
        },
        { 
            label: 'Total Distance', 
            value: `${data.totalDistanceKm} km`, 
            icon: BarChart3, 
            color: 'bg-indigo-50 text-indigo-600',
            sub: 'Lifetime fleet distance'
        },
        { 
            label: 'Fuel Estimation', 
            value: `${data.estimatedFuelLiters} L`, 
            icon: Fuel, 
            color: 'bg-orange-50 text-orange-600',
            sub: 'Approx. consumption'
        },
        { 
            label: 'Fleet Efficiency', 
            value: `${data.performanceScore}%`, 
            icon: Trophy, 
            color: 'bg-emerald-50 text-emerald-600',
            sub: 'Trip completion rate'
        }
    ];

    return (
        <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
            {/* Grid Stats */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                {stats.map((s, idx) => (
                    <div key={idx} className="bg-white p-5 rounded-3xl border border-slate-100 shadow-sm hover:shadow-md transition-all group">
                        <div className="flex items-center justify-between mb-3">
                            <div className={`p-2.5 ${s.color} rounded-2xl group-hover:scale-110 transition-transform`}>
                                <s.icon className="h-5 w-5" />
                            </div>
                            <ArrowUpRight className="h-4 w-4 text-slate-300 group-hover:text-primary-500 transition-colors" />
                        </div>
                        <h4 className="text-slate-500 text-sm font-medium">{s.label}</h4>
                        <div className="text-2xl font-bold text-slate-900 mt-1">{s.value}</div>
                        <p className="text-xs text-slate-400 mt-1">{s.sub}</p>
                    </div>
                ))}
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* Trip Volume Card */}
                <div className="lg:col-span-2 bg-white p-6 rounded-3xl border border-slate-100 shadow-sm">
                    <div className="flex items-center justify-between mb-6">
                        <div>
                            <h3 className="text-lg font-bold text-slate-900">Trip Volume Analysis</h3>
                            <p className="text-sm text-slate-500">Comparison of daily vs historical activity</p>
                        </div>
                        <Calendar className="h-5 w-5 text-slate-400" />
                    </div>
                    
                    <div className="space-y-6">
                        <div>
                            <div className="flex justify-between text-sm mb-2">
                                <span className="font-medium text-slate-600">Daily Trips ({data.dailyTrips})</span>
                                <span className="text-slate-400">{Math.round((data.dailyTrips / (data.totalTrips || 1)) * 100)}% of total</span>
                            </div>
                            <div className="h-3 bg-slate-50 rounded-full overflow-hidden border border-slate-100">
                                <div 
                                    className="h-full bg-primary-500 rounded-full transition-all duration-1000" 
                                    style={{ width: `${Math.min(100, (data.dailyTrips / (data.totalTrips || 1)) * 1000)}%` }} // Scaled for visibility if small
                                ></div>
                            </div>
                        </div>

                        <div className="grid grid-cols-2 gap-4">
                            <div className="p-4 bg-emerald-50/50 rounded-2xl border border-emerald-100">
                                <p className="text-xs font-bold text-emerald-600 uppercase mb-1">Today's Earnings</p>
                                <p className="text-xl font-bold text-slate-900">₹ {data.dailyEarnings.toLocaleString('en-IN')}</p>
                            </div>
                            <div className="p-4 bg-primary-50/50 rounded-2xl border border-primary-100">
                                <p className="text-xs font-bold text-primary-600 uppercase mb-1">Total Trips</p>
                                <p className="text-xl font-bold text-slate-900">{data.totalTrips}</p>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Performance Gauge */}
                <div className="bg-gradient-to-br from-slate-900 to-slate-800 p-6 rounded-3xl shadow-xl text-white relative overflow-hidden">
                    <div className="absolute top-0 right-0 w-32 h-32 bg-primary-500/10 rounded-full blur-3xl -mr-16 -mt-16"></div>
                    <div className="relative z-10">
                        <Zap className="h-8 w-8 text-yellow-400 mb-4" />
                        <h3 className="text-xl font-bold mb-1">Efficiency Score</h3>
                        <p className="text-slate-400 text-sm mb-6">Based on trip completions vs cancellations</p>
                        
                        <div className="flex items-center justify-center py-4">
                            <div className="relative">
                                <svg className="w-32 h-32 transform -rotate-90">
                                    <circle
                                        cx="64"
                                        cy="64"
                                        r="58"
                                        stroke="currentColor"
                                        strokeWidth="8"
                                        fill="transparent"
                                        className="text-slate-700"
                                    />
                                    <circle
                                        cx="64"
                                        cy="64"
                                        r="58"
                                        stroke="currentColor"
                                        strokeWidth="8"
                                        fill="transparent"
                                        strokeDasharray={364.4}
                                        strokeDashoffset={364.4 - (364.4 * data.performanceScore) / 100}
                                        className="text-primary-500 transition-all duration-1000"
                                        strokeLinecap="round"
                                    />
                                </svg>
                                <div className="absolute inset-0 flex items-center justify-center flex-col">
                                    <span className="text-3xl font-bold">{data.performanceScore}</span>
                                    <span className="text-[10px] uppercase font-bold text-slate-400">Score</span>
                                </div>
                            </div>
                        </div>

                        <div className="mt-6 flex items-center gap-2 text-sm font-medium">
                            {data.performanceScore >= 90 ? (
                                <>
                                    <TrendingUp className="h-4 w-4 text-emerald-400" />
                                    <span className="text-emerald-400">Excellent Fleet Health</span>
                                </>
                            ) : (
                                <>
                                    <TrendingDown className="h-4 w-4 text-orange-400" />
                                    <span className="text-orange-400">Needs Optimization</span>
                                </>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default TransporterReports;
