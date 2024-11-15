﻿// <copyright file="CharacterWizard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System;
    using System.Linq;

    using Concierge.Character;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
    using Concierge.Display.Windows;
    using Concierge.Leveling;

    /// <summary>
    /// Represents the Character Creation Wizard, guiding users through the process of creating a character.
    /// </summary>
    public sealed class CharacterWizard
    {
        private readonly CharacterSheet character;

        private bool isStopped;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterWizard"/> class.
        /// </summary>
        public CharacterWizard(CharacterSheet character)
        {
            this.character = character;
            this.isStopped = false;
        }

        /// <summary>
        /// Prompts the user with an initial message introducing the Character Creation Wizard.
        /// </summary>
        /// <returns>The result of the prompt.</returns>
        public static ConciergeResult Prompt()
        {
            return ConciergeMessageBox.Show(
                "This is the Concierge Character Creation Wizard. This will help jump start your path to godhood.",
                "Character Creation",
                ConciergeButtons.Ok | ConciergeButtons.Cancel,
                ConciergeIcons.Information);
        }

        /// <summary>
        /// Starts the character creation process.
        /// </summary>
        /// <returns><c>true</c> if the process is started successfully; otherwise, <c>false</c>.</returns>
        public bool StartCreation()
        {
            this.isStopped = false;

            this.RunSetupSteps();
            this.RemoveDuplicates();
            Program.UndoRedoService.Clear();

            return !this.isStopped;
        }

        /// <summary>
        /// Stops the character creation process.
        /// </summary>
        public void StopCreation()
        {
            this.isStopped = true;
        }

        private void RunDefinitions()
        {
            this.character.Detail.Proficiencies.AddRange(LevelingMap.GetProficiencies(this.character.Disposition.Class1.Name, false));

            var saves = LevelingMap.GetSavingThrows(this.character.Disposition.Class1.Name);
            this.character.Attributes.Strength.Proficiency = saves.Strength;
            this.character.Attributes.Dexterity.Proficiency = saves.Dexterity;
            this.character.Attributes.Constitution.Proficiency = saves.Constitution;
            this.character.Attributes.Intelligence.Proficiency = saves.Intelligence;
            this.character.Attributes.Wisdom.Proficiency = saves.Wisdom;
            this.character.Attributes.Charisma.Proficiency = saves.Charisma;

            if (this.character.Disposition.Class2.Level > 0)
            {
                this.character.Detail.Proficiencies.AddRange(LevelingMap.GetProficiencies(this.character.Disposition.Class2.Name, true));
            }

            if (this.character.Disposition.Class3.Level > 0)
            {
                this.character.Detail.Proficiencies.AddRange(LevelingMap.GetProficiencies(this.character.Disposition.Class3.Name, true));
            }

            var senses = LevelingMap.GetRaceSenses(this.character.Disposition.Race);
            this.character.Detail.Senses.BaseMovement = senses.Movement;
            this.character.Detail.Senses.Vision = senses.VisionType;
        }

        private void RemoveDuplicates()
        {
            this.character.Detail.Proficiencies = this.character.Detail.Proficiencies.Distinct().ToList();
        }

        private void RunSetupSteps()
        {
            this.NextSetupStep(typeof(PropertiesWindow), "Skip Section");
            this.NextSetupStep(typeof(MagicClassWindow), "Continue");
            this.NextSetupStep(typeof(AttributesWindow), "Skip Section");
            this.NextSetupStep(typeof(LevelUpWindow), "Skip Section");
            this.RunDefinitions();
            this.NextSetupStep(typeof(SensesWindow), "Skip Section");
            this.NextSetupStep(typeof(HealthWindow), "Skip Section");
            this.NextSetupStep(typeof(HitDiceWindow), "Skip Section");
            this.NextSetupStep(typeof(WealthWindow), "Skip Section");
            this.NextSetupStep(typeof(AppearanceWindow), "Skip Section");
            this.NextSetupStep(typeof(PersonalityWindow), "Skip Section");
            this.NextSetupStep(typeof(LanguagesWindow), "Continue");
            this.NextSetupStep(typeof(ClassResourceWindow), "Continue");
            this.NextSetupStep(typeof(AbilitiesWindow), "Continue");
            this.NextSetupStep(typeof(InventoryWindow), "Continue");
            this.NextSetupStep(typeof(AttacksWindow), "Continue");
            this.NextSetupStep(typeof(AugmentationWindow), "Continue");
            this.NextSetupStep(typeof(StatusEffectsWindow), "Continue");
            this.NextSetupStep(typeof(ImageWindow), "Skip Section");
            this.NextSetupStep(typeof(SpellSlotsWindow), "Skip Section");
            this.NextSetupStep(typeof(SpellWindow), "Continue");
        }

        private void NextSetupStep(Type conciergeWindowType, string buttonText)
        {
            ConciergeResult wizardResult;
            ConciergeResult confirmExitResult;

            if (this.isStopped)
            {
                return;
            }

            do
            {
                var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(conciergeWindowType);
                confirmExitResult = ConciergeResult.NoResult;
                wizardResult = conciergeWindow?.ShowWizardSetup(buttonText) ?? ConciergeResult.NoResult;

                if (wizardResult == ConciergeResult.Exit)
                {
                    confirmExitResult = ConciergeMessageBox.Show(
                        "Would you like to exit Character Creation? Existing progress will be lost.",
                        "Character Creation",
                        ConciergeButtons.Yes | ConciergeButtons.No,
                        ConciergeIcons.Question);

                    if (confirmExitResult is ConciergeResult.Yes or ConciergeResult.Exit)
                    {
                        this.StopCreation();
                    }
                }
            }
            while (confirmExitResult == ConciergeResult.No);

            Program.Modify();
        }
    }
}
