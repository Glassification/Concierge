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

    public sealed class CustomColorService
    {
        private readonly IReadWriters readwriter;

        public CustomColorService()
        {
            this.readwriter = new CustomColorReadWriter(Program.ErrorService);

            this.CustomColors = [];
            this.DefaultColors = [];
            this.DotNetColors = ColorUtility.ListDotNetColors().Select(x => new CustomColor(x)).ToList();
            this.RecentColors = [];
        }

        public List<CustomColor> CustomColors { get; set; }

        public List<CustomColor> DefaultColors { get; set; }

        [JsonIgnore]
        public List<CustomColor> DotNetColors { get; set; }

        public List<CustomColor> RecentColors { get; set; }

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

        public void AddCustomColor(CustomColor color)
        {
            this.CustomColors.Insert(0, color);
            this.Update();
        }

        public void RemoveCustomColor(CustomColor color)
        {
            this.CustomColors.Remove(color);
            this.Update();
        }

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
