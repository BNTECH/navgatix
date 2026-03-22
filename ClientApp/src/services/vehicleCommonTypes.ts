import apiClient from '../api/apiClient';
import { normalizeCommonTypes, type NormalizedCommonType } from '../lib/commonTypes';

export interface VehicleCommonTypesResponse {
  vehicleTypes: NormalizedCommonType[];
  bodyTypes: NormalizedCommonType[];
  tyreTypes: NormalizedCommonType[];
}

const pickCollection = (payload: any, ...candidates: string[]): any => {
  if (!payload) return [];
  for (const key of candidates) {
    const value = payload[key];
    if (value !== undefined && value !== null) {
      return value;
    }
  }
  return [];
};

export const fetchVehicleCommonTypes = async (): Promise<VehicleCommonTypesResponse> => {
  const response = await apiClient.get('/CommonType/vehicle-types');
  const payload = response.data || {};
  return {
    vehicleTypes: normalizeCommonTypes(pickCollection(payload, 'vehicleTypes', 'VehicleTypes')),
    bodyTypes: normalizeCommonTypes(pickCollection(payload, 'bodyTypes', 'BodyTypes')),
    tyreTypes: normalizeCommonTypes(pickCollection(payload, 'tyreTypes', 'TyreTypes')),
  };
};

export interface VehicleCommonTypesCache extends VehicleCommonTypesResponse {
  timestamp: number;
}
