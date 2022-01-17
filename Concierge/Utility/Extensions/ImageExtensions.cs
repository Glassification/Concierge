// <copyright file="ImageExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
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

        public static Color GetColorFromPoint(this Image image, Point point)
        {
            var renderTargetBitmap = new RenderTargetBitmap(
                (int)image.ActualWidth,
                (int)image.ActualHeight,
                96,
                96,
                PixelFormats.Default);

            renderTargetBitmap.Render(image);
            if ((point.X <= renderTargetBitmap.PixelWidth) && (point.Y <= renderTargetBitmap.PixelHeight))
            {
                var croppedBitmap = new CroppedBitmap(
                    renderTargetBitmap,
                    new Int32Rect((int)point.X, (int)point.Y, 1, 1));

                var pixels = new byte[4];
                croppedBitmap.CopyPixels(pixels, 4, 0);

                return Color.FromArgb(255, pixels[2], pixels[1], pixels[0]);
            }

            return Colors.Transparent;
        }
    }
}
