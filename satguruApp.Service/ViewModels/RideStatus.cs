using System;
using System.Collections.Generic;

namespace satguruApp.Service.ViewModels
{
    public static class RideStatus
    {
        public const int RequestForRide = 1;
        public const int DriverAssigned = 2;
        public const int DriverArriving = 3;
        public const int RideStarted = 4;
        public const int RideCompleted = 5;
        public const int Cancelled = 6;

        private static readonly Dictionary<string, int> NameToCode = new(StringComparer.OrdinalIgnoreCase)
        {
            ["request_for_ride"] = RequestForRide,
            ["driver_assigned"] = DriverAssigned,
            ["driver_arriving"] = DriverArriving,
            ["ride_started"] = RideStarted,
            ["ride_completed"] = RideCompleted,
            ["cancelled"] = Cancelled,
        };

        private static readonly Dictionary<int, string> CodeToName = new()
        {
            [RequestForRide] = "request_for_ride",
            [DriverAssigned] = "driver_assigned",
            [DriverArriving] = "driver_arriving",
            [RideStarted] = "ride_started",
            [RideCompleted] = "ride_completed",
            [Cancelled] = "cancelled",
        };

        public static bool TryParse(string? status, out int code)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                code = 0;
                return false;
            }

            return NameToCode.TryGetValue(status.Trim(), out code);
        }

        public static string ToName(int? code)
        {
            if (!code.HasValue)
            {
                return string.Empty;
            }

            return CodeToName.TryGetValue(code.Value, out var name) ? name : string.Empty;
        }

        public static bool CanTransition(int? fromCode, int toCode)
        {
            if (!fromCode.HasValue)
            {
                return toCode == RequestForRide;
            }

            if (fromCode.Value == RideCompleted || fromCode.Value == Cancelled)
            {
                return false;
            }

            if (toCode == Cancelled)
            {
                return true;
            }

            return fromCode.Value switch
            {
                RequestForRide => toCode == DriverAssigned,
                DriverAssigned => toCode == DriverArriving,
                DriverArriving => toCode == RideStarted,
                RideStarted => toCode == RideCompleted,
                _ => false,
            };
        }
    }
}
