using System;

namespace satguruApp.Service.Services
{
    public interface ILocationService
    {
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
        bool IsWithinRadius(double lat1, double lon1, double lat2, double lon2, double radiusKm);
        bool IsValidLatitude(decimal? latitude);
        bool IsValidLongitude(decimal? longitude);
        bool AreValidCoordinates(decimal? latitude, decimal? longitude);
    }

    public class LocationService : ILocationService
    {
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusKm = 6371.0;

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        public bool IsWithinRadius(double lat1, double lon1, double lat2, double lon2, double radiusKm)
        {
            return CalculateDistance(lat1, lon1, lat2, lon2) <= radiusKm;
        }

        public bool IsValidLatitude(decimal? latitude)
        {
            return latitude.HasValue && latitude.Value >= -90m && latitude.Value <= 90m;
        }

        public bool IsValidLongitude(decimal? longitude)
        {
            return longitude.HasValue && longitude.Value >= -180m && longitude.Value <= 180m;
        }

        public bool AreValidCoordinates(decimal? latitude, decimal? longitude)
        {
            return IsValidLatitude(latitude) && IsValidLongitude(longitude);
        }

        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
