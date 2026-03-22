import React, { useEffect, useState } from 'react';
import { MapContainer, TileLayer, Marker, Popup, useMap, Polyline } from 'react-leaflet';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { useSignalR } from '../hooks/useSignalR';

// Fix Leaflet icon issue
import markerIcon from 'leaflet/dist/images/marker-icon.png';
import markerShadow from 'leaflet/dist/images/marker-shadow.png';

let DefaultIcon = L.icon({
  iconUrl: markerIcon,
  shadowUrl: markerShadow,
  iconSize: [25, 41],
  iconAnchor: [12, 41]
});
L.Marker.prototype.options.icon = DefaultIcon;

const TruckIcon = L.divIcon({
  html: `<div style="font-size: 24px;">🚚</div>`,
  className: 'truck-marker',
  iconSize: [30, 30],
  iconAnchor: [15, 15]
});

interface TrackingMapProps {
  bookingId: number;
  pickupLat: number;
  pickupLng: number;
  dropLat: number;
  dropLng: number;
}

const ZoomHandler = ({ location }: { location: { latitude: number; longitude: number } | null }) => {
  const map = useMap();
  React.useEffect(() => {
    if (location) {
      map.panTo([location.latitude, location.longitude]);
    }
  }, [location, map]);
  return null;
};

const TrackingMap: React.FC<TrackingMapProps> = ({ bookingId, pickupLat, pickupLng, dropLat, dropLng }) => {
  const { driverLocation } = useSignalR(bookingId);
  const [route, setRoute] = useState<[number, number][]>([]);

  useEffect(() => {
    const fetchRoute = async () => {
      try {
        const url = `https://router.project-osrm.org/route/v1/driving/${pickupLng},${pickupLat};${dropLng},${dropLat}?overview=full&geometries=geojson`;
        const response = await fetch(url);
        const data = await response.json();
        if (data.routes && data.routes.length > 0) {
          const coords = data.routes[0].geometry.coordinates.map((c: any) => [c[1], c[0]] as [number, number]);
          setRoute(coords);
        }
      } catch (err) {
        console.error('OSRM fetch failed:', err);
      }
    };
    fetchRoute();
  }, [pickupLat, pickupLng, dropLat, dropLng]);

  return (
    <div className="h-[400px] w-full rounded-xl overflow-hidden shadow-lg border border-slate-200">
      <MapContainer 
        center={[pickupLat, pickupLng]} 
        zoom={13} 
        scrollWheelZoom={true} 
        style={{ height: '100%', width: '100%' }}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />

        {/* Pickup Marker */}
        <Marker position={[pickupLat, pickupLng]}>
          <Popup>Pickup Location</Popup>
        </Marker>

        {/* Drop Marker */}
        <Marker position={[dropLat, dropLng]}>
          <Popup>Drop Location</Popup>
        </Marker>

        {/* Route Polyline */}
        {route.length > 0 && (
          <Polyline 
            positions={route} 
            color="#4f46e5" 
            weight={4} 
            opacity={0.6} 
            dashArray="10, 10"
          />
        )}

        {/* Driver Marker */}
        {driverLocation && (
          <Marker position={[driverLocation.latitude, driverLocation.longitude]} icon={TruckIcon}>
            <Popup>Driver is here</Popup>
          </Marker>
        )}

        <ZoomHandler location={driverLocation} />
      </MapContainer>
    </div>
  );
};

export default TrackingMap;
