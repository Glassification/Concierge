// <copyright file="WealthControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;

    /// <summary>
    /// Interaction logic for WealthControl.xaml.
    /// </summary>
    public partial class WealthControl : UserControl
    {
        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(WealthControl));

        public WealthControl()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public CoinType SelectedCoin { get; private set; }

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

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            this.SelectedCoin = Wealth.GetCoinType(grid.Name);
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent, this));
        }
    }
}
