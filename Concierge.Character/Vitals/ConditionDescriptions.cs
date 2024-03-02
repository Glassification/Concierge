// <copyright file="ConditionDescriptions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    /// <summary>
    /// Provides descriptions for different conditions.
    /// </summary>
    public static class ConditionDescriptions
    {
        public const string Blinded = "Automatically fail any ability checks. Attack rolls against you have advantage, your attacks have disadvantage.";
        public const string Charmed = "You cannot attack the charmer. The charmer has advantage on ability checks when interacting socially.";
        public const string Death = "You must succeed on 3 death saving throws, before failing 3. Succeeding will stabilize at 0 HP.";
        public const string Deafened = "You cannot hear and automatically fails any ability check that requires hearing.";
        public const string Encumbered = "A carry weight exceeding 5 and 10 times Strength will reduce speed by 10 and 20 respectively.";
        public const string Fatigued = "Exhaustion levels stack up to 6. A long rest reduces the level by 1.";
        public const string Frightened = "You have disadvantage on Ability Checks and Attack rolls while the source of fear is within line of sight. You can’t willingly move closer to the source.";
        public const string Grappled = "Your speed becomes 0. It ends when the grappler is incapacitated or you are thrown away.";
        public const string Incapacitated = "You Cannot take Actions or reactions.";
        public const string Invisible = "You are impossible to see without the aid of magic or a Special sense. Attacks against you have disadvantage, your attacks have advantage.";
        public const string Paralyzed = "You are incapacitated and automatically fail Strength and Dexterity Saving Throws. Attacks have advantage, and melee are auto crit.";
        public const string Petrified = "You are transformed into an inanimate substance and are incapacitated. Resistant to all damage and immune to poison and disease.";
        public const string Poisoned = "You have disadvantage on Attack rolls and Ability Checks";
        public const string Prone = "Your only movement option is to crawl and have disadvantage on attacks. Melee attack is advantage, ranged is disadvantage.";
        public const string Restrained = "Your speed becomes 0. Your attacks have disadvantage, enemies have advantage. Dexterity Saving Throws are disadvantage.";
        public const string Stunned = "You are incapacitated and speak falteringly, and automatically fail Strength and Dexterity Saving Throws. Attacks against have advantage.";
        public const string Unconscious = "You are incapacitated, drop what you're holding, and fall prone. Attacks against have advantage and hits are auto crit.";
    }
}
