// <copyright file="CharacterCreationWizard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Concierge.Interfaces;
    using Concierge.Interfaces.AbilitiesPageInterface;
    using Concierge.Interfaces.CompanionPageInterface;
    using Concierge.Interfaces.DetailsPageInterface;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.EquipmentPageInterface;
    using Concierge.Interfaces.EquippedItemsPageInterface;
    using Concierge.Interfaces.HelperInterface;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Interfaces.NotesPageInterface;
    using Concierge.Interfaces.OverviewPageInterface;
    using Concierge.Interfaces.SpellcastingPageInterface;

    public class CharacterCreationWizard
    {
        private readonly ModifyAttributesWindow modifyAttributesWindow = new ();
        private readonly Interfaces.ModifyPropertiesWindow modifyCharacterPropertiesWindow = new ();
        private readonly ModifyAppearanceWindow modifyAppearanceWindow = new ();

        public CharacterCreationWizard()
        {
            this.IsStopped = false;
        }

        private bool IsStopped { get; set; }

        public void Start()
        {
            this.IsStopped = false;

            this.NextSetupStep(this.modifyAttributesWindow);
        }

        public void Stop()
        {
            this.IsStopped = true;
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

                if (result == MessageWindowResult.No)
                {
                    this.Stop();
                }
            }
        }
    }
}
