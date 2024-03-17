// <copyright file="CharacterCreationWizard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.Linq;

    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
    using Concierge.Display.Windows;
    using Concierge.Leveling;

    public sealed class CharacterCreationWizard
    {
        private bool isStopped;

        public CharacterCreationWizard()
        {
            this.isStopped = false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "The way she goes.")]
        public ConciergeResult Prompt()
        {
            return ConciergeMessageBox.Show(
                "This is the Concierge Character Creation Wizard. This will help jump start your path to godhood.",
                "Character Creation",
                ConciergeButtons.Ok | ConciergeButtons.Cancel,
                ConciergeIcons.Information);
        }

        public bool Start()
        {
            this.isStopped = false;

            this.RunSetupSteps();
            RemoveDuplicates();
            Program.UndoRedoService.Clear();

            return !this.isStopped;
        }

        public void Stop()
        {
            this.isStopped = true;
        }

        private static void RunDefinitions()
        {
            var character = Program.CcsFile.Character;
            character.Detail.Proficiencies.AddRange(LevelingMap.GetProficiencies(character.Disposition.Class1.Name, false));

            var saves = LevelingMap.GetSavingThrows(character.Disposition.Class1.Name);
            character.Attributes.Strength.Proficiency = saves.Strength;
            character.Attributes.Dexterity.Proficiency = saves.Dexterity;
            character.Attributes.Constitution.Proficiency = saves.Constitution;
            character.Attributes.Intelligence.Proficiency = saves.Intelligence;
            character.Attributes.Wisdom.Proficiency = saves.Wisdom;
            character.Attributes.Charisma.Proficiency = saves.Charisma;

            if (character.Disposition.Class2.Level > 0)
            {
                character.Detail.Proficiencies.AddRange(LevelingMap.GetProficiencies(character.Disposition.Class2.Name, true));
            }

            if (character.Disposition.Class3.Level > 0)
            {
                character.Detail.Proficiencies.AddRange(LevelingMap.GetProficiencies(character.Disposition.Class3.Name, true));
            }

            var senses = LevelingMap.GetRaceSenses(character.Disposition.Race);
            character.Detail.Senses.BaseMovement = senses.Movement;
            character.Detail.Senses.Vision = senses.VisionType;
        }

        private static void RemoveDuplicates()
        {
            var character = Program.CcsFile.Character;

            character.Detail.Proficiencies = character.Detail.Proficiencies.Distinct().ToList();
        }

        private void RunSetupSteps()
        {
            this.NextSetupStep(typeof(PropertiesWindow), "Skip Section");
            this.NextSetupStep(typeof(MagicClassWindow), "Continue");
            this.NextSetupStep(typeof(AttributesWindow), "Skip Section");
            this.NextSetupStep(typeof(LevelUpWindow), "Skip Section");
            RunDefinitions();
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
                        this.Stop();
                    }
                }
            }
            while (confirmExitResult == ConciergeResult.No);

            Program.Modify();
        }
    }
}
