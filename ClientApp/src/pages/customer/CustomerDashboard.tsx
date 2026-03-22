import { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { Anchor, ArrowRightLeft, CheckCircle, ClipboardList, Database, History, Info, Layout, MapPin, Navigation, Package, Search, Truck, User, X } from 'lucide-react';
import apiClient from '../../api/apiClient';
import { fetchVehicleCommonTypes } from '../../services/vehicleCommonTypes';
import { normalizeCommonTypes, type NormalizedCommonType } from '../../lib/commonTypes';
import { MapContainer, TileLayer, Marker } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';
import TrackingMap from '../../components/TrackingMap';

// Fix for default marker icon in react-leaflet
delete (L.Icon.Default.prototype as any)._getIconUrl;
L.Icon.Default.mergeOptions({
    iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon-2x.png',
    iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon.png',
    shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
});

type RideStatus =
    | 'request_for_ride'
    | 'driver_assigned'
    | 'driver_arriving'
    | 'ride_started'
    | 'ride_completed'
    | 'cancelled';

interface Shipment {
    id: number;
    productType: string;
    pickup: string;
    destination: string;
    weight: number;
    vehicle: string;
    matchedCount: number;
    status: RideStatus;
    date: string;
}

const STATUS_OPTIONS: RideStatus[] = [
    'request_for_ride',
    'driver_assigned',
    'driver_arriving',
    'ride_started',
    'ride_completed',
    'cancelled',
];

const DEFAULT_PRODUCT_TYPES = [
    'Electronics',
    'Furniture',
    'Groceries',
    'Clothing',
    'Books',
    'Machinery',
    'Raw Materials',
    'Perishables',
    'Pharmaceuticals',
    'Other',
];

const FALLBACK_PRODUCT_TYPES: NormalizedCommonType[] = DEFAULT_PRODUCT_TYPES.map((name, index) => ({
    id: -(index + 1),
    name,
}));

const CustomerDashboard = () => {
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams();
    const [user, setUser] = useState<any>(null);
    const [activeTab, setActiveTab] = useState<'shipments' | 'new'>('shipments');
    const [shipments, setShipments] = useState<Shipment[]>([]);
    
    // Dynamic common types
    const [vehicleTypes, setVehicleTypes] = useState<NormalizedCommonType[]>([]);
    const [bodyTypes, setBodyTypes] = useState<NormalizedCommonType[]>([]);
    const [tyreTypes, setTyreTypes] = useState<NormalizedCommonType[]>([]);
    const [productTypes, setProductTypes] = useState<NormalizedCommonType[]>([]);

    const [formData, setFormData] = useState({
        productType: '',
        customProductType: '',
        pickup: '',
        destination: '',
        pickupLat: '',
        pickupLng: '',
        dropLat: '',
        dropLng: '',
        weight: '',
        vehicle: '', 
        ctBodyType: '',
        ctTyreType: '',
    });
    const [submitStatus, setSubmitStatus] = useState<'idle' | 'loading' | 'success' | 'error'>('idle');
    const [errorMessage, setErrorMessage] = useState('');
    const [statusDrafts, setStatusDrafts] = useState<Record<number, RideStatus>>({});
    const [paymentDrafts, setPaymentDrafts] = useState<Record<number, string>>({});
    const [disputeDrafts, setDisputeDrafts] = useState<Record<number, string>>({});
    const [trackingBooking, setTrackingBooking] = useState<any>(null);

    useEffect(() => {
        const userStr = localStorage.getItem('user');
        if (userStr) {
            setUser(JSON.parse(userStr));
        } else {
            navigate('/login');
        }
    }, [navigate]);

    useEffect(() => {
        const loadInitialData = async () => {
            const customerUserId = user?.userId || user?.UserId || user?.id || '';
            
            // Load common types from backend helper
            try {
                const [masterData, prodRes] = await Promise.all([
                    fetchVehicleCommonTypes(),
                    apiClient.get('/CommonType/getcommontypeWithKeys/PRODTYP'),
                ]);
                setVehicleTypes(masterData.vehicleTypes);
                setBodyTypes(masterData.bodyTypes);
                setTyreTypes(masterData.tyreTypes);
                setProductTypes(normalizeCommonTypes(prodRes.data || []));
            } catch (err) {
                console.error('Failed to load vehicle common types', err);
            }

            if (!customerUserId) {
                return;
            }

            try {
                const res = await apiClient.post('/Vehicle/bookingVehiclerides', null, {
                    params: { userId: customerUserId },
                });
                const data = Array.isArray(res.data) ? res.data : [];
                const normalized = data
                    .filter((item) => item?.Id || item?.id)
                    .map((item) => ({
                        id: Number(item.id ?? item.Id),
                        productType: item.goodsType ?? item.GoodsType ?? 'Goods',
                        pickup: item.pickupAddress ?? item.PickupAddress ?? '',
                        destination: item.dropAddress ?? item.DropAddress ?? '',
                        weight: Number(item.goodsWeight ?? item.GoodsWeight ?? 0),
                        vehicle: item.vehicleNumber ?? item.VehicleNumber ?? 'Assigned vehicle',
                        matchedCount: 0,
                        status: (item.rideStatus ?? item.RideStatus ?? 'request_for_ride') as RideStatus,
                        date: item.createdAt ?? item.CreatedAt ?? '',
                    }));

                setShipments(normalized);
                setStatusDrafts(
                    normalized.reduce<Record<number, RideStatus>>((acc, shipment) => {
                        acc[shipment.id] = shipment.status;
                        return acc;
                    }, {})
                );
            } catch (err) {
                console.error(err);
            }
        };

        loadInitialData();
    }, [user]);

    useEffect(() => {
        const tab = searchParams.get('tab');
        setActiveTab(tab === 'new' ? 'new' : 'shipments');
    }, [searchParams]);

    const openNewShipment = () => {
        setActiveTab('new');
        setSearchParams({ tab: 'new' });
    };

    const openShipmentList = () => {
        setActiveTab('shipments');
        setSearchParams({});
    };

    const toCommonTypeId = (value: string) => {
        const parsed = Number(value);
        return Number.isFinite(parsed) ? parsed : null;
    };

    const parseNum = (value: string) => {
        const num = Number(value);
        return Number.isFinite(num) ? num : 0;
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        setFormData((prev) => ({ ...prev, [e.target.name]: e.target.value }));
    };

    const handleCreateShipment = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMessage('');

    if (!formData.vehicle) {
        setErrorMessage('Vehicle type selection is mandatory for creating a shipment.');
        setSubmitStatus('error');
        return;
    }

    setSubmitStatus('loading');

            try {
            // Use custom product type if "Other" is selected, otherwise use the selected product type
            const productListForSubmit = productTypes.length ? productTypes : FALLBACK_PRODUCT_TYPES;
            const selectedProduct = productListForSubmit.find((item) => String(item.id) === formData.productType);
            const finalProductType =
                formData.productType === 'Other'
                    ? formData.customProductType
                    : selectedProduct?.name ?? formData.productType;

            const payload = {
                customerId: user?.userId || user?.id || user?.UserId || '',
                customerName: user?.firstName || user?.name || user?.company || 'Customer',
                pickupAddress: formData.pickup,
                dropAddress: formData.destination,
                pickupLat: parseNum(formData.pickupLat),
                pickupLng: parseNum(formData.pickupLng),
                dropLat: parseNum(formData.dropLat),
                dropLng: parseNum(formData.dropLng),
                goodsWeight: parseNum(formData.weight),
                goodsType: finalProductType,
                estimatedFare: 0,
                scheduledTime: new Date().toISOString(),
                CT_VehicleType: toCommonTypeId(formData.vehicle),
                CTBodyType: toCommonTypeId(formData.ctBodyType),
                CTTyreType: toCommonTypeId(formData.ctTyreType),
                radiusKm: 50,
            };

            const res = await apiClient.post('/Vehicle/matchDriversAndRequestRide', payload);
            const data = res.data || {};
            const bookingId = Number(data.bookingId ?? data.BookingId ?? 0);
            const matchedCount = Number(data.matchedCount ?? data.MatchedCount ?? 0);

            if (!bookingId) {
                throw new Error(data.message || data.Message || 'Ride request creation failed.');
            }

                const selectedVehicleType = vehicleTypes.find((v) => v.id === Number(formData.vehicle));
                const vehicleName = selectedVehicleType ? selectedVehicleType.name : 'Unknown Vehicle';

            const newShipment: Shipment = {
                id: bookingId,
                productType: finalProductType,
                pickup: formData.pickup,
                destination: formData.destination,
                weight: parseNum(formData.weight),
                vehicle: vehicleName,
                matchedCount,
                status: 'request_for_ride',
                date: new Date().toLocaleDateString(),
            };

            setShipments((prev) => [newShipment, ...prev]);
            setStatusDrafts((prev) => ({ ...prev, [bookingId]: 'request_for_ride' }));
            setSubmitStatus('success');
            setFormData({
                productType: '',
                customProductType: '',
                pickup: '',
                destination: '',
                pickupLat: '',
                pickupLng: '',
                dropLat: '',
                dropLng: '',
                weight: '',
                vehicle: '',
                ctBodyType: '',
                ctTyreType: '',
            });
            setTimeout(() => {
                setSubmitStatus('idle');
                openShipmentList();
            }, 3000);
        } catch (err: any) {
            console.error(err);
            setErrorMessage(err?.response?.data?.message || err?.response?.data?.Message || err?.message || 'Failed to submit shipment.');
            setSubmitStatus('error');
        }
    };

    const updateRideStatus = async (rideId: number) => {
        const status = statusDrafts[rideId];
        if (!status) return;

        try {
            const res = await apiClient.patch(`/Vehicle/${rideId}/rideStatus`, null, { params: { status } });
            const data = res.data || {};
            const next = (data.rideStatus || data.RideStatus || status) as RideStatus;
            setShipments((prev) => prev.map((shipment) => (shipment.id === rideId ? { ...shipment, status: next } : shipment)));
        } catch (err: any) {
            alert(err?.response?.data?.message || err?.response?.data?.Message || 'Unable to update ride status.');
        }
    };

    const markRidePayment = async (rideId: number) => {
        const amount = Number(paymentDrafts[rideId] || 0);
        if (!amount || amount <= 0) {
            alert('Enter a valid payment amount.');
            return;
        }

        try {
            const payload = {
                rideId,
                amount,
                paymentMode: 'ride_payment',
                transactionReference: `CUSTOMER_PAY_${rideId}_${Date.now()}`,
            };
            const res = await apiClient.post('/DriverFinance/ridePayment', payload);
            alert(res.data?.message || res.data?.Message || 'Ride payment recorded.');
            setPaymentDrafts((prev) => ({ ...prev, [rideId]: '' }));
        } catch (err: any) {
            alert(err?.response?.data?.message || err?.response?.data?.Message || 'Payment update failed.');
        }
    };

    const reportDispute = async (rideId: number, endpoint: 'reportComplaint' | 'reportRideIssue') => {
        const description = (disputeDrafts[rideId] || '').trim();
        if (!description) {
            alert('Please write issue details first.');
            return;
        }

        try {
            const payload = {
                rideId,
                issueType: endpoint === 'reportRideIssue' ? 'ride_issue' : 'complaint',
                description,
                createdBy: Number(user?.appUserId || user?.AppUserId || 0),
            };
            const res = await apiClient.post(`/Dispute/${endpoint}`, payload);
            alert(res.data?.message || res.data?.Message || 'Dispute submitted.');
            setDisputeDrafts((prev) => ({ ...prev, [rideId]: '' }));
        } catch (err: any) {
            alert(err?.response?.data?.message || err?.response?.data?.Message || 'Dispute submission failed.');
        }
    };

    const productPicklist = productTypes.length ? productTypes : FALLBACK_PRODUCT_TYPES;

    return (
        <div className="min-h-screen bg-slate-50 py-8 font-sans">
            <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
                <div className="mb-8 overflow-hidden rounded-[2rem] bg-slate-900 text-white shadow-2xl shadow-slate-300/40">
                    <div className="grid gap-8 px-8 py-10 lg:grid-cols-[1.3fr_0.9fr] lg:px-10">
                        <div>
                            <div className="inline-flex items-center gap-2 rounded-full border border-white/10 bg-white/10 px-4 py-2 text-sm font-semibold text-primary-200">
                                <Truck className="h-4 w-4" />
                                Need a driver
                            </div>
                            <h1 className="mt-5 text-4xl font-extrabold tracking-tight">Customer (Logistics) Dashboard</h1>
                            <p className="mt-4 max-w-2xl text-lg leading-8 text-slate-300">
                                This page is dedicated to customers. Add a shipment request, choose the required vehicle, and track every active booking from one place.
                            </p>
                            <div className="mt-8 flex flex-wrap gap-3">
                                <button
                                    onClick={openNewShipment}
                                    className="inline-flex items-center gap-2 rounded-xl bg-primary-600 px-6 py-3 font-semibold text-white shadow-lg shadow-primary-500/25 transition-colors hover:bg-primary-500"
                                >
                                    <Package className="h-4 w-4" />
                                    Add Shipment
                                </button>
                                <button
                                    onClick={openShipmentList}
                                    className="inline-flex items-center gap-2 rounded-xl border border-white/15 bg-white/5 px-6 py-3 font-semibold text-white transition-colors hover:bg-white/10"
                                >
                                    <ClipboardList className="h-4 w-4" />
                                    View Shipments
                                </button>
                            </div>
                        </div>

                        <div className="grid gap-4 sm:grid-cols-3 lg:grid-cols-1">
                            <div className="rounded-2xl border border-white/10 bg-white/5 p-5 backdrop-blur-sm">
                                <p className="text-sm font-medium text-slate-300">Active Requests</p>
                                <p className="mt-2 text-3xl font-extrabold">{shipments.length}</p>
                            </div>
                            <div className="rounded-2xl border border-white/10 bg-white/5 p-5 backdrop-blur-sm">
                                <p className="text-sm font-medium text-slate-300">Required Details</p>
                                <p className="mt-2 text-lg font-bold">Product, vehicle, weight, location</p>
                            </div>
                            <div className="rounded-2xl border border-white/10 bg-white/5 p-5 backdrop-blur-sm">
                                <p className="text-sm font-medium text-slate-300">Route Scope</p>
                                <p className="mt-2 text-lg font-bold">Pickup and drop workflow</p>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="mb-8 flex flex-wrap items-center gap-3">
                    <button
                        onClick={openShipmentList}
                        className={`rounded-xl px-5 py-3 font-semibold transition-colors ${activeTab === 'shipments' ? 'bg-slate-900 text-white' : 'border border-slate-300 bg-white text-slate-700 hover:border-slate-400'}`}
                    >
                        Shipment Overview
                    </button>
                    <button
                        onClick={openNewShipment}
                        className={`rounded-xl px-5 py-3 font-semibold transition-colors ${activeTab === 'new' ? 'bg-primary-600 text-white' : 'border border-slate-300 bg-white text-slate-700 hover:border-primary-300 hover:text-primary-700'}`}
                    >
                        Add Shipment
                    </button>
                </div>

                {submitStatus === 'success' && (
                    <div className="mb-8 flex items-start gap-4 rounded-xl border border-emerald-200 bg-emerald-50 p-4 shadow-sm">
                        <div className="mt-0.5">
                            <CheckCircle className="h-6 w-6 text-emerald-600" />
                        </div>
                        <div>
                            <h4 className="font-bold text-emerald-800">Shipment Requested Successfully</h4>
                            <p className="mt-1 text-sm text-emerald-700">Ride request sent to available drivers within 50 km.</p>
                        </div>
                    </div>
                )}

                {submitStatus === 'error' && (
                    <div className="mb-8 rounded-xl border border-red-200 bg-red-50 p-4 text-sm text-red-700">
                        {errorMessage || 'Unable to submit shipment.'}
                    </div>
                )}

                <div className="grid grid-cols-1 gap-8 lg:grid-cols-3">
                    <div className="col-span-1 lg:col-span-2">
                        {activeTab === 'shipments' ? (
                            <div>
                                <h2 className="mb-6 flex items-center gap-2 text-xl font-bold text-slate-800">
                                    <Search className="h-5 w-5 text-primary-600" /> Active Shipments
                                </h2>

                                {shipments.length === 0 ? (
                                    <div className="rounded-2xl border border-dashed border-slate-200 bg-slate-100/50 p-12 text-center text-slate-500">
                                        <p>No active shipments found for this customer account yet.</p>
                                        <button
                                            onClick={openNewShipment}
                                            className="mt-5 inline-flex items-center gap-2 rounded-xl bg-primary-600 px-5 py-3 font-semibold text-white shadow-lg shadow-primary-500/25 transition-colors hover:bg-primary-500"
                                        >
                                            <Package className="h-4 w-4" />
                                            Add Shipment
                                        </button>
                                    </div>
                                ) : (
                                    <div className="space-y-4">
                                        {shipments.map((shipment) => (
                                            <div key={shipment.id} className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
                                                <div className="mb-4 flex items-start justify-between">
                                                    <div>
                                                        <span className="rounded-lg border border-primary-100 bg-primary-50 px-2.5 py-1 text-xs font-bold uppercase tracking-widest text-primary-600">
                                                            Ride #{shipment.id}
                                                        </span>
                                                        <h3 className="mt-3 font-bold text-slate-900">{shipment.vehicle}</h3>
                                                        <p className="mt-1 text-sm text-slate-500">
                                                            {shipment.productType} | {shipment.weight} kg | Matched drivers: {shipment.matchedCount}
                                                        </p>
                                                    </div>
                                                    <span className="rounded-lg border border-amber-200 bg-amber-50 px-3 py-1 text-sm font-semibold text-amber-700">
                                                        {shipment.status}
                                                    </span>
                                                </div>

                                                <div className="mb-4 flex items-center gap-4 rounded-xl border border-slate-100 bg-slate-50 p-4 text-sm font-medium text-slate-600">
                                                    <div className="flex items-center gap-2">
                                                        <MapPin className="h-4 w-4 text-slate-400" /> {shipment.pickup}
                                                    </div>
                                                    <span className="text-slate-300">to</span>
                                                    <div className="flex items-center gap-2">
                                                        <Navigation className="h-4 w-4 text-slate-400" /> {shipment.destination}
                                                    </div>
                                                </div>

                                                <div className="mb-3 grid gap-3 md:grid-cols-3">
                                                    <select
                                                        value={statusDrafts[shipment.id] || shipment.status}
                                                        onChange={(e) => setStatusDrafts((prev) => ({ ...prev, [shipment.id]: e.target.value as RideStatus }))}
                                                        className="rounded-xl border border-slate-300 px-3 py-2 text-sm"
                                                    >
                                                        {STATUS_OPTIONS.map((status) => (
                                                            <option key={status} value={status}>{status}</option>
                                                        ))}
                                                    </select>
                                                    <button
                                                        onClick={() => updateRideStatus(shipment.id)}
                                                        className="rounded-xl bg-slate-900 px-4 py-2 text-sm font-semibold text-white"
                                                    >
                                                        Update Ride Status
                                                    </button>
                                                    <div className="flex gap-2">
                                                        <input
                                                            value={paymentDrafts[shipment.id] || ''}
                                                            onChange={(e) => setPaymentDrafts((prev) => ({ ...prev, [shipment.id]: e.target.value }))}
                                                            className="w-full rounded-xl border border-slate-300 px-3 py-2 text-sm"
                                                            placeholder="Payment amount"
                                                            type="number"
                                                        />
                                                        <button
                                                            onClick={() => markRidePayment(shipment.id)}
                                                            className="rounded-xl bg-emerald-600 px-3 py-2 text-sm font-semibold text-white"
                                                        >
                                                            Pay
                                                        </button>
                                                    </div>
                                                </div>

                                                <div className="grid gap-3 md:grid-cols-3">
                                                    <input
                                                        value={disputeDrafts[shipment.id] || ''}
                                                        onChange={(e) => setDisputeDrafts((prev) => ({ ...prev, [shipment.id]: e.target.value }))}
                                                        className="rounded-xl border border-slate-300 px-3 py-2 text-sm md:col-span-2"
                                                        placeholder="Report complaint or ride issue"
                                                    />
                                                    <div className="flex gap-2">
                                                        <button
                                                            onClick={() => reportDispute(shipment.id, 'reportComplaint')}
                                                            className="flex-1 rounded-xl border border-slate-300 px-3 py-2 text-sm font-semibold"
                                                        >
                                                            Complaint
                                                        </button>
                                                        <button
                                                            onClick={() => reportDispute(shipment.id, 'reportRideIssue')}
                                                            className="flex-1 rounded-xl border border-red-300 px-3 py-2 text-sm font-semibold text-red-700"
                                                        >
                                                            Ride Issue
                                                        </button>
                                                    </div>
                                                </div>

                                                {(shipment.status === 'ride_started' || shipment.status === 'driver_assigned' || shipment.status === 'driver_arriving') && (
                                                    <button
                                                        onClick={() => setTrackingBooking(shipment)}
                                                        className="mt-4 w-full flex items-center justify-center gap-2 rounded-xl bg-primary-50 py-3 text-sm font-bold text-primary-700 hover:bg-primary-100 transition-colors"
                                                    >
                                                        <Navigation className="h-4 w-4" />
                                                        Track Live Location
                                                    </button>
                                                )}
                                            </div>
                                        ))}
                                    </div>
                                )}
                            </div>
                        ) : (
                            <div className="rounded-2xl border border-slate-200 bg-white p-8 shadow-sm">
                                <div className="mb-8 flex items-start justify-between gap-4">
                                    <div>
                                        <h2 className="flex items-center gap-2 text-2xl font-bold text-slate-900">
                                            <Package className="h-6 w-6 text-primary-600" /> Add Shipment
                                        </h2>
                                        <p className="mt-2 text-slate-500">Fill the shipment details below to request a driver.</p>
                                    </div>
                                    <div className="hidden rounded-2xl border border-primary-100 bg-primary-50 p-4 text-primary-700 sm:block">
                                        <p className="text-sm font-semibold">Required fields</p>
                                        <p className="mt-1 text-sm">Product type, vehicle type, product weight, pickup, and drop location.</p>
                                    </div>
                                </div>

                                <form onSubmit={handleCreateShipment} className="space-y-6">
                                    <div className="grid gap-4 md:grid-cols-2">
                                        <div>
                                            <label className="mb-2 block text-sm font-semibold text-slate-700">Product Type</label>
                                            <select
                                                required
                                                name="productType"
                                                value={formData.productType}
                                                onChange={handleChange}
                                                className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3"
                                            >
                                                <option value="">-- Select Product Type --</option>
                                                {productPicklist.map((product) => (
                                                    <option key={product.id} value={String(product.id)}>
                                                        {product.name}
                                                    </option>
                                                ))}
                                                <option value="Other">Other</option>
                                            </select>
                                            {formData.productType === 'Other' && (
                                                <input
                                                    type="text"
                                                    name="customProductType"
                                                    value={formData.customProductType}
                                                    onChange={handleChange}
                                                    placeholder="Please specify your product type..."
                                                    className="mt-2 w-full rounded-xl border border-slate-300 px-4 py-3"
                                                    required
                                                />
                                            )}
                                        </div>
                                    <div>
                                        <label className="mb-2 flex items-center gap-2 text-sm font-semibold text-slate-700">
                                            <Layout className="h-4 w-4 text-primary-600" /> Vehicle Type <span className="text-rose-500 font-semibold text-xs">(Required)</span>
                                        </label>
                                        <select
                                            name="vehicle"
                                            required
                                            value={formData.vehicle}
                                            onChange={handleChange}
                                            className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3"
                                        >
                                            <option value="">-- Select Vehicle Type --</option>
                                            {(Array.isArray(vehicleTypes) ? vehicleTypes : []).map((vt) => (
                                                <option key={vt.id} value={vt.id}>{vt.name}</option>
                                            ))}
                                        </select>
                                    </div>
                                    </div>
                                    
                                    <div className="grid gap-4 md:grid-cols-2">
                                        <div>
                                            <label className="mb-2 flex items-center gap-2 text-sm font-semibold text-slate-700">
                                                <Layout className="h-4 w-4 text-primary-600" /> Body Type <span className="text-slate-400 font-normal text-xs">(Optional)</span>
                                            </label>
                                            <select
                                                name="ctBodyType"
                                                value={formData.ctBodyType}
                                                onChange={handleChange}
                                                className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3"
                                            >
                                                <option value="">-- Any Body Type --</option>
                                                {(Array.isArray(bodyTypes) ? bodyTypes : []).map((bt) => (
                                                    <option key={bt.id} value={bt.id}>{bt.name}</option>
                                                ))}
                                            </select>
                                        </div>
                                        <div>
                                            <label className="mb-2 flex items-center gap-2 text-sm font-semibold text-slate-700">
                                                <Database className="h-4 w-4 text-primary-600" /> Vehicle Tyre <span className="text-slate-400 font-normal text-xs">(Optional)</span>
                                            </label>
                                            <select
                                                name="ctTyreType"
                                                value={formData.ctTyreType}
                                                onChange={handleChange}
                                                className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3"
                                            >
                                                <option value="">-- Any Tyre Configuration --</option>
                                                {(Array.isArray(tyreTypes) ? tyreTypes : []).map((tt) => (
                                                    <option key={tt.id} value={tt.id}>{tt.name}</option>
                                                ))}
                                            </select>
                                        </div>
                                    </div>

                                    <div className="grid gap-4 md:grid-cols-2">
                                        <div>
                                            <label className="mb-2 block text-sm font-semibold text-slate-700">Product Weight (kg)</label>
                                            <input
                                                required
                                                min="1"
                                                type="number"
                                                name="weight"
                                                value={formData.weight}
                                                onChange={handleChange}
                                                placeholder="Weight in kg"
                                                className="w-full rounded-xl border border-slate-300 px-4 py-3"
                                            />
                                        </div>
                                        <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                                            <p className="text-sm font-semibold text-slate-700">Matching note</p>
                                            <p className="mt-1 text-sm leading-6 text-slate-500">Vehicle match is based on the selected vehicle requirements and the pickup/drop details you provide.</p>
                                        </div>
                                    </div>

                                    <div className="grid gap-4 md:grid-cols-2">
                                        <div>
                                            <label className="mb-2 block text-sm font-semibold text-slate-700">Pickup Location</label>
                                            <input
                                                required
                                                type="text"
                                                name="pickup"
                                                value={formData.pickup}
                                                onChange={handleChange}
                                                placeholder="Pickup address"
                                                className="w-full rounded-xl border border-slate-300 px-4 py-3"
                                            />
                                        </div>
                                        <div>
                                            <label className="mb-2 block text-sm font-semibold text-slate-700">Drop Location</label>
                                            <input
                                                required
                                                type="text"
                                                name="destination"
                                                value={formData.destination}
                                                onChange={handleChange}
                                                placeholder="Drop address"
                                                className="w-full rounded-xl border border-slate-300 px-4 py-3"
                                            />
                                        </div>
                                    </div>

                                    <div className="rounded-2xl border border-slate-200 bg-slate-50 p-5">
                                        <div className="mb-4 flex flex-col md:flex-row items-start md:items-center justify-between gap-4">
                                            <div>
                                                <div className="mb-1 flex items-center gap-2 text-slate-800">
                                                    <MapPin className="h-5 w-5 text-primary-600" />
                                                    <h3 className="font-bold">Interactive Location Picker</h3>
                                                </div>
                                                <p className="text-sm text-slate-500">Drag the pins or click on the map to set exact pickup and drop coordinates.</p>
                                            </div>
                                        </div>

                                        <div className="h-[400px] w-full rounded-xl overflow-hidden shadow-sm border border-slate-300 mb-6">
                                            {typeof window !== 'undefined' && (
                                                <MapContainer
                                                    center={[20.5937, 78.9629]}
                                                    zoom={5}
                                                    scrollWheelZoom={true}
                                                    style={{ height: '100%', width: '100%' }}
                                                >
                                                    <TileLayer
                                                        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                                                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                                                    />
                                                    <Marker
                                                        position={[
                                                            Number.isFinite(Number(formData.pickupLat)) ? Number(formData.pickupLat) : 28.6139,
                                                            Number.isFinite(Number(formData.pickupLng)) ? Number(formData.pickupLng) : 77.2090
                                                        ]}
                                                        draggable={true}
                                                        eventHandlers={{
                                                            dragend: (e) => {
                                                                const marker = e.target;
                                                                const position = marker.getLatLng();
                                                                setFormData(p => ({ ...p, pickupLat: position.lat.toFixed(6), pickupLng: position.lng.toFixed(6) }));
                                                            },
                                                        }}
                                                    />
                                                    <Marker
                                                        position={[
                                                            Number.isFinite(Number(formData.dropLat)) ? Number(formData.dropLat) : 19.0760,
                                                            Number.isFinite(Number(formData.dropLng)) ? Number(formData.dropLng) : 72.8777
                                                        ]}
                                                        draggable={true}
                                                        eventHandlers={{
                                                            dragend: (e) => {
                                                                const marker = e.target;
                                                                const position = marker.getLatLng();
                                                                setFormData(p => ({ ...p, dropLat: position.lat.toFixed(6), dropLng: position.lng.toFixed(6) }));
                                                            },
                                                        }}
                                                    />
                                                </MapContainer>
                                            )}
                                        </div>

                                        <div className="grid gap-4 md:grid-cols-2">
                                            <div>
                                                <label className="text-xs font-semibold text-slate-500 uppercase">Pickup Coordinates</label>
                                                <div className="grid grid-cols-2 gap-2 mt-1">
                                                    <input
                                                        type="number"
                                                        step="any"
                                                        name="pickupLat"
                                                        value={formData.pickupLat}
                                                        onChange={handleChange}
                                                        placeholder="Latitude"
                                                        className="w-full rounded-lg border border-slate-300 px-3 py-2 text-sm"
                                                    />
                                                    <input
                                                        type="number"
                                                        step="any"
                                                        name="pickupLng"
                                                        value={formData.pickupLng}
                                                        onChange={handleChange}
                                                        placeholder="Longitude"
                                                        className="w-full rounded-lg border border-slate-300 px-3 py-2 text-sm"
                                                    />
                                                </div>
                                            </div>
                                            <div>
                                                <label className="text-xs font-semibold text-slate-500 uppercase">Drop Coordinates</label>
                                                <div className="grid grid-cols-2 gap-2 mt-1">
                                                    <input
                                                        type="number"
                                                        step="any"
                                                        name="dropLat"
                                                        value={formData.dropLat}
                                                        onChange={handleChange}
                                                        placeholder="Latitude"
                                                        className="w-full rounded-lg border border-slate-300 px-3 py-2 text-sm"
                                                    />
                                                    <input
                                                        type="number"
                                                        step="any"
                                                        name="dropLng"
                                                        value={formData.dropLng}
                                                        onChange={handleChange}
                                                        placeholder="Longitude"
                                                        className="w-full rounded-lg border border-slate-300 px-3 py-2 text-sm"
                                                    />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="flex items-center gap-4 pt-2">
                                        <button type="submit" disabled={submitStatus === 'loading'} className="btn-primary flex-1 rounded-xl py-4 text-lg font-bold">
                                            {submitStatus === 'loading' ? 'Processing...' : 'Submit and Find Driver'}
                                        </button>
                                        <button type="button" onClick={openShipmentList} className="rounded-xl border border-slate-300 px-6 py-4 font-bold">
                                            Cancel
                                        </button>
                                    </div>
                                </form>
                            </div>
                        )}
                    </div>

                    <div className="col-span-1 space-y-6">
                        <div className="relative overflow-hidden rounded-2xl bg-primary-600 p-6 text-white shadow-lg shadow-primary-500/20">
                            <div className="absolute right-0 bottom-0 translate-x-1/4 translate-y-1/4 opacity-10">
                                <Anchor className="h-32 w-32" />
                            </div>
                            <h3 className="relative z-10 mb-4 font-bold">Quick Profile</h3>
                            <div className="relative z-10 flex items-center gap-4">
                                <div className="flex h-12 w-12 items-center justify-center rounded-full border border-white/30 bg-white/20 backdrop-blur-sm">
                                    <User className="h-6 w-6" />
                                </div>
                                <div>
                                    <p className="text-lg font-bold leading-tight">{user?.firstName || user?.company || 'Customer'}</p>
                                    <p className="text-sm text-primary-100">Customer Account</p>
                                </div>
                            </div>
                        </div>

                        <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
                            <h3 className="mb-4 flex items-center gap-2 border-b border-slate-100 pb-2 font-bold text-slate-800">
                                <History className="h-5 w-5 text-slate-400" /> Shipment History
                            </h3>
                            <p className="text-sm leading-relaxed text-slate-600">
                                Found <strong className="text-slate-900">{shipments.length} records</strong> in the current session.
                            </p>
                        </div>

                        <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
                            <h3 className="mb-4 flex items-center gap-2 border-b border-slate-100 pb-2 font-bold text-slate-800">
                                <ArrowRightLeft className="h-5 w-5 text-slate-400" /> Shipment Checklist
                            </h3>
                            <ul className="space-y-3 text-sm text-slate-600">
                                <li>1. Enter the product type clearly.</li>
                                <li>2. Choose the required vehicle type.</li>
                                <li>3. Add product weight in kilograms.</li>
                                <li>4. Fill pickup and drop locations.</li>
                            </ul>
                        </div>

                        <div className="rounded-2xl border border-slate-200 bg-slate-50 p-6 shadow-sm">
                            <h3 className="mb-4 flex items-center gap-2 border-b border-slate-200 pb-2 font-bold text-slate-800">
                                <Info className="h-5 w-5 text-primary-600" /> Vehicle Matching
                            </h3>
                            <p className="text-sm leading-relaxed text-slate-600">
                                Matching uses backend geolocation. If exact map pins are not entered, the shipment still uses your pickup and drop addresses.
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            {/* Tracking Modal */}
            {trackingBooking && (
                <div className="fixed inset-0 z-[1000] flex items-center justify-center bg-slate-900/60 backdrop-blur-sm p-4">
                    <div className="relative w-full max-w-4xl rounded-3xl bg-white shadow-2xl overflow-hidden">
                        <div className="flex items-center justify-between border-b border-slate-100 px-6 py-4">
                            <div>
                                <h3 className="text-xl font-bold text-slate-900">Live Tracking: Ride #{trackingBooking.id}</h3>
                                <p className="text-sm text-slate-500">{trackingBooking.pickup} → {trackingBooking.destination}</p>
                            </div>
                            <button 
                                onClick={() => setTrackingBooking(null)}
                                className="rounded-full p-2 hover:bg-slate-100 transition-colors"
                            >
                                <X className="h-6 w-6 text-slate-400" />
                            </button>
                        </div>
                        <div className="p-6">
                            <TrackingMap 
                                bookingId={trackingBooking.id}
                                pickupLat={trackingBooking.pickupLat || formData.pickupLat || 28.6139}
                                pickupLng={trackingBooking.pickupLng || formData.pickupLng || 77.2090}
                                dropLat={trackingBooking.dropLat || formData.dropLat || 19.0760}
                                dropLng={trackingBooking.dropLng || formData.dropLng || 72.8777}
                            />
                            <div className="mt-6 flex items-center gap-4 rounded-2xl bg-slate-50 p-4">
                                <div className="flex h-10 w-10 items-center justify-center rounded-full bg-primary-100 text-primary-600">
                                    <Truck className="h-5 w-5" />
                                </div>
                                <div>
                                    <p className="font-bold text-slate-900">{trackingBooking.vehicle}</p>
                                    <p className="text-sm text-slate-500">Status: <span className="font-semibold text-primary-600 uppercase">{trackingBooking.status}</span></p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default CustomerDashboard;
