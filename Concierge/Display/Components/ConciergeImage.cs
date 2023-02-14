// <copyright file="ConciergeImage.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.IO;
    using System.Windows.Media.Imaging;

    using Image = System.Windows.Controls.Image;

    public sealed class ConciergeImage : Image
    {
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
    }
}
