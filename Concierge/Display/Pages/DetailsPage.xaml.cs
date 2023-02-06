// <copyright file="DetailsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Display.Enums;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Services;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for DetailsPage.xaml.
    /// </summary>
    public partial class DetailsPage : Page, Concierge.Interfaces.IConciergePage
    {
        public DetailsPage()
        {
            this.InitializeComponent();
        }

        public Interfaces.Enums.ConciergePage ConciergePage => Interfaces.Enums.ConciergePage.Details;

        public bool HasEditableDataGrid => true;

        public void Draw()
        {
            this.DrawAppearance();
            this.DrawPersonality();
            this.DrawArmor();
        }

        public void Edit(object itemToEdit)
        {
        }

        public void DrawAppearance()
        {
            this.AppearanceDisplay.SetAppearance(Program.CcsFile.Character.Appearance);
        }

        public void DrawPersonality()
        {
            this.PersonalityDisplay.SetPersonality(Program.CcsFile.Character.Personality);
        }

        public void DrawArmor()
        {
            this.ArmorDisplay.SetArmorDetails(Program.CcsFile.Character.Armor);
        }
    }
}
