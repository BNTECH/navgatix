import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import markerIcon from 'leaflet/dist/images/marker-icon.png';
import markerRetinaIcon from 'leaflet/dist/images/marker-icon-2x.png';
import markerShadow from 'leaflet/dist/images/marker-shadow.png';

let isConfigured = false;

export const ensureLeafletIconsConfigured = () => {
    if (isConfigured) return;
    isConfigured = true;

    L.Icon.Default.mergeOptions({
        iconRetinaUrl: markerRetinaIcon,
        iconUrl: markerIcon,
        shadowUrl: markerShadow,
    });
};
