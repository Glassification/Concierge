﻿// <copyright file="EncumbranceCondition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses.ConditionStatus
{
    using Concierge;
    using Concierge.Character.Enums;
    using Concierge.Configuration;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public sealed class EncumbranceCondition : Condition, ICopyable<EncumbranceCondition>
    {
        public EncumbranceCondition()
            : base(string.Empty, string.Empty)
        {
        }

        public EncumbranceCondition(string description, string name)
            : base(description, name)
        {
            this.OverrideEncumbrance = false;
        }

        public override string Value => $"{this.EncumbranceLevel.ToString().FormatFromEnum()} - {this.Description}";

        public EncumbranceLevel EncumbranceLevel
        {
            get
            {
                if (this.OverrideEncumbrance)
                {
                    return this.EncumbranceLevelOverride;
                }

                var encumbrance = EncumbranceLevel.Normal;

                if (Program.CcsFile.Character.Armor.Strength > Program.CcsFile.Character.Attributes.Strength)
                {
                    encumbrance = EncumbranceLevel.Encumbered;
                }

                if (AppSettingsManager.UserSettings.UseEncumbrance)
                {
                    if (Program.CcsFile.Character.CarryWeight > Program.CcsFile.Character.LightCarryCapacity && Program.CcsFile.Character.CarryWeight <= Program.CcsFile.Character.MediumCarryCapacity)
                    {
                        encumbrance = EncumbranceLevel.Encumbered;
                    }
                    else if (Program.CcsFile.Character.CarryWeight > Program.CcsFile.Character.MediumCarryCapacity)
                    {
                        encumbrance = EncumbranceLevel.HeavilyEncumbered;
                    }
                }

                return encumbrance;
            }
        }

        public EncumbranceLevel EncumbranceLevelOverride { get; set; }

        public bool OverrideEncumbrance { get; set; }

        public override bool IsAfflicted()
        {
            return this.EncumbranceLevel != EncumbranceLevel.Normal;
        }

        public EncumbranceCondition DeepCopy()
        {
            return new EncumbranceCondition(this.Description, this.Name);
        }
    }
}