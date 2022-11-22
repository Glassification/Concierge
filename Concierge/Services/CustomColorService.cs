// <copyright file="CustomColorService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Primitives;
    using Concierge.Utility.Utilities;
    using Newtonsoft.Json;

    public sealed class CustomColorService
    {
        public CustomColorService()
        {
            this.CustomColors = new List<CustomColor>();
            this.DefaultColors = new List<CustomColor>();
            this.DotNetColors = ColorUtility.ListDotNetColors().Select(x => new CustomColor(x)).ToList();
            this.RecentColors = new List<CustomColor>();
        }

        public List<CustomColor> CustomColors { get; set; }

        public List<CustomColor> DefaultColors { get; set; }

        [JsonIgnore]
        public List<CustomColor> DotNetColors { get; set; }

        public List<CustomColor> RecentColors { get; set; }

        public void AddRecentColor(CustomColor color)
        {
            if (this.RecentColors.Count == 0)
            {
                Program.Logger.Warning($"{nameof(this.RecentColors)} is empty.");
                return;
            }

            this.RecentColors.RemoveAt(this.RecentColors.Count - 1);
            this.RecentColors.Insert(0, color);
            this.Update();
        }

        public void AddCustomColor(CustomColor color)
        {
            this.CustomColors.Add(color);
            this.Update();
        }

        public void UpdateRecentColors(int index)
        {
            if (this.RecentColors.Count == 0)
            {
                Program.Logger.Warning($"{nameof(this.RecentColors)} is empty.");
                return;
            }

            var color = this.RecentColors[index];
            this.RecentColors.RemoveAt(index);
            this.RecentColors.Insert(0, color);
            this.Update();
        }

        private void Update()
        {
            CustomColorReadWriter.Write(Path.Combine(ConciergeFiles.GetCorrectCustomColorsPath(), ConciergeFiles.CustomColorsName), this);
        }
    }
}
