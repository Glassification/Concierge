// <copyright file="CharacterCreationWizard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Interface
{
    using System;
    using System.Linq;

    using Concierge.Character;
    using Concierge.Interfaces;
    using Concierge.Interfaces.AbilitiesPageInterface;
    using Concierge.Interfaces.AttackDefensePageInterface;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.DetailsPageInterface;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.EquippedItemsPageInterface;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Interfaces.OverviewPageInterface;
    using Concierge.Interfaces.SpellcastingPageInterface;
    using Concierge.Utility.Utilities;

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
            RunDefinitions();
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

            if (character.Properties.Class1.Level > 0)
            {
                character.Proficiencies.AddRange(CharacterUtility.GetProficiencies(character.Properties.Class1.Name, false));
                character.SavingThrow.SetProficiency(CharacterUtility.GetSavingThrows(character.Properties.Class1.Name));
            }

            if (character.Properties.Class2.Level > 0)
            {
                character.Proficiencies.AddRange(CharacterUtility.GetProficiencies(character.Properties.Class2.Name, true));
            }

            if (character.Properties.Class3.Level > 0)
            {
                character.Proficiencies.AddRange(CharacterUtility.GetProficiencies(character.Properties.Class3.Name, true));
            }

            character.Proficiencies = character.Proficiencies.Distinct().ToList();
        }

        private void RunSetupSteps()
        {
            this.NextSetupStep(typeof(ModifyPropertiesWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifyAttributesWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifySensesWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifyHealthWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifyHitDiceWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifyWealthWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifyAppearanceWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifyPersonalityWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifyLanguagesWindow), "Continue");
            this.NextSetupStep(typeof(ModifyClassResourceWindow), "Continue");
            this.NextSetupStep(typeof(ModifyAbilitiesWindow), "Continue");
            this.NextSetupStep(typeof(ModifyInventoryWindow), "Continue");
            this.NextSetupStep(typeof(ModifyAttackWindow), "Continue");
            this.NextSetupStep(typeof(ModifyAmmoWindow), "Continue");
            this.NextSetupStep(typeof(ModifyStatusEffectsWindow), "Continue");
            this.NextSetupStep(typeof(ModifyCharacterImageWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifySpellClassWindow), "Continue");
            this.NextSetupStep(typeof(ModifySpellSlotsWindow), "Skip Section");
            this.NextSetupStep(typeof(ModifySpellWindow), "Continue");
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
