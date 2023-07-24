// <copyright file="ArmorControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Equipable;

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

        public void SetDefenseDetails(Defense defense)
        {
            this.AcField.Text = defense.Armor.Ac.ToString();
            this.ArmorWornField.Text = defense.Armor.Name;
            this.ArmorTypeField.Text = defense.Armor.Type.ToString();
            this.ArmorStealthField.Text = defense.Armor.Stealth.ToString();
            this.ShieldWornField.Text = defense.Shield;
            this.ShieldAcField.Text = defense.ShieldAc.ToString();
            this.MiscAcField.Text = defense.MiscAc.ToString();
            this.MagicAcField.Text = defense.MagicAc.ToString();
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
