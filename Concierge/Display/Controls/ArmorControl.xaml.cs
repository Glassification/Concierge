// <copyright file="ArmorControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Items;

    /// <summary>
    /// Interaction logic for ArmorControl.xaml.
    /// </summary>
    public partial class ArmorControl : UserControl
    {
        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(ArmorControl));

        public ArmorControl()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public void SetArmorDetails(Armor armor)
        {
            this.AcField.Text = armor.TotalArmorClass.ToString();
            this.ArmorWornField.Text = armor.Equiped;
            this.ArmorTypeField.Text = armor.Type.ToString();
            this.ArmorStealthField.Text = armor.Stealth.ToString();
            this.ShieldWornField.Text = armor.Shield;
            this.ShieldAcField.Text = armor.ShieldArmorClass.ToString();
            this.MiscAcField.Text = armor.MiscArmorClass.ToString();
            this.MagicAcField.Text = armor.MagicArmorClass.ToString();
        }

        private void EditDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
