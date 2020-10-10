namespace ControllerEmulatorAPI
{
    using System.IO;
    using System.Text.Json;

    public class Serializer
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { AllowTrailingCommas = true, WriteIndented = true };

        public static void Serialize<T>(FileInfo file, T t)
        {
            using var stream = file.Open(FileMode.Create);
            byte[] buffer = JsonSerializer.SerializeToUtf8Bytes(t, jsonSerializerOptions);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
        }

        public static T Deserialize<T>(FileInfo file)
        {
            using var stream = file.OpenRead();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            return JsonSerializer.Deserialize<T>(buffer);
        }
    }
}