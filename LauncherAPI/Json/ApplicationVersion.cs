using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LauncherAPI.Json
{
    public class ApplicationVersion
    {
        [JsonPropertyName("url")] public Uri Url { get; set; }

        [JsonPropertyName("version")] public string Version { get; set; }

        [JsonPropertyName("hash")] public string Hash { get; set; }

        [JsonIgnore] public bool Installed { get; set; }

        public static ApplicationVersion GetLatestVersion()
        {
            using WebClient client = new WebClient();
            client.Headers.Add("Cache-Control", "no-cache");
            string json = client.DownloadString("https://raw.githubusercontent.com/TomMeinhold/ControllerEmulator/master/version.json");
            return JsonSerializer.Deserialize<ApplicationVersion>(json);
        }

        public static ApplicationVersion GetStoredVersion(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!file.Exists)
            {
                return new ApplicationVersion { Version = "0.0.0.0", Installed = false };
            }

            using FileStream fileStream = file.OpenRead();
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Close();
            ApplicationVersion version = JsonSerializer.Deserialize<ApplicationVersion>(Encoding.UTF8.GetString(buffer));
            version.Installed = true;
            return version;
        }

        public void StoreVersion(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
            using FileStream fileStream = file.Create();
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Close();
        }

        public bool UpdateRequired(ApplicationVersion version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            // Convert from 1.0.0.0 to 1000 (int)
            int ver1 = Convert.ToInt32(this.Version.Replace(".", "", StringComparison.CurrentCulture), CultureInfo.CurrentCulture);
            int ver2 = Convert.ToInt32(version.Version.Replace(".", "", StringComparison.CurrentCulture), CultureInfo.CurrentCulture);
            return ver1 < ver2;
        }
    }
}