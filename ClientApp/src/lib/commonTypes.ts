export interface NormalizedCommonType {
  id: number;
  name: string;
  code?: string;
  keys?: string;
  valueStr?: string;
  orderBy?: number;
  raw?: any;
}

const toNumber = (value: any): number | undefined => {
  if (value === null || value === undefined || value === '') return undefined;
  const parsed = typeof value === 'number' ? value : Number(value);
  return Number.isFinite(parsed) ? parsed : undefined;
};

const pickName = (item: any): string => {
  return (
    item.name ??
    item.Name ??
    item.commonTypeName ??
    item.CommonTypeName ??
    item.vehicleTypeName ??
    item.VehicleTypeName ??
    item.bodyTypeName ??
    item.BodyTypeName ??
    item.tyreTypeName ??
    item.TyreTypeName ??
    ''
  );
};

export const normalizeCommonTypes = (data: any): NormalizedCommonType[] => {
  const items = data?.$values || data;
  if (!Array.isArray(items)) {
    console.warn('normalizeCommonTypes: items is not an array', items);
    return [];
  }
  return items
    .map((item): NormalizedCommonType | null => {
      const candidateId =
        item.id ??
        item.Id ??
        item.commonTypeId ??
        item.CommonTypeId ??
        item.ctid ??
        item.CTID ??
        item.commonTypeID;
      const parsedId = toNumber(candidateId);
      if (!Number.isFinite(parsedId ?? NaN)) {
        return null;
      }

      return {
        id: parsedId!,
        name: pickName(item) || `Item ${parsedId}`,
        code: item.code ?? item.Code,
        keys: item.keys ?? item.Keys,
        valueStr: item.valueStr ?? item.ValueStr,
        orderBy: toNumber(item.orderBy ?? item.OrderBy),
        raw: item,
      };
    })
    .filter((item): item is NormalizedCommonType => item !== null);
};
