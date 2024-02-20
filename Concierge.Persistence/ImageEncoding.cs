// <copyright file="ImageEncoding.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.IO;
    using System.Windows.Media.Imaging;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    public class ImageEncoding
    {
        private readonly IErrorService errorService;

        public ImageEncoding(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        public BitmapImage? Decode(string base64String)
        {
            try
            {
                var binaryData = Convert.FromBase64String(base64String);
                var bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(binaryData);
                bitmapImage.EndInit();

                return bitmapImage;
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
                return null;
            }
        }

        public string Encode(string filePath)
        {
            if (filePath.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            try
            {
                var bytes = File.ReadAllBytes(filePath);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                this.errorService.LogError(ex);
                return string.Empty;
            }
        }
    }
}
