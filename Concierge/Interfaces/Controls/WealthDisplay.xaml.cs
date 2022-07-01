// <copyright file="WealthDisplay.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Statuses;

    /// <summary>
    /// Interaction logic for WealthDisplay.xaml.
    /// </summary>
    public partial class WealthDisplay : UserControl
    {
        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(WealthDisplay));

        public WealthDisplay()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public void SetWealth(Wealth wealth)
        {
            this.TotalWealthField.Text = Wealth.FormatGoldValue(wealth.TotalValue);
            this.ItemWealthField.Text = Wealth.FormatGoldValue(wealth.ItemValue);

            this.CopperField.Text = wealth.Copper.ToString();
            this.SilverField.Text = wealth.Silver.ToString();
            this.ElectrumField.Text = wealth.Electrum.ToString();
            this.GoldField.Text = wealth.Gold.ToString();
            this.PlatinumField.Text = wealth.Platinum.ToString();
        }

        private void EditWealthButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
