// <copyright file="EncumbranceCondition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses.ConditionStatus
{
    using Concierge;
    using Concierge.Character.Enums;
    using Concierge.Configuration;
    using Concierge.Utility;

    public class EncumbranceCondition : Condition, ICopyable<EncumbranceCondition>
    {
        public EncumbranceCondition()
            : base(string.Empty, string.Empty)
        {
        }

        public EncumbranceCondition(string description, string name)
            : base(description, name)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Code style.")]
        public EncumbranceLevel EncumbranceLevel
        {
            get
            {
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

        public override string ToString()
        {
            return $"{this.EncumbranceLevel} - {this.Description}";
        }

        public EncumbranceCondition DeepCopy()
        {
            return new EncumbranceCondition(this.Description, this.Name);
        }

        public override bool IsAfflicted()
        {
            return this.EncumbranceLevel != EncumbranceLevel.Normal;
        }
    }
}
