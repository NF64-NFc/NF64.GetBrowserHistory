using System;
using System.IO;
using System.Linq;

namespace NF64.WebBrowser
{
    public static class WebBrowserHistoryPath
    {
        public static readonly string GoogleChromePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"Google\Chrome\User Data\Default\History"
            );

        public static readonly string ChromiumEdgePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"Microsoft\Edge\User Data\Default\History"
            );


        public static readonly string FirefoxDirectoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"Mozilla\Firefox\Profiles\"
            );

        public static readonly string FirefoxHistoryFileName = "places.sqlite";

        public static string[] GetFirefoxHistoryFilePath()
            => GetFirefoxHistoryFilePath(FirefoxDirectoryPath);

        public static string[] GetFirefoxHistoryFilePath(string profilesDirectoryPath)
        {
            if (string.IsNullOrEmpty(profilesDirectoryPath))
                throw new ArgumentException($"{profilesDirectoryPath} is null or empty", nameof(profilesDirectoryPath));

            return Directory.GetDirectories(profilesDirectoryPath)
                .Select(dir => Path.Combine(dir, FirefoxHistoryFileName))
                .Where(path => File.Exists(path))
                .ToArray();
        }
    }
}
