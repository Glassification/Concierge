// <copyright file="CustomColorService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Concierge.Common;
    using Concierge.Common.Utilities;
    using Concierge.Data;
    using Concierge.Persistence.ReadWriters;
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

            if (this.RecentColors.Any(x => x.Id.Equals(color.Id)))
            {
                return;
            }

            this.RecentColors.RemoveAt(this.RecentColors.Count - 1);
            this.RecentColors.Insert(0, color);
            this.Update();
        }

        public void AddCustomColor(CustomColor color)
        {
            this.CustomColors.Insert(0, color);
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

        public List<CustomColor> GetMissingColors(List<CustomColor> customColors)
        {
            var missingColors = new List<CustomColor>();
            foreach (var color in this.CustomColors)
            {
                if (!customColors.Any(x => x.Id.Equals(color.Id)))
                {
                    missingColors.Add(color);
                }
            }

            return missingColors;
        }

        private void Update()
        {
            CustomColorReadWriter.Write(Path.Combine(ConciergeFiles.GetCorrectCustomColorsPath(), ConciergeFiles.CustomColorsName), this);
        }
    }
}
