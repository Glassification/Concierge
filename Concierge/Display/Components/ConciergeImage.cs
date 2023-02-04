// <copyright file="ConciergeImage.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Concierge.Utility;

    using Image = System.Windows.Controls.Image;

    public sealed class ConciergeImage : Image
    {
        private RenderTargetBitmap? renderTargetBitmap;

        public ConciergeImage()
        {
        }

        public void LoadFromByteArray(byte[] array)
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
            this.Source = bitmapImage;
        }

        public Color GetColorFromPoint(Point point)
        {
            if (this.renderTargetBitmap is null)
            {
                this.renderTargetBitmap = this.GeneratePointRender();
                this.renderTargetBitmap.Render(this);
            }

            if ((point.X <= this.renderTargetBitmap.PixelWidth) && (point.Y <= this.renderTargetBitmap.PixelHeight))
            {
                var croppedBitmap = new CroppedBitmap(
                    this.renderTargetBitmap,
                    new Int32Rect((int)point.X, (int)point.Y, 1, 1));

                var pixels = new byte[4];
                croppedBitmap.CopyPixels(pixels, 4, 0);

                return Color.FromArgb(255, pixels[2], pixels[1], pixels[0]);
            }

            return Colors.Transparent;
        }

        private RenderTargetBitmap GeneratePointRender()
        {
            return new RenderTargetBitmap(
                (int)this.ActualWidth,
                (int)this.ActualHeight,
                ResolutionScaling.Dpi,
                ResolutionScaling.Dpi,
                PixelFormats.Default);
        }
    }
}
