// <copyright file="CharacterImage.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Concierge;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class CharacterImage : ICopyable<CharacterImage>
    {
        public CharacterImage()
        {
            this.Encoded = string.Empty;
            this.Path = string.Empty;
            this.Stretch = Stretch.UniformToFill;
            this.UseCustomImage = false;
        }

        public string Encoded { get; set; }

        public string Path { get; set; }

        public Stretch Stretch { get; set; }

        public bool UseCustomImage { get; set; }

        public BitmapImage? ToImage()
        {
            if (!this.UseCustomImage)
            {
                return null;
            }

            try
            {
                var binaryData = Convert.FromBase64String(this.Encoded);
                var bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(binaryData);
                bitmapImage.EndInit();

                return bitmapImage;
            }
            catch (NotSupportedException ex)
            {
                Program.Logger.Warning(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
                return null;
            }
        }

        public void EncodeImage(string filePath)
        {
            if (filePath.IsNullOrWhiteSpace())
            {
                this.Path = string.Empty;
                this.Encoded = string.Empty;

                return;
            }

            try
            {
                var bytes = File.ReadAllBytes(filePath);

                this.Path = filePath;
                this.Encoded = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                this.Path = string.Empty;
                this.Encoded = string.Empty;

                Program.ErrorService.LogError(ex);
            }
        }

        public CharacterImage DeepCopy()
        {
            return new CharacterImage()
            {
                Encoded = this.Encoded,
                Path = this.Path,
                Stretch = this.Stretch,
                UseCustomImage = this.UseCustomImage,
            };
        }
    }
}
