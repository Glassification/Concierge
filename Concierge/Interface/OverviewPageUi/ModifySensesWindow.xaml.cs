﻿// <copyright file="ModifySensesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.OverviewPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifySensesWindow.xaml.
    /// </summary>
    public partial class ModifySensesWindow : Window
    {
        public ModifySensesWindow()
        {
            this.InitializeComponent();
            this.VisionComboBox.ItemsSource = Enum.GetValues(typeof(VisionTypes)).Cast<VisionTypes>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        public void EditSenses()
        {
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.InitiativeBonusUpDown.UpdatingValue();
            this.PerceptionBonusUpDown.UpdatingValue();
            this.BaseMovementUpDown.UpdatingValue();

            this.InitiativeTextBox.Text = Program.CcsFile.Character.Initiative.ToString();
            this.InitiativeBonusUpDown.Value = Program.CcsFile.Character.Details.InitiativeBonus;
            this.PerceptionTextBox.Text = Program.CcsFile.Character.PassivePerception.ToString();
            this.PerceptionBonusUpDown.Value = Program.CcsFile.Character.Details.PerceptionBonus;
            this.VisionComboBox.Text = Program.CcsFile.Character.Details.Vision.ToString();
            this.MovementTextBox.Text = Program.CcsFile.Character.Details.Movement.ToString();
            this.BaseMovementUpDown.Value = Program.CcsFile.Character.Details.BaseMovement;
        }

        private void UpdateSenses()
        {
            Program.CcsFile.Character.Details.InitiativeBonus = this.InitiativeBonusUpDown.Value ?? 0;
            Program.CcsFile.Character.Details.PerceptionBonus = this.PerceptionBonusUpDown.Value ?? 0;
            Program.CcsFile.Character.Details.Vision = (VisionTypes)Enum.Parse(typeof(VisionTypes), this.VisionComboBox.Text);
            Program.CcsFile.Character.Details.BaseMovement = this.BaseMovementUpDown.Value ?? 0;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.TapNavigation();

            this.UpdateSenses();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.TapNavigation();

            this.UpdateSenses();
            this.FillFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
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

        private void VisionComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ConciergeSound.UpdateValue();
        }
    }
}