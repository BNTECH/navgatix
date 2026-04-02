import { useMemo } from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import { ensureLeafletIconsConfigured } from '../lib/leafletHelpers';

interface LiveFleetMapProps {
    vehicles: Array<{
        vehicleNumber?: string;
        driverName?: string;
        latitude?: number;
        longitude?: number;
        liveStatus?: string;
    }>;
}

const LiveFleetMap: React.FC<LiveFleetMapProps> = ({ vehicles }) => {
    ensureLeafletIconsConfigured();

    const points = useMemo(
        () =>
            vehicles
                .filter((vehicle) => typeof vehicle.latitude === 'number' && typeof vehicle.longitude === 'number')
                .map((vehicle) => ({
                    lat: Number(vehicle.latitude),
                    lng: Number(vehicle.longitude),
                    title: vehicle.vehicleNumber || 'Vehicle',
                    driver: vehicle.driverName,
                    liveStatus: vehicle.liveStatus,
                })),
        [vehicles]
    );

    const center = useMemo(() => {
        if (!points.length) {
            return [20.5937, 78.9629]; // India center
        }

        const total = points.reduce(
            (acc, point) => {
                acc.lat += point.lat;
                acc.lng += point.lng;
                return acc;
            },
            { lat: 0, lng: 0 }
        );

        return [total.lat / points.length, total.lng / points.length];
    }, [points]);

    if (!points.length) {
        return (
            <div className="h-[280px] rounded-2xl border border-dashed border-slate-200 bg-slate-50 grid place-items-center text-sm text-slate-500">
                No live locations reported yet.
            </div>
        );
    }

    return (
        <div className="h-[280px] rounded-2xl border border-slate-200 overflow-hidden">
            <MapContainer center={center as [number, number]} zoom={5} scrollWheelZoom className="h-full w-full">
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                {points.map((point, index) => (
                    <Marker key={`${point.lat}-${point.lng}-${index}`} position={[point.lat, point.lng]}>
                        <Popup>
                            <div className="text-sm">
                                <p className="font-bold text-slate-900">{point.title}</p>
                                {point.driver && <p className="text-slate-500 text-xs">{point.driver}</p>}
                                {point.liveStatus && <p className="text-xs text-slate-500">{point.liveStatus}</p>}
                            </div>
                        </Popup>
                    </Marker>
                ))}
            </MapContainer>
        </div>
    );
};

export default LiveFleetMap;
