using System;

namespace LauncherAPI.Humanize
{
    public static class Bytes
    {
        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string SizeSuffix(this double value)
        {
            int order = 0;
            while (value >= 1024 && order < SizeSuffixes.Length - 1)
            {
                order++;
                value /= 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would show a
            // single decimal place, and no space.
            return $"{Math.Round(value, 2)} {SizeSuffixes[order]}";
        }

        public static string SpeedSuffix(this double value)
        {
            return value.SizeSuffix() + "/s";
        }
    }
}