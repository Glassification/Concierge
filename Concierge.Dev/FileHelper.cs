// <copyright file="FileHelper.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools
{
    using System.IO;

    using Concierge.Persistence;

    public static class FileHelper
    {
        public static void Compress(string file)
        {
            var bytes = CcsCompression.Zip(File.ReadAllText(file));
            var path = Path.GetDirectoryName(file);
            var name = Path.GetFileNameWithoutExtension(file);
            var ext = Path.GetExtension(file);

            File.WriteAllBytes(@$"{path}\{name}_Compressed.{ext}", bytes);
        }

        public static void Decompress(string file)
        {
            var bytes = CcsCompression.Unzip(File.ReadAllBytes(file));
            var path = Path.GetDirectoryName(file);
            var name = Path.GetFileNameWithoutExtension(file);
            var ext = Path.GetExtension(file);

            File.WriteAllText(@$"{path}\{name}_Decompressed.{ext}", bytes);
        }
    }
}
