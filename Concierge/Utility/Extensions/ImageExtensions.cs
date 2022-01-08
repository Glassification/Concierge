// <copyright file="ImageExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System.IO;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    public static class ImageExtensions
    {
        public static void LoadFromByteArray(this Image image, byte[] array)
        {
            if (array == null || array.Length == 0)
            {
                return;
            }

            var bitmapImage = new BitmapImage();
            using (var mem = new MemoryStream(array))
            {
                mem.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = null;
                bitmapImage.StreamSource = mem;
                bitmapImage.EndInit();
            }

            bitmapImage.Freeze();
            image.Source = bitmapImage;
        }
    }
}
