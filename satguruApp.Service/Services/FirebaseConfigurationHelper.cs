using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace satguruApp.Service.Services
{
    internal static class FirebaseConfigurationHelper
    {
        public static string? ResolveServiceAccountPath(IConfiguration configuration)
        {
            var configuredPath = configuration["Firebase:ServiceAccountPath"];
            var envPath = Environment.GetEnvironmentVariable("FIREBASE_SERVICE_ACCOUNT_PATH");
            var serviceAccountPath = !string.IsNullOrWhiteSpace(configuredPath)
                ? configuredPath
                : envPath;

            if (string.IsNullOrWhiteSpace(serviceAccountPath))
            {
                return null;
            }

            if (Path.IsPathRooted(serviceAccountPath))
            {
                return File.Exists(serviceAccountPath) ? serviceAccountPath : null;
            }

            var currentDirectoryPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), serviceAccountPath));
            if (File.Exists(currentDirectoryPath))
            {
                return currentDirectoryPath;
            }

            var appBaseDirectoryPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, serviceAccountPath));
            if (File.Exists(appBaseDirectoryPath))
            {
                return appBaseDirectoryPath;
            }

            return null;
        }
    }
}
