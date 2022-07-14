// <copyright file="CcsCompression.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    public static class CcsCompression
    {
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
