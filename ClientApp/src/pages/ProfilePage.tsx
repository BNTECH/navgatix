import { useEffect, useMemo, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { Camera, FileText, ShieldCheck, ArrowRight, Loader2, CheckCircle2, Truck, CreditCard, Layout, Database, Building2, AlertTriangle } from 'lucide-react';
import Navbar from '../components/Navbar';
import apiClient from '../api/apiClient';
import { fetchVehicleCommonTypes } from '../services/vehicleCommonTypes';
import { type NormalizedCommonType } from '../lib/commonTypes';

const EMAIL_REGEX = /^[^\s@]+@(gmail\.com|hotmail\.com|outlook\.com|yahoo\.com)$/i;
const PHONE_REGEX = /^\d{10}$/;
const DL_REGEX = /^[A-Z0-9-]{10,16}$/i;
const GST_REGEX = /^[0-9A-Z]{15}$/;

const ProfilePage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const redirectReason = (location.state as any)?.reason;
    const userStr = localStorage.getItem('user');
    let user: any = null;
    try {
        user = userStr ? JSON.parse(userStr) : null;
    } catch (e) {
        console.error('Failed to parse user from localStorage', e);
    }
    
    const roles: string[] = Array.isArray(user?.Roles) ? user.Roles : (Array.isArray(user?.roles) ? user.roles : []);
    const role = (user?.roleName || user?.RoleName || (roles && roles.length > 0 ? roles[0] : '') || '').toLowerCase();

    const isDriver = role === 'driver';
    const isTransporter = role === 'transporter' || role === 'company';
    const isCustomer = role === 'customer';

    const [fullName, setFullName] = useState(`${user?.firstName || user?.FirstName || ''} ${user?.lastName || user?.LastName || ''}`.trim());
    const [email, setEmail] = useState(user?.email || user?.Email || '');
    const [phoneNumber, setPhoneNumber] = useState(user?.phoneNumber || user?.PhoneNumber || '');
    const [address, setAddress] = useState('');
    
    // KYC Documents
    const [aadhaarFile, setAadhaarFile] = useState<File | null>(null);
    const [aadhaarUrl, setAadhaarUrl] = useState('');
    const [panFile, setPanFile] = useState<File | null>(null);
    const [panUrl, setPanUrl] = useState('');
    
    // Driver / Vehicle Fields
    const [drivingLicenseNumber, setDrivingLicenseNumber] = useState('');
    const [vehicleName, setVehicleName] = useState('');
    const [vehicleNumber, setVehicleNumber] = useState('');
    const [gstNumber, setGstNumber] = useState('');
    const [ctVehicleType, setCtVehicleType] = useState<number | ''>('');
    const [ctBodyType, setCtBodyType] = useState<number | ''>('');
    const [ctTyreType, setCtTyreType] = useState<number | ''>('');

    // Master Data
    const [vehicleTypes, setVehicleTypes] = useState<NormalizedCommonType[]>([]);
    const [bodyTypes, setBodyTypes] = useState<NormalizedCommonType[]>([]);
    const [tyreTypes, setTyreTypes] = useState<NormalizedCommonType[]>([]);
    
    const [profilePicFile, setProfilePicFile] = useState<File | null>(null);
    const [profilePicUrl, setProfilePicUrl] = useState(user?.profilePic || user?.ProfilePic || '');

    const [isSubmitting, setIsSubmitting] = useState(false);
    const [isLoadingData, setIsLoadingData] = useState(false);
    const [successMsg, setSuccessMsg] = useState('');
    const [errors, setErrors] = useState<string[]>([]);

    const parseCommonTypeValue = (value: any): number | '' => {
        if (value === null || value === undefined || value === '') return '';
        const parsed = Number(value);
        return Number.isFinite(parsed) ? parsed : '';
    };

    const resolveFirstDefinedValue = (object: any, keys: string[]): any => {
        if (!object) return undefined;
        for (const key of keys) {
            if (object[key] !== undefined && object[key] !== null && object[key] !== '') {
                return object[key];
            }
        }
        return undefined;
    };

    useEffect(() => {
        const fetchMasterData = async () => {
            try {
                const masterTypes = await fetchVehicleCommonTypes();
                setVehicleTypes(masterTypes.vehicleTypes);
                setBodyTypes(masterTypes.bodyTypes);
                setTyreTypes(masterTypes.tyreTypes);
            } catch (err) {
                console.error('Failed to fetch master data:', err);
            }
        };

        const fetchProfile = async () => {
            const userId = localStorage.getItem('userId') || user?.userId || user?.UserId || '';
            if (!userId) return;

            setIsLoadingData(true);
            try {
                const res = await apiClient.get(`/User/getUserDetail/${userId}`);
                const data = res.data;
                if (data) {
                    // Basic Info with robust mapping
                    const firstName = resolveFirstDefinedValue(data, ['firstName', 'FirstName', 'name', 'Name']) || '';
                    const lastName = resolveFirstDefinedValue(data, ['lastName', 'LastName']) || '';
                    const mappedEmail = resolveFirstDefinedValue(data, ['email', 'Email']) || '';
                    const mappedPhone = resolveFirstDefinedValue(data, ['phoneNumber', 'PhoneNumber', 'mobile', 'Mobile']) || '';
                    const mappedAddress = resolveFirstDefinedValue(data, ['address', 'Address']) || '';
                    const mappedProfilePic = resolveFirstDefinedValue(data, ['profilePic', 'ProfilePic', 'photoUrl', 'PhotoUrl']) || '';

                    if (firstName || lastName) setFullName(`${firstName} ${lastName}`.trim());
                    if (mappedEmail) setEmail(mappedEmail);
                    if (mappedPhone) setPhoneNumber(mappedPhone);
                    if (mappedAddress) setAddress(mappedAddress);
                    if (mappedProfilePic) setProfilePicUrl(mappedProfilePic);

                    // Driver/Transporter specific
                    setDrivingLicenseNumber(resolveFirstDefinedValue(data, ['licenseNumber', 'LicenseNumber']) || '');
                    setGstNumber(resolveFirstDefinedValue(data, ['gstNumber', 'GSTNumber', 'gstNo', 'GSTNo']) || '');
                    setVehicleName(resolveFirstDefinedValue(data, ['vehicleName', 'VehicleName', 'name', 'Name']) || '');
                    setVehicleNumber(resolveFirstDefinedValue(data, ['vehicleNumber', 'VehicleNumber']) || '');

                    setCtVehicleType(
                        parseCommonTypeValue(
                            resolveFirstDefinedValue(data, [
                                'ctVehicleType',
                                'CTVehicleType',
                                'CTVehicletype',
                                'CT_VehicleType',
                                'ct_VehicleType',
                                'vehicleTypeId',
                                'VehicleTypeId'
                            ])
                        ) || ''
                    );
                    setCtBodyType(
                        parseCommonTypeValue(
                            resolveFirstDefinedValue(data, [
                                'ctBodyType',
                                'CTBodyType',
                                'CTBodytype',
                                'CT_BodyType',
                                'ct_bodyType',
                                'bodyTypeId',
                                'BodyTypeId'
                            ])
                        ) || ''
                    );
                    setCtTyreType(
                        parseCommonTypeValue(
                            resolveFirstDefinedValue(data, [
                                'ctTyreType',
                                'CTTyreType',
                                'CTTyretype',
                                'CT_TyreType',
                                'ct_tyreType',
                                'tyreTypeId',
                                'TyreTypeId'
                            ])
                        ) || ''
                    );

                    // Parse description for KYCs
                    const description = resolveFirstDefinedValue(data, ['description', 'Description']);
                    if (description && typeof description === 'string') {
                        const aadhaarMatch = description.match(/AADHAAR_URL:([^|\s]+)/);
                        if (aadhaarMatch) setAadhaarUrl(aadhaarMatch[1]);
                        
                        const panMatch = description.match(/PAN_URL:([^|\s]+)/);
                        if (panMatch) setPanUrl(panMatch[1]);
                    }
                }
            } catch (err) {
                console.error('Failed to fetch profile:', err);
            } finally {
                setIsLoadingData(false);
            }
        };

        fetchMasterData();
        fetchProfile();
    }, []);

    const previewUrl = useMemo(() => {
        if (profilePicFile) {
            return URL.createObjectURL(profilePicFile);
        }
        return profilePicUrl;
    }, [profilePicFile, profilePicUrl]);

    const validate = (): boolean => {
        const errs: string[] = [];

        if (!fullName.trim() || fullName.trim().length < 2) errs.push('Full name must be at least 2 characters.');
        if (!EMAIL_REGEX.test(email.trim())) errs.push('Email must use gmail.com, hotmail.com, outlook.com, or yahoo.com.');
        if (!PHONE_REGEX.test(phoneNumber.trim())) errs.push('Phone number must be exactly 10 digits.');
        if (!address.trim() || address.trim().length < 10) errs.push('Address must be at least 10 characters.');
        
        // Aadhaar: Required for Driver and Transporter, Optional for others
        if ((isDriver || isTransporter) && !aadhaarFile && !aadhaarUrl) {
            errs.push('Aadhaar document upload is mandatory.');
        }

        const allowedDocTypes = ['application/pdf', 'image/jpeg', 'image/png', 'image/jpg', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];
        
        if (aadhaarFile && !allowedDocTypes.includes(aadhaarFile.type)) {
            errs.push('Aadhaar file must be PDF, JPG, PNG, or DOC/DOCX.');
        }

        if (panFile && !allowedDocTypes.includes(panFile.type)) {
            errs.push('PAN file must be PDF, JPG, PNG, or DOC/DOCX.');
        }

        // Driver mandatory fields
        if (isDriver) {
            if (!DL_REGEX.test(drivingLicenseNumber.trim())) errs.push('Valid driving license number is required.');
            if (!vehicleName.trim()) errs.push('Vehicle name is mandatory.');
            if (!vehicleNumber.trim()) errs.push('Vehicle number is mandatory.');
        }

        // Transporter fields
        if (isTransporter) {
            if (!DL_REGEX.test(drivingLicenseNumber.trim())) errs.push('Driving license number must be 10 to 16 characters.');
            if (gstNumber && !GST_REGEX.test(gstNumber.trim().toUpperCase())) errs.push('GST number must be exactly 15 characters.');
        }

        if (profilePicFile && profilePicFile.size > 2 * 1024 * 1024) errs.push('Profile photo must be 2MB or smaller.');
        if (profilePicFile && !['image/jpeg', 'image/png', 'image/jpg'].includes(profilePicFile.type)) errs.push('Profile photo must be JPG or PNG.');

        setErrors(errs);
        return errs.length === 0;
    };

    const uploadFile = async (file: File | null, userId: string, type: 'Aadhaar' | 'PAN' | 'Profile') => {
        if (!file) return null;

        const formData = new FormData();
        formData.append('File', file);
        formData.append('UserId', userId);
        formData.append('DocumentType', type);

        const endpoint = type === 'Profile' ? '/User/uploadProfile' : '/User/UploadDriverKYC';
        const res = await apiClient.post(endpoint, formData, {
            headers: { 'Content-Type': 'multipart/form-data' }
        });

        return res.data?.kycUrl || res.data?.profilePic || res.data?.ProfilePic || res.data?.KycUrl;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!validate()) return;

        setIsSubmitting(true);
        setErrors([]);
        setSuccessMsg('');

        try {
            const nameParts = fullName.trim().split(/\s+/);
            const firstName = nameParts[0] || '';
            const lastName = nameParts.slice(1).join(' ');
            const userIdNum = localStorage.getItem('userId') || user?.userId || user?.UserId || '';
            
            const [savedProfilePic, savedAadhaar, savedPan] = await Promise.all([
                uploadFile(profilePicFile, userIdNum, 'Profile'),
                uploadFile(aadhaarFile, userIdNum, 'Aadhaar'),
                uploadFile(panFile, userIdNum, 'PAN')
            ]);

            const finalAadhaarUrl = savedAadhaar || aadhaarUrl;
            const finalPanUrl = savedPan || panUrl;

            let endpoint = '/User/updateUser';
            let payload: any = {
                UserId: userIdNum,
                FirstName: firstName,
                LastName: lastName,
                Email: email.trim().toLowerCase(),
                PhoneNumber: phoneNumber.trim(),
                ProfilePic: savedProfilePic || undefined,
            };

            if (isDriver) {
                endpoint = '/User/updateDriverDetail';
                payload = {
                    ...payload,
                    WhatsAppLink: address.trim(),
                    LicenseNumber: drivingLicenseNumber.trim().toUpperCase(),
                    VehicleName: vehicleName.trim(),
                    VehicleNumber: vehicleNumber.trim().toUpperCase(),
                    CT_VehicleType: ctVehicleType || undefined,
                    CTBodyType: ctBodyType || undefined,
                    CTTyreType: ctTyreType || undefined,
                    PANCardUrl: finalPanUrl || undefined,
                    Status: "Finalizing", // Flag for backend to perform strict validation
                    DOB: new Date().toISOString()
                };
            } else if (isTransporter) {
                endpoint = '/User/updateTransporterDetail';
                payload = {
                    ...payload,
                    LicenseNumber: drivingLicenseNumber.trim().toUpperCase() || undefined,
                    GSTNumber: gstNumber.trim().toUpperCase() || undefined,
                };
            } else if (isCustomer) {
                endpoint = '/User/updateCustomerDetail';
                payload = {
                    ...payload,
                    Address: address.trim(),
                };
            }

            // Also attach Aadhaar URL to description for legacy support if needed
            if (finalAadhaarUrl) {
                payload.Description = `AADHAAR_URL:${finalAadhaarUrl}${finalPanUrl ? `|PAN_URL:${finalPanUrl}` : ''}`;
            }

            await apiClient.post(endpoint, payload);

            const updatedUserRes = await apiClient.get(`/User/getUserDetail/${userIdNum}`);
            if (updatedUserRes.data) {
                localStorage.setItem('user', JSON.stringify(updatedUserRes.data));
                setProfilePicUrl(updatedUserRes.data.profilePic || updatedUserRes.data.ProfilePic || profilePicUrl);
            }

            setSuccessMsg('Profile updated successfully!');
            setProfilePicFile(null);
            setAadhaarFile(null);
            setPanFile(null);

            setTimeout(() => {
                if (isCustomer) navigate('/customer-portal');
                else if (isDriver) navigate('/driver-dashboard');
                else navigate('/transporter-dashboard');
            }, 2000);

        } catch (error: any) {
            console.error('Failed to update profile:', error);
            setErrors([error?.response?.data?.message || error?.response?.data?.Message || 'Unable to update profile. Please check all mandatory fields.']);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="min-h-screen bg-slate-50 font-sans pb-20">
            <Navbar />
            <div className="max-w-4xl mx-auto px-4 py-12">
                <div className="text-center mb-10">
                  <div className="bg-primary-600 text-white text-[10px] font-bold py-1 px-4 rounded-full inline-block mb-4 shadow-sm">
                    BUILD UPDATED: 2026-03-16 21:30
                  </div>
                  <h2 className="text-4xl font-extrabold text-slate-900 mb-2">My Profile</h2>
                  <p className="text-slate-500 font-medium">Manage your personal and business details</p>
                </div>

                {redirectReason === 'incomplete_profile' && (
                    <div className="mb-8 p-6 bg-amber-50 border-l-4 border-amber-500 rounded-r-2xl animate-in slide-in-from-top-4">
                        <div className="flex items-center gap-4">
                            <div className="h-12 w-12 bg-amber-100 rounded-full flex items-center justify-center flex-shrink-0">
                                <AlertTriangle className="h-6 w-6 text-amber-600" />
                            </div>
                            <div>
                                <h3 className="text-lg font-bold text-amber-900">Action Required: Complete Your Profile</h3>
                                <p className="text-amber-800 text-sm">You must fill in all mandatory fields and upload your Aadhaar card to access your dashboard and start receiving requests.</p>
                            </div>
                        </div>
                    </div>
                )}

                {isLoadingData ? (
                    <div className="flex flex-col items-center justify-center p-20 bg-white rounded-3xl shadow-xl border border-slate-200">
                        <Loader2 className="h-12 w-12 text-primary-600 animate-spin mb-4" />
                        <p className="text-slate-500 font-medium text-lg">Loading your profile details...</p>
                    </div>
                ) : (
                    <div className="bg-white rounded-3xl shadow-xl border border-slate-200 p-8 md:p-12">
                    <form onSubmit={handleSubmit} className="space-y-12" noValidate>
                        {/* Profile Photo Section */}
                        <div className="space-y-6">
                            <h3 className="text-xl font-bold text-slate-900 border-b border-slate-100 pb-3 flex items-center gap-2">
                                <Camera className="h-5 w-5 text-primary-600" />
                                Profile Photo
                            </h3>
                            <div className="flex flex-col items-center gap-4">
                                <div className="h-32 w-32 overflow-hidden rounded-full border-4 border-white ring-4 ring-primary-100 bg-slate-100 shadow-lg">
                                    {previewUrl ? (
                                        <img src={previewUrl} alt="Profile" className="h-full w-full object-cover" />
                                    ) : (
                                        <div className="flex h-full w-full items-center justify-center text-slate-400">
                                            <Camera className="h-12 w-12" />
                                        </div>
                                    )}
                                </div>
                                <label className="cursor-pointer rounded-xl border-2 border-dashed border-slate-300 bg-slate-50 px-6 py-3 text-sm font-bold text-slate-700 hover:border-primary-500 hover:bg-primary-50 transition-all flex items-center gap-2">
                                    Choose New Photo
                                    <input
                                        type="file"
                                        accept=".jpg,.jpeg,.png"
                                        className="hidden"
                                        onChange={(e) => setProfilePicFile(e.target.files?.[0] || null)}
                                    />
                                </label>
                                <p className="text-xs text-slate-400">JPG or PNG, max 2MB.</p>
                            </div>
                        </div>

                        {/* Personal Information Section */}
                        <div className="space-y-6">
                            <h3 className="text-xl font-bold text-slate-900 border-b border-slate-100 pb-3">Personal Information</h3>
                            <div className="grid md:grid-cols-2 gap-8">
                                <div>
                                    <label className="block text-sm font-bold text-slate-700 mb-2">Full Name <span className="text-red-500">*</span></label>
                                    <input value={fullName} onChange={(e) => setFullName(e.target.value)} maxLength={50} className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3.5 text-slate-900 focus:border-primary-500 focus:ring-4 focus:ring-primary-500/10 outline-none transition-all placeholder:text-slate-400" placeholder="Enter full name" />
                                </div>
                                <div>
                                    <label className="block text-sm font-bold text-slate-700 mb-2">Email Address <span className="text-red-500">*</span></label>
                                    <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} maxLength={60} className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3.5 text-slate-900 focus:border-primary-500 focus:ring-4 focus:ring-primary-500/10 outline-none transition-all placeholder:text-slate-400" placeholder="name@email.com" />
                                </div>
                                <div>
                                    <label className="block text-sm font-bold text-slate-700 mb-2">Phone Number <span className="text-red-500">*</span></label>
                                    <div className="relative">
                                        <span className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 font-medium">+91</span>
                                        <input value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value.replace(/\D/g, '').slice(0, 10))} maxLength={10} className="w-full rounded-xl border border-slate-300 bg-white pl-12 pr-4 py-3.5 text-slate-900 focus:border-primary-500 focus:ring-4 focus:ring-primary-500/10 outline-none transition-all placeholder:text-slate-400" placeholder="10 digit mobile number" />
                                    </div>
                                </div>
                                <div className="md:col-span-2">
                                    <label className="block text-sm font-bold text-slate-700 mb-2">Complete Address <span className="text-red-500">*</span></label>
                                    <textarea value={address} onChange={(e) => setAddress(e.target.value)} rows={3} maxLength={250} className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3.5 text-slate-900 focus:border-primary-500 focus:ring-4 focus:ring-primary-500/10 outline-none transition-all placeholder:text-slate-400 resize-none" placeholder="Flat No, Street, Locality, City, State, Pincode" />
                                </div>
                            </div>
                        </div>

                        {/* Verification & KYC Section */}
                        <div className="space-y-6">
                            <h3 className="text-xl font-bold text-slate-900 border-b border-slate-100 pb-3 flex items-center justify-between">
                                Verification Details
                                <span className={`text-xs px-3 py-1 rounded-full ${isDriver ? 'bg-orange-100 text-orange-700' : isCustomer ? 'bg-emerald-100 text-emerald-700' : 'bg-primary-100 text-primary-700'}`}>
                                    {role.toUpperCase()}
                                </span>
                            </h3>
                            <div className="grid md:grid-cols-2 gap-8">
                                {/* Aadhaar Section */}
                                <div className="space-y-2">
                                    <label className="text-sm font-bold text-slate-700 flex items-center gap-2">
                                        <ShieldCheck className="h-4 w-4 text-primary-600" /> Aadhaar Card
                                        {(isDriver || isTransporter) && <span className="text-red-500">*</span>}
                                        {!isDriver && !isTransporter && <span className="text-slate-400 font-normal text-xs">(Optional)</span>}
                                    </label>
                                    <div className="flex items-center gap-3">
                                        <div className="relative flex-1">
                                            <input 
                                                type="file"
                                                accept=".jpg,.jpeg,.png,.pdf,.doc,.docx"
                                                onChange={(e) => setAadhaarFile(e.target.files?.[0] || null)}
                                                className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-sm focus:border-primary-500 outline-none"
                                            />
                                            {aadhaarUrl && (
                                                <div className="absolute right-3 top-1/2 -translate-y-1/2 flex items-center gap-1 text-emerald-600 font-bold text-xs bg-emerald-50 px-2 py-1 rounded-lg border border-emerald-100">
                                                    <CheckCircle2 className="h-3 w-3" /> Uploaded
                                                </div>
                                            )}
                                        </div>
                                    </div>
                                    <p className="text-[10px] text-slate-400">PDF, JPG, PNG or DOCX. Drivers must upload this for verification.</p>
                                </div>

                                {/* Driving License - Only Driver/Transporter */}
                                {(isDriver || isTransporter) && (
                                    <div className="space-y-2">
                                        <label className="text-sm font-bold text-slate-700 flex items-center gap-2">
                                            <FileText className="h-4 w-4 text-primary-600" /> Driving License Number <span className="text-red-500">*</span>
                                        </label>
                                        <input 
                                            value={drivingLicenseNumber} 
                                            onChange={(e) => setDrivingLicenseNumber(e.target.value.toUpperCase().replace(/[^A-Z0-9-]/g, '').slice(0, 16))} 
                                            className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 focus:border-primary-500 outline-none placeholder:text-slate-400 uppercase"
                                            placeholder="DL-XX-XXXX-XXXXXXX"
                                            maxLength={16}
                                        />
                                        <p className="text-[10px] text-slate-400">10 to 16 alphanumeric characters.</p>
                                    </div>
                                )}

                                {/* PAN Card - Mandatory Driver, Gone for Customer */}
                                {(isDriver || isTransporter) && (
                                    <div className="space-y-2">
                                        <label className="text-sm font-bold text-slate-700 flex items-center gap-2">
                                            <CreditCard className="h-4 w-4 text-primary-600" /> PAN Card Document
                                            <span className="text-slate-400 font-normal text-xs">(Optional)</span>
                                        </label>
                                        <div className="relative">
                                            <input 
                                                type="file"
                                                accept=".jpg,.jpeg,.png,.pdf,.doc,.docx"
                                                onChange={(e) => setPanFile(e.target.files?.[0] || null)}
                                                className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-sm"
                                            />
                                            {panUrl && (
                                                <div className="absolute right-3 top-1/2 -translate-y-1/2 flex items-center gap-1 text-emerald-600 font-bold text-xs bg-emerald-50 px-2 py-1 rounded-lg border border-emerald-100">
                                                    <CheckCircle2 className="h-3 w-3" /> Uploaded
                                                </div>
                                            )}
                                        </div>
                                        <p className="text-[10px] text-slate-400">Upload clear scan of PAN Card.</p>
                                    </div>
                                )}

                                {/* GST Number - Only Transporter */}
                                {isTransporter && (
                                    <div className="space-y-2">
                                        <label className="text-sm font-bold text-slate-700 flex items-center gap-2">
                                            <Building2 className="h-4 w-4 text-primary-600" /> GST Number
                                            <span className="text-slate-400 font-normal text-xs">(Optional)</span>
                                        </label>
                                        <input 
                                            value={gstNumber} 
                                            onChange={(e) => setGstNumber(e.target.value.toUpperCase().replace(/[^A-Z0-9]/g, '').slice(0, 15))} 
                                            className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 focus:border-primary-500 outline-none placeholder:text-slate-400 uppercase"
                                            placeholder="15 character GSTIN"
                                            maxLength={15}
                                        />
                                    </div>
                                )}
                            </div>
                        </div>

                        {/* Driver / Vehicle Details Section */}
                        {isDriver && (
                            <div className="space-y-6 p-8 bg-slate-50 rounded-3xl border border-slate-200">
                                <h3 className="text-xl font-bold text-slate-900 flex items-center gap-2">
                                    <Truck className="h-5 w-5 text-primary-600" />
                                    Vehicle Details
                                </h3>
                                <div className="grid md:grid-cols-2 gap-8">
                                    <div>
                                        <label className="block text-sm font-bold text-slate-700 mb-2">Vehicle Name <span className="text-red-500">*</span></label>
                                        <input 
                                            type="text" 
                                            value={vehicleName} 
                                            onChange={(e) => setVehicleName(e.target.value)} 
                                            className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 focus:border-primary-500 outline-none transition-all placeholder:text-slate-400" 
                                            placeholder="e.g. Tata Prima 40 Tons"
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-bold text-slate-700 mb-2">Vehicle Number <span className="text-red-500">*</span></label>
                                        <input 
                                            type="text" 
                                            value={vehicleNumber} 
                                            onChange={(e) => setVehicleNumber(e.target.value.toUpperCase())} 
                                            maxLength={13}
                                            className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 focus:border-primary-500 outline-none transition-all placeholder:text-slate-400 uppercase"
                                            placeholder="Vehicle number (e.g. KA01AB1234)"
                                        />
                                    </div>
                                    <div>
                                        <label className="mb-2 flex items-center gap-2 text-sm font-semibold text-slate-700">
                                            <Truck className="h-4 w-4 text-primary-600" /> Vehicle Type <span className="text-slate-400 font-normal text-xs">(Optional)</span>
                                        </label>
                                        <select
                                            value={ctVehicleType}
                                            onChange={(e) => setCtVehicleType(e.target.value ? Number(e.target.value) : '')}
                                            className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 focus:border-primary-500 outline-none"
                                        >
                                            <option value="">Select Vehicle Type</option>
                                            {Array.isArray(vehicleTypes) && vehicleTypes.map((type) => (
                                                <option key={type.id} value={type.id}>{type.name}</option>
                                            ))}
                                        </select>
                                    </div>
                                    
                                    <div>
                                        <label className="mb-2 flex items-center gap-2 text-sm font-semibold text-slate-700">
                                            <Layout className="h-4 w-4 text-primary-600" /> Body Type <span className="text-slate-400 font-normal text-xs">(Optional)</span>
                                        </label>
                                        <select
                                            value={ctBodyType}
                                            onChange={(e) => setCtBodyType(e.target.value ? Number(e.target.value) : '')}
                                            className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 focus:border-primary-500 outline-none"
                                        >
                                            <option value="">Select Body Type</option>
                                            {Array.isArray(bodyTypes) && bodyTypes.map((t) => (
                                                <option key={t.id} value={t.id}>{t.name}</option>
                                            ))}
                                        </select>
                                    </div>
                                    <div>
                                        <label className="mb-2 flex items-center gap-2 text-sm font-semibold text-slate-700">
                                            <Database className="h-4 w-4 text-primary-600" /> Vehicle Tyre <span className="text-slate-400 font-normal text-xs">(Optional)</span>
                                        </label>
                                        <select
                                            value={ctTyreType}
                                            onChange={(e) => setCtTyreType(e.target.value ? Number(e.target.value) : '')}
                                            className="w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-slate-900 focus:border-primary-500 outline-none"
                                        >
                                            <option value="">Select Tyre Type</option>
                                            {Array.isArray(tyreTypes) && tyreTypes.map((t) => (
                                                <option key={t.id} value={t.id}>{t.name}</option>
                                            ))}
                                        </select>
                                    </div>
                                </div>
                            </div>
                        )}

                        {/* Error Messages */}
                        {errors.length > 0 && (
                            <div className="p-6 bg-red-50 border border-red-200 rounded-2xl flex items-start gap-4">
                                <div className="h-6 w-6 rounded-full bg-red-100 flex items-center justify-center flex-shrink-0">
                                    <span className="text-red-600 font-bold">!</span>
                                </div>
                                <div className="space-y-1">
                                    <p className="text-sm font-bold text-red-800">Please correct the following errors:</p>
                                    <ul className="list-disc list-inside space-y-0.5">
                                        {errors.map((err, i) => (
                                            <li key={i} className="text-xs text-red-700">{err}</li>
                                        ))}
                                    </ul>
                                </div>
                            </div>
                        )}

                        {/* Success Message */}
                        {successMsg && (
                            <div className="p-6 bg-emerald-50 text-emerald-700 rounded-2xl font-bold border border-emerald-200 text-center flex items-center justify-center gap-2 shadow-sm animate-in fade-in slide-in-from-bottom-2">
                                <CheckCircle2 className="h-6 w-6" /> {successMsg}
                            </div>
                        )}

                        {/* Submit Button */}
                        <div className="pt-8">
                            <button 
                                type="submit" 
                                disabled={isSubmitting} 
                                className="w-full bg-primary-600 hover:bg-primary-500 text-white font-extrabold py-5 rounded-2xl text-xl shadow-xl shadow-primary-600/20 active:scale-[0.98] transition-all flex items-center justify-center gap-3 disabled:opacity-70"
                            >
                                {isSubmitting ? (
                                    <><Loader2 className="h-6 w-6 animate-spin" /> Finalizing Profile...</>
                                ) : (
                                    <>Save & Finalize Profile <ArrowRight className="h-6 w-6" /></>
                                )}
                            </button>
                            <p className="text-center text-xs text-slate-400 mt-4 font-medium italic">
                                * Mandatory fields are required to access your dashboard and start received requests.
                            </p>
                        </div>
                    </form>
                </div>
                )}
            </div>
        </div>
    );
};

export default ProfilePage;
