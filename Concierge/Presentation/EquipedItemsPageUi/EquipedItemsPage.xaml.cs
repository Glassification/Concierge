// <copyright file="EquipedItemsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.EquipedItemsPageUi
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Characters.Collections;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for EquipedItemsPage.xaml.
    /// </summary>
    public partial class EquipedItemsPage : Page
    {
        public EquipedItemsPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public double EquipedItemsHeight => SystemParameters.PrimaryScreenHeight - 100;

        public double EquipedItemsWidth => SystemParameters.PrimaryScreenWidth;

        public void Draw()
        {
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
