﻿// <copyright file="ModifyHpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.OverviewPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Status;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyHpWindow.xaml.
    /// </summary>
    public partial class ModifyHpWindow : Window
    {
        public ModifyHpWindow()
        {
            this.InitializeComponent();
        }

        private bool IsOk { get; set; }

        public void AddHP(Vitality vitality)
        {
            this.HeaderTextBlock.Text = "Add HP";

            this.ShowDialog();

            if (this.IsOk)
            {
                vitality.BaseHealth += this.HpUpDown.Value ?? 0;
                vitality.BaseHealth = Math.Min(vitality.BaseHealth, vitality.MaxHealth);
            }
        }

        public void SubtractHP(Vitality vitality)
        {
            this.HeaderTextBlock.Text = "Subract HP";

            this.ShowDialog();

            if (this.IsOk)
            {
                vitality.BaseHealth -= this.HpUpDown.Value ?? 0;
                vitality.BaseHealth = Math.Max(vitality.BaseHealth, 0);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.IsOk = false;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.IsOk = false;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.IsOk = false;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.TapNavigation();

            this.IsOk = true;
            this.Hide();
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}