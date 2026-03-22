import React, { useState, useEffect } from 'react';
import { X, Truck, Loader2, CheckCircle2, Layout, Database } from 'lucide-react';
import apiClient from '../api/apiClient';
import { fetchVehicleCommonTypes, type VehicleCommonTypesCache } from '../services/vehicleCommonTypes';

interface VehicleModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    userId: string;
}

const VehicleModal: React.FC<VehicleModalProps> = ({ isOpen, onClose, onSuccess, userId }) => {
    const [vehicleName, setVehicleName] = useState('');
    const [vehicleNumber, setVehicleNumber] = useState('');
    const [ctVehicleType, setCtVehicleType] = useState<number | ''>('');
    const [ctBodyType, setCtBodyType] = useState<number | ''>('');
    const [ctTyreType, setCtTyreType] = useState<number | ''>('');
    
    const [vehicleTypes, setVehicleTypes] = useState<any[]>([]);
    const [bodyTypes, setBodyTypes] = useState<any[]>([]);
    const [tyreTypes, setTyreTypes] = useState<any[]>([]);
    
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState(false);

    useEffect(() => {
        if (isOpen) {
            const fetchMasterData = async () => {
                try {
                    const cachedDataString = localStorage.getItem('navgatixMasterData');
                    const now = Date.now();

                    if (cachedDataString) {
                        try {
                            const parsed: VehicleCommonTypesCache = JSON.parse(cachedDataString);
                            if (now - parsed.timestamp < 1000 * 60 * 30) {
                                setVehicleTypes(parsed.vehicleTypes || []);
                                setBodyTypes(parsed.bodyTypes || []);
                                setTyreTypes(parsed.tyreTypes || []);
                                return;
                            }
                        } catch {
                            // invalid cache, continue to refresh
                        }
                    }

                    const masterData = await fetchVehicleCommonTypes();
                    setVehicleTypes(masterData.vehicleTypes);
                    setBodyTypes(masterData.bodyTypes);
                    setTyreTypes(masterData.tyreTypes);
                    localStorage.setItem('navgatixMasterData', JSON.stringify({ ...masterData, timestamp: now }));
                } catch (err) {
                    console.error('Failed to fetch master data:', err);
                }
            };
            fetchMasterData();
            
            // Reset state
            setVehicleName('');
            setVehicleNumber('');
            setCtVehicleType('');
            setCtBodyType('');
            setCtTyreType('');
            setError('');
            setSuccess(false);
        }
    }, [isOpen]);

    if (!isOpen) return null;

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        
        if (!vehicleName.trim() || !vehicleNumber.trim() || !ctVehicleType) {
            setError('Vehicle name, number and type are mandatory.');
            return;
        }

        setIsSubmitting(true);
        try {
            await apiClient.post('/Vehicle/saveVehicle', {
                VehicleName: vehicleName.trim(),
                VehicleNumber: vehicleNumber.trim().toUpperCase(),
                Ct_VehicleType: Number(ctVehicleType),
                CtBodyType: ctBodyType ? Number(ctBodyType) : null,
                CtTyreType: ctTyreType ? Number(ctTyreType) : null,
                UserId: userId || localStorage.getItem('userId')
            });
            setSuccess(true);
            setTimeout(() => {
                onSuccess();
                onClose();
            }, 2000);
        } catch (err: any) {
            setError(err?.response?.data?.message || err?.response?.data?.Message || 'Failed to save vehicle.');
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-slate-900/60 backdrop-blur-sm">
            <div className="bg-white rounded-3xl shadow-2xl w-full max-w-xl overflow-hidden animate-in fade-in zoom-in duration-300">
                <div className="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50">
                    <div className="flex items-center gap-3">
                        <div className="w-10 h-10 bg-primary-600 rounded-xl flex items-center justify-center shadow-lg shadow-primary-500/20">
                            <Truck className="text-white h-5 w-5" />
                        </div>
                        <h2 className="text-xl font-bold text-slate-900">Add New Vehicle</h2>
                    </div>
                    <button onClick={onClose} className="p-2 text-slate-400 hover:text-slate-600 hover:bg-slate-200 rounded-full transition-colors">
                        <X className="h-5 w-5" />
                    </button>
                </div>

                <div className="p-8">
                    {success ? (
                        <div className="flex flex-col items-center justify-center py-6 text-center">
                            <div className="w-16 h-16 bg-emerald-100 rounded-full flex items-center justify-center mb-4">
                                <CheckCircle2 className="h-10 w-10 text-emerald-600" />
                            </div>
                            <h3 className="text-xl font-bold text-slate-900 mb-2">Vehicle Saved!</h3>
                            <p className="text-slate-500">Your vehicle has been successfully registered.</p>
                        </div>
                    ) : (
                        <form onSubmit={handleSubmit} className="space-y-6">
                            {error && (
                                <div className="p-4 bg-red-50 border border-red-100 text-red-700 text-sm rounded-xl font-medium">
                                    {error}
                                </div>
                            )}

                            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                                <div>
                                    <label className="block text-sm font-bold text-slate-700 mb-2">Vehicle Name <span className="text-red-500">*</span></label>
                                    <input
                                        type="text"
                                        value={vehicleName}
                                        onChange={(e) => setVehicleName(e.target.value)}
                                        className="w-full rounded-xl border border-slate-300 px-4 py-3 text-slate-900 focus:border-primary-500 focus:ring-4 focus:ring-primary-500/10 outline-none transition-all"
                                        placeholder="e.g. TATA Prima 40 Tons"
                                        required
                                    />
                                </div>

                                <div>
                                    <label className="block text-sm font-bold text-slate-700 mb-2">Vehicle Number <span className="text-red-500">*</span></label>
                                    <input
                                        type="text"
                                        value={vehicleNumber}
                                        onChange={(e) => setVehicleNumber(e.target.value.toUpperCase())}
                                        className="w-full rounded-xl border border-slate-300 px-4 py-3 text-slate-900 focus:border-primary-500 focus:ring-4 focus:ring-primary-500/10 outline-none transition-all uppercase"
                                        placeholder="e.g. MH-12-AB-1234"
                                        required
                                    />
                                </div>

                                <div>
                                    <label className="block text-sm font-bold text-slate-700 mb-2 flex items-center gap-2">
                                        <Layout className="h-4 w-4 text-primary-600" /> Vehicle Type <span className="text-red-500">*</span>
                                    </label>
                                    <select
                                        value={ctVehicleType}
                                        onChange={(e) => setCtVehicleType(e.target.value ? Number(e.target.value) : '')}
                                        className="w-full rounded-xl border border-slate-300 px-4 py-3 text-slate-900 focus:border-primary-500 outline-none transition-all bg-white"
                                        required
                                    >
                                        <option value="">Select Type</option>
                                        {vehicleTypes.map((vt) => (
                                            <option key={vt.id} value={vt.id}>{vt.name}</option>
                                        ))}
                                    </select>
                                </div>

                                <div>
                                    <label className="block text-sm font-bold text-slate-700 mb-2 flex items-center gap-2">
                                        <Layout className="h-4 w-4 text-primary-600" /> Body Type
                                    </label>
                                    <select
                                        value={ctBodyType}
                                        onChange={(e) => setCtBodyType(e.target.value ? Number(e.target.value) : '')}
                                        className="w-full rounded-xl border border-slate-300 px-4 py-3 text-slate-900 focus:border-primary-500 outline-none transition-all bg-white"
                                    >
                                        <option value="">Select Body Type</option>
                                        {bodyTypes.map((bt) => (
                                            <option key={bt.id} value={bt.id}>{bt.name}</option>
                                        ))}
                                    </select>
                                </div>

                                <div>
                                    <label className="block text-sm font-bold text-slate-700 mb-2 flex items-center gap-2">
                                        <Database className="h-4 w-4 text-primary-600" /> Tyre Configuration
                                    </label>
                                    <select
                                        value={ctTyreType}
                                        onChange={(e) => setCtTyreType(e.target.value ? Number(e.target.value) : '')}
                                        className="w-full rounded-xl border border-slate-300 px-4 py-3 text-slate-900 focus:border-primary-500 outline-none transition-all bg-white"
                                    >
                                        <option value="">Select Tyre Count</option>
                                        {tyreTypes.map((tt) => (
                                            <option key={tt.id} value={tt.id}>{tt.name}</option>
                                        ))}
                                    </select>
                                </div>
                            </div>

                            <button
                                type="submit"
                                disabled={isSubmitting}
                                className="w-full bg-primary-600 hover:bg-primary-500 text-white font-extrabold py-5 rounded-2xl text-xl shadow-xl shadow-primary-600/20 active:scale-[0.98] transition-all flex items-center justify-center gap-3 disabled:opacity-70"
                            >
                                {isSubmitting ? (
                                    <>
                                        <Loader2 className="h-6 w-6 animate-spin" />
                                        Registering Vehicle...
                                    </>
                                ) : (
                                    'Register Vehicle'
                                )}
                            </button>
                        </form>
                    )}
                </div>
            </div>
        </div>
    );
};

export default VehicleModal;
