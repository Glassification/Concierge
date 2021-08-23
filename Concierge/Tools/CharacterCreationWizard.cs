// <copyright file="CharacterCreationWizard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using Concierge.Character;
    using Concierge.Interfaces;
    using Concierge.Interfaces.AbilitiesPageInterface;
    using Concierge.Interfaces.DetailsPageInterface;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.EquipmentPageInterface;
    using Concierge.Interfaces.EquippedItemsPageInterface;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Interfaces.OverviewPageInterface;
    using Concierge.Interfaces.SpellcastingPageInterface;

    public class CharacterCreationWizard
    {
        private readonly ModifyPropertiesWindow modifyCharacterPropertiesWindow = new ();
        private readonly ModifyAttributesWindow modifyAttributesWindow = new ();
        private readonly ModifySensesWindow modifySensesWindow = new ();
        private readonly ModifyHealthWindow modifyHealthWindow = new ();
        private readonly ModifyHitDiceWindow modifyHitDiceWindow = new ();
        private readonly ModifyWealthWindow modifyWealthWindow = new ();
        private readonly ModifyAppearanceWindow modifyAppearanceWindow = new ();
        private readonly ModifyPersonalityWindow modifyPersonalityWindow = new ();
        private readonly ModifyProficiencyWindow modifyProficiencyWindow = new ();
        private readonly ModifyLanguagesWindow modifyLanguagesWindow = new ();
        private readonly ModifyClassResourceWindow modifyClassResourceWindow = new ();
        private readonly ModifyArmorWindow modifyArmorWindow = new ();
        private readonly ModifyWeaponWindow modifyWeaponWindow = new ();
        private readonly ModifyAmmoWindow modifyAmmoWindow = new ();
        private readonly ModifyInventoryWindow modifyInventoryWindow = new ();
        private readonly ModifyEquippedItemsWindow modifyEquippedItemsWindow = new ();
        private readonly ModifySpellClassWindow modifySpellClassWindow = new ();
        private readonly ModifySpellSlotsWindow modifySpellSlotsWindow = new ();
        private readonly ModifySpellWindow modifySpellWindow = new ();
        private readonly ModifyAbilitiesWindow modifyAbilitiesWindow = new ();

        public CharacterCreationWizard()
        {
            this.IsStopped = false;
        }

        private bool IsStopped { get; set; }

        public void Start()
        {
            this.IsStopped = false;

            this.NextSetupStep(this.modifyCharacterPropertiesWindow);
            this.NextSetupStep(this.modifyAttributesWindow);
            this.NextSetupStep(this.modifySensesWindow);
            this.NextSetupStep(this.modifyHealthWindow);
            this.NextSetupStep(this.modifyHitDiceWindow);
            this.NextSetupStep(this.modifyWealthWindow);
            this.NextSetupStep(this.modifyAppearanceWindow);
            this.NextSetupStep(this.modifyPersonalityWindow);
            this.NextSetupStep(this.modifyProficiencyWindow);
            this.NextSetupStep(this.modifyLanguagesWindow);
            this.NextSetupStep(this.modifyClassResourceWindow);
            this.NextSetupStep(this.modifyAbilitiesWindow);
            this.NextSetupStep(this.modifyArmorWindow);
            this.NextSetupStep(this.modifyWeaponWindow);
            this.NextSetupStep(this.modifyAmmoWindow);
            this.NextSetupStep(this.modifyInventoryWindow);
            this.NextSetupStep(this.modifyEquippedItemsWindow);
            this.NextSetupStep(this.modifySpellClassWindow);
            this.NextSetupStep(this.modifySpellSlotsWindow);
            this.NextSetupStep(this.modifySpellWindow);
        }

        public void Stop()
        {
            this.IsStopped = true;
            Program.CcsFile.Character = new ConciergeCharacter();
        }

        private void NextSetupStep(IConciergeWindow conciergeWindow)
        {
            if (this.IsStopped)
            {
                return;
            }

            var result = conciergeWindow.ShowWizardSetup();

            if (result == MessageWindowResult.Exit)
            {
                result = Program.ConciergeMessageWindow.ShowWindow(
                            "Would you like to exit Character Creation?",
                            "Character Creation",
                            MessageWindowButtons.YesNo,
                            MessageWindowIcons.Question);

                if (result == MessageWindowResult.Yes)
                {
                    this.Stop();
                }
            }
        }
    }
}
