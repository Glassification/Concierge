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

    /// <summary>
    /// Provides functionality to encode and decode images.
    /// </summary>
    public class ImageEncoding
    {
        private readonly IErrorService errorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEncoding"/> class with the specified error service.
        /// </summary>
        /// <param name="errorService">The error logging service.</param>
        public ImageEncoding(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        /// <summary>
        /// Decodes a base64-encoded string into a <see cref="BitmapImage"/>.
        /// </summary>
        /// <param name="base64String">The base64-encoded string representing the image.</param>
        /// <returns>A <see cref="BitmapImage"/> decoded from the base64 string, or null if decoding fails.</returns>
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

        /// <summary>
        /// Encodes an image file into a base64 string.
        /// </summary>
        /// <param name="filePath">The path to the image file.</param>
        /// <returns>The base64-encoded string representing the image, or an empty string if encoding fails.</returns>
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
