// <copyright file="ConditionDescriptions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System.Collections.Generic;

    using Concierge.Character.Enums;

    /// <summary>
    /// Provides descriptions for different conditions.
    /// </summary>
    public static class ConditionDescriptions
    {
        public const string Encumbered = "A carry weight exceeding 5 and 10 times Strength will reduce speed by 10 and 20 respectively.";
        public const string Fatigued = "Exhaustion levels stack up to 6. A long rest reduces the level by 1.";

        private static readonly Dictionary<ConditionTypes, string> descriptions = new ()
        {
            { ConditionTypes.Blinded, "Automatically fail any ability checks. Attack rolls against you have advantage, your attacks have disadvantage." },
            { ConditionTypes.Charmed, "Automatically fail any ability checks. Attack rolls against you have advantage, your attacks have disadvantage." },
            { ConditionTypes.Dead, "You must succeed on 3 death saving throws, before failing 3. Succeeding will stabilize at 0 HP." },
            { ConditionTypes.Deafened, "You cannot hear and automatically fails any ability check that requires hearing." },
            { ConditionTypes.Frightened, "You have disadvantage on Ability Checks and Attack rolls while the source of fear is within line of sight. You can’t willingly move closer to the source." },
            { ConditionTypes.Grappled, "Your speed becomes 0. It ends when the grappler is incapacitated or you are thrown away." },
            { ConditionTypes.Incapacitated, "You Cannot take Actions or reactions." },
            { ConditionTypes.Invisible, "You are impossible to see without the aid of magic or a Special sense. Attacks against you have disadvantage, your attacks have advantage." },
            { ConditionTypes.Paralyzed, "You are incapacitated and automatically fail Strength and Dexterity Saving Throws. Attacks have advantage, and melee are auto crit." },
            { ConditionTypes.Petrified, "You are transformed into an inanimate substance and are incapacitated. Resistant to all damage and immune to poison and disease." },
            { ConditionTypes.Poisoned, "You have disadvantage on Attack rolls and Ability Checks." },
            { ConditionTypes.Prone, "Your only movement option is to crawl and have disadvantage on attacks. Melee attack is advantage, ranged is disadvantage." },
            { ConditionTypes.Restrained, "Your speed becomes 0. Your attacks have disadvantage, enemies have advantage. Dexterity Saving Throws are disadvantage." },
            { ConditionTypes.Stunned, "You are incapacitated and speak falteringly, and automatically fail Strength and Dexterity Saving Throws. Attacks against have advantage." },
            { ConditionTypes.Unconscious, "You are incapacitated, drop what you're holding, and fall prone. Attacks against have advantage and hits are auto crit." },
        };

        /// <summary>
        /// Retrieves the description associated with the specified condition type.
        /// </summary>
        /// <param name="condition">The condition type for which to retrieve the description.</param>
        /// <returns>
        /// The description associated with the specified condition type, or an empty string if the description is not found.
        /// </returns>
        public static string Get(ConditionTypes condition)
        {
            if (descriptions.TryGetValue(condition, out string? description))
            {
                return description ?? string.Empty;
            }

            return string.Empty;
        }
    }
}
