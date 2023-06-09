// <copyright file="CcsCompression.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    /// <summary>
    /// Provides methods for compressing and decompressing data using GZip compression.
    /// </summary>
    public static class CcsCompression
    {
        /// <summary>
        /// Compresses a file into a byte array using GZip compression.
        /// </summary>
        /// <param name="file">The file to compress.</param>
        /// <returns>A byte array representing the compressed data.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "Cleaner Code.")]
        public static byte[] Zip(string file)
        {
            var bytes = Encoding.UTF8.GetBytes(file);

            using (var inputStream = new MemoryStream(bytes))
            using (var outputStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    inputStream.CopyTo(zipStream);
                }

                return outputStream.ToArray();
            }
        }

        /// <summary>
        /// Decompresses a byte array into a string using GZip decompression.
        /// </summary>
        /// <param name="bytes">The byte array representing the compressed data.</param>
        /// <returns>A string representing the decompressed data.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "Cleaner Code.")]
        public static string Unzip(byte[] bytes)
        {
            using (var inputStream = new MemoryStream(bytes))
            using (var outputStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    zipStream.CopyTo(outputStream);
                }

                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }
    }
}
