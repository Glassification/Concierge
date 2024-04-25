// <copyright file="CustomColorService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Data;
    using Concierge.Persistence.ReadWriters;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides methods for managing custom colors and recent color selections.
    /// </summary>
    public sealed class CustomColorService
    {
        private readonly CustomColorReadWriter readwriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomColorService"/> class.
        /// </summary>
        public CustomColorService()
        {
            this.readwriter = new CustomColorReadWriter(Program.ErrorService);

            this.CustomColors = [];
            this.DefaultColors = [];
            this.DotNetColors = ColorUtility.ListDotNetColors().Select(x => new CustomColor(x)).ToList();
            this.RecentColors = [];
        }

        /// <summary>
        /// Gets or sets the list of user created colours.
        /// </summary>
        public List<CustomColor> CustomColors { get; set; }

        /// <summary>
        /// Gets or sets the list of default colours.
        /// </summary>
        public List<CustomColor> DefaultColors { get; set; }

        /// <summary>
        /// Gets or sets the list of built int .NET colours.
        /// </summary>
        [JsonIgnore]
        public List<CustomColor> DotNetColors { get; set; }

        /// <summary>
        /// Gets or sets the list of recent colours.
        /// </summary>
        public List<CustomColor> RecentColors { get; set; }

        /// <summary>
        /// Adds a custom color to the list of recent colors.
        /// </summary>
        /// <param name="color">The custom color to add.</param>
        public void AddRecentColor(CustomColor color)
        {
            if (this.RecentColors.IsEmpty())
            {
                Program.Logger.Warning($"{nameof(this.RecentColors)} is empty.");
                return;
            }

            if (this.RecentColors.Any(x => x.Id.Equals(color.Id)))
            {
                return;
            }

            this.RecentColors.RemoveAt(this.RecentColors.Count - 1);
            this.RecentColors.Insert(0, color);
            this.Update();
        }

        /// <summary>
        /// Adds a custom color to the list of custom colors.
        /// </summary>
        /// <param name="color">The custom color to add.</param>
        public void AddCustomColor(CustomColor color)
        {
            this.CustomColors.Insert(0, color);
            this.Update();
        }

        /// <summary>
        /// Removes a custom color from the list of custom colors.
        /// </summary>
        /// <param name="color">The custom color to remove.</param>
        public void RemoveCustomColor(CustomColor color)
        {
            this.CustomColors.Remove(color);
            this.Update();
        }

        /// <summary>
        /// Updates the list of recent colors by moving the color at the specified index to the top of the list.
        /// </summary>
        /// <param name="index">The index of the color to move.</param>
        public void UpdateRecentColors(int index)
        {
            if (this.RecentColors.IsEmpty())
            {
                Program.Logger.Warning($"{nameof(this.RecentColors)} is empty.");
                return;
            }

            var color = this.RecentColors[index];
            this.RecentColors.RemoveAt(index);
            this.RecentColors.Insert(0, color);
            this.Update();
        }

        /// <summary>
        /// Updates the list of custom colors.
        /// </summary>
        public void UpdateCustomColors()
        {
            this.Update();
        }

        private void Update()
        {
            this.readwriter.WriteJson(Path.Combine(ConciergeFiles.GetCorrectCustomColorsPath(), ConciergeFiles.CustomColorsName), this);
        }
    }
}
