// <copyright file="CharacterCreationWizard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Interface
{
    using Concierge.Character;
    using Concierge.Interfaces;
    using Concierge.Interfaces.AbilitiesPageInterface;
    using Concierge.Interfaces.AttackDefensePageInterface;
    using Concierge.Interfaces.DetailsPageInterface;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.EquippedItemsPageInterface;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Interfaces.OverviewPageInterface;
    using Concierge.Interfaces.SpellcastingPageInterface;

    public class CharacterCreationWizard
    {
        private readonly ModifyPropertiesWindow modifyCharacterPropertiesWindow = new ();
        private readonly ModifyAttributesWindow modifyAttributesWindow = new (ConciergePage.None);
        private readonly ModifySensesWindow modifySensesWindow = new (ConciergePage.None);
        private readonly ModifyHealthWindow modifyHealthWindow = new (ConciergePage.None);
        private readonly ModifyHitDiceWindow modifyHitDiceWindow = new (ConciergePage.None);
        private readonly ModifyWealthWindow modifyWealthWindow = new (ConciergePage.None);
        private readonly ModifyAppearanceWindow modifyAppearanceWindow = new (ConciergePage.None);
        private readonly ModifyPersonalityWindow modifyPersonalityWindow = new (ConciergePage.None);
        private readonly ModifyProficiencyWindow modifyProficiencyWindow = new (ConciergePage.None);
        private readonly ModifyLanguagesWindow modifyLanguagesWindow = new (ConciergePage.None);
        private readonly ModifyClassResourceWindow modifyClassResourceWindow = new (ConciergePage.None);
        private readonly ModifyArmorWindow modifyArmorWindow = new (ConciergePage.None);
        private readonly ModifyAttackWindow modifyAttackWindow = new (ConciergePage.None);
        private readonly ModifyAmmoWindow modifyAmmoWindow = new (ConciergePage.None);
        private readonly ModifyInventoryWindow modifyInventoryWindow = new (ConciergePage.None);
        private readonly ModifyEquippedItemsWindow modifyEquippedItemsWindow = new ();
        private readonly ModifySpellClassWindow modifySpellClassWindow = new (ConciergePage.None);
        private readonly ModifySpellSlotsWindow modifySpellSlotsWindow = new (ConciergePage.None);
        private readonly ModifySpellWindow modifySpellWindow = new (ConciergePage.None);
        private readonly ModifyAbilitiesWindow modifyAbilitiesWindow = new (ConciergePage.None);
        private readonly ModifyCharacterImageWindow modifyCharacterImageWindow = new ("768x1024 image ratio is recommended", ConciergePage.None);
        private readonly ModifyStatusEffectsWindow modifyStatusEffectsWindow = new (ConciergePage.None);

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
            this.NextSetupStep(this.modifyCharacterPropertiesWindow, "Skip Section");
            this.NextSetupStep(this.modifyAttributesWindow, "Skip Section");
            this.NextSetupStep(this.modifySensesWindow, "Skip Section");
            this.NextSetupStep(this.modifyHealthWindow, "Skip Section");
            this.NextSetupStep(this.modifyHitDiceWindow, "Skip Section");
            this.NextSetupStep(this.modifyWealthWindow, "Skip Section");
            this.NextSetupStep(this.modifyAppearanceWindow, "Skip Section");
            this.NextSetupStep(this.modifyPersonalityWindow, "Skip Section");
            this.NextSetupStep(this.modifyProficiencyWindow, "Continue");
            this.NextSetupStep(this.modifyLanguagesWindow, "Continue");
            this.NextSetupStep(this.modifyClassResourceWindow, "Continue");
            this.NextSetupStep(this.modifyAbilitiesWindow, "Continue");
            this.NextSetupStep(this.modifyArmorWindow, "Skip Section");
            this.NextSetupStep(this.modifyAttackWindow, "Continue");
            this.NextSetupStep(this.modifyAmmoWindow, "Continue");
            this.NextSetupStep(this.modifyInventoryWindow, "Continue");
            this.NextSetupStep(this.modifyEquippedItemsWindow, "Continue");
            this.NextSetupStep(this.modifyStatusEffectsWindow, "Continue");
            this.NextSetupStep(this.modifyCharacterImageWindow, "Skip Section");
            this.NextSetupStep(this.modifySpellClassWindow, "Continue");
            this.NextSetupStep(this.modifySpellSlotsWindow, "Skip Section");
            this.NextSetupStep(this.modifySpellWindow, "Continue");
        }

        private void NextSetupStep(IConciergeModifyWindow conciergeWindow, string buttonText)
        {
            ConciergeWindowResult wizardResult;
            ConciergeWindowResult confirmExitResult;

            if (this.IsStopped)
            {
                return;
            }

            Program.Modify();

            do
            {
                confirmExitResult = ConciergeWindowResult.NoResult;

                conciergeWindow.UpdateCancelButton(buttonText);
                wizardResult = conciergeWindow.ShowWizardSetup();

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
        }
    }
}
