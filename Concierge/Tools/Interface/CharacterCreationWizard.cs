// <copyright file="CharacterCreationWizard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Interface
{
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

    public class CharacterCreationWizard
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
            Program.UndoRedoService.Clear();

            ConciergeMessageBox.Show(
                $"Character creation {(this.IsStopped ? "aborted" : "completed successfully")}.",
                "Character Creation",
                ConciergeWindowButtons.Ok,
                ConciergeWindowIcons.Information);
        }

        public void Stop()
        {
            this.IsStopped = true;
            Program.CcsFile.Character = new ConciergeCharacter();
            Program.Unmodify();
        }

        private void RunSetupSteps()
        {
            this.NextSetupStep(new ModifyPropertiesWindow(), "Skip Section");
            this.NextSetupStep(new ModifyAttributesWindow(), "Skip Section");
            this.NextSetupStep(new ModifySensesWindow(), "Skip Section");
            this.NextSetupStep(new ModifyHealthWindow(), "Skip Section");
            this.NextSetupStep(new ModifyHitDiceWindow(), "Skip Section");
            this.NextSetupStep(new ModifyWealthWindow(), "Skip Section");
            this.NextSetupStep(new ModifyAppearanceWindow(), "Skip Section");
            this.NextSetupStep(new ModifyPersonalityWindow(), "Skip Section");
            this.NextSetupStep(new ModifyProficiencyWindow(), "Continue");
            this.NextSetupStep(new ModifyLanguagesWindow(), "Continue");
            this.NextSetupStep(new ModifyClassResourceWindow(), "Continue");
            this.NextSetupStep(new ModifyAbilitiesWindow(), "Continue");
            this.NextSetupStep(new ModifyArmorWindow(), "Skip Section");
            this.NextSetupStep(new ModifyAttackWindow(), "Continue");
            this.NextSetupStep(new ModifyAmmoWindow(), "Continue");
            this.NextSetupStep(new ModifyInventoryWindow(), "Continue");
            this.NextSetupStep(new ModifyEquippedItemsWindow(), "Continue");
            this.NextSetupStep(new ModifyStatusEffectsWindow(), "Continue");
            this.NextSetupStep(new ModifyCharacterImageWindow(), "Skip Section");
            this.NextSetupStep(new ModifySpellClassWindow(), "Continue");
            this.NextSetupStep(new ModifySpellSlotsWindow(), "Skip Section");
            this.NextSetupStep(new ModifySpellWindow(), "Continue");
        }

        private void NextSetupStep(ConciergeWindow conciergeWindow, string buttonText)
        {
            ConciergeWindowResult wizardResult;
            ConciergeWindowResult confirmExitResult;

            if (this.IsStopped)
            {
                return;
            }

            do
            {
                confirmExitResult = ConciergeWindowResult.NoResult;
                wizardResult = conciergeWindow.ShowWizardSetup(buttonText);

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
