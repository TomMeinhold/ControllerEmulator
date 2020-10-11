using LauncherAPI.Cryptography;
using System.IO;

namespace LauncherAPI.Extensions
{
    /// <summary>
    /// Static extension class for <see cref="FileInfo"/>.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Create a 512bit hash from an file.
        /// </summary>
        /// <param name="fileInfo">The file.</param>
        /// <returns>Hash 512bit.</returns>
        public static byte[] CreateHash(this FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return null;
            }

            using CRC64 crc64 = CRC64.Create();
            using var stream = fileInfo.OpenRead();
            byte[] hash = crc64.ComputeHash(stream);
            stream.Close();
            return hash;
        }
    }
}