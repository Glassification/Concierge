// <copyright file="Portrait.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Windows.Media;

    using Concierge.Common;

    public sealed class Portrait : ICopyable<Portrait>
    {
        public Portrait()
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

        public Portrait DeepCopy()
        {
            return new Portrait()
            {
                Encoded = this.Encoded,
                Path = this.Path,
                Stretch = this.Stretch,
                UseCustomImage = this.UseCustomImage,
            };
        }
    }
}
