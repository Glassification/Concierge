// <copyright file="CharacterCreationWizard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
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

    public sealed class CharacterCreationWizard
    {
        public CharacterCreationWizard()
        {
            this.IsStopped = false;
        }

        private bool IsStopped { get; set; }

        public void Start()
        {
            this.IsStopped = false;

            var result = ConciergeMessageBox.Show(
                "This is the Concierge Character Creation Wizard. This will help jump start your path to godhood.",
                "Character Creation",
                ConciergeWindowButtons.OkCancel,
                ConciergeWindowIcons.Information);

            if (result != ConciergeWindowResult.OK)
            {
                this.Stop();
                return;
            }

            this.RunSetupSteps();
            RemoveDuplicates();
            Program.UndoRedoService.Clear();
        }

        public void Stop()
        {
            this.IsStopped = true;
            Program.CcsFile.Character = new ConciergeCharacter();
            Program.Unmodify();
        }

        private static void RunDefinitions()
        {
            var character = Program.CcsFile.Character;
            character.Characteristic.Proficiencies.AddRange(LevelingMap.GetProficiencies(character.Properties.Class1.Name, false));
            character.SavingThrows.SetProficiency(LevelingMap.GetSavingThrows(character.Properties.Class1.Name));

            if (character.Properties.Class2.Level > 0)
            {
                character.Characteristic.Proficiencies.AddRange(LevelingMap.GetProficiencies(character.Properties.Class2.Name, true));
            }

            if (character.Properties.Class3.Level > 0)
            {
                character.Characteristic.Proficiencies.AddRange(LevelingMap.GetProficiencies(character.Properties.Class3.Name, true));
            }

            var senses = LevelingMap.GetRaceSenses(character.Properties.Race);
            character.Characteristic.Senses.BaseMovement = senses.Movement;
            character.Characteristic.Senses.Vision = senses.VisionType;
        }

        private static void RemoveDuplicates()
        {
            var character = Program.CcsFile.Character;

            character.Characteristic.Proficiencies = character.Characteristic.Proficiencies.Distinct().ToList();
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
            this.NextSetupStep(typeof(AmmunitionWindow), "Continue");
            this.NextSetupStep(typeof(StatusEffectsWindow), "Continue");
            this.NextSetupStep(typeof(ImageWindow), "Skip Section");
            this.NextSetupStep(typeof(SpellSlotsWindow), "Skip Section");
            this.NextSetupStep(typeof(SpellWindow), "Continue");
        }

        private void NextSetupStep(Type conciergeWindowType, string buttonText)
        {
            ConciergeWindowResult wizardResult;
            ConciergeWindowResult confirmExitResult;

            if (this.IsStopped)
            {
                return;
            }

            do
            {
                var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(conciergeWindowType);
                confirmExitResult = ConciergeWindowResult.NoResult;
                wizardResult = conciergeWindow?.ShowWizardSetup(buttonText) ?? ConciergeWindowResult.NoResult;

                if (wizardResult == ConciergeWindowResult.Exit)
                {
                    confirmExitResult = ConciergeMessageBox.Show(
                        "Would you like to exit Character Creation? Existing progress will be lost.",
                        "Character Creation",
                        ConciergeWindowButtons.YesNo,
                        ConciergeWindowIcons.Question);

                    if (confirmExitResult is ConciergeWindowResult.Yes or ConciergeWindowResult.Exit)
                    {
                        this.Stop();
                    }
                }
            }
            while (confirmExitResult == ConciergeWindowResult.No);

            Program.Modify();
        }
    }
}
