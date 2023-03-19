// <copyright file="AmmunitionWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for AmmunitionWindow.xaml.
    /// </summary>
    public partial class AmmunitionWindow : ConciergeWindow
    {
        public AmmunitionWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = Constants.Ammunitions;
            this.DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(DamageTypes)).Cast<DamageTypes>();
            this.CoinTypeComboBox.ItemsSource = Enum.GetValues(typeof(CoinType)).Cast<CoinType>();
            this.ConciergePage = ConciergePage.None;
            this.Ammunitions = new List<Ammunition>();
            this.SelectedAmmo = new Ammunition();
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Ammunition";

        public override string WindowName => nameof(AmmunitionWindow);

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private Ammunition SelectedAmmo { get; set; }

        private List<Ammunition> Ammunitions { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Ammunitions = Program.CcsFile.Character.Ammunitions;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T ammunitions)
        {
            if (ammunitions is not List<Ammunition> castItem)
            {
                return false;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Ammunitions = castItem;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T ammunition)
        {
            if (ammunition is not Ammunition castItem)
            {
                return;
            }

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedAmmo = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateAmmunition(this.SelectedAmmo);
            }
            else
            {
                this.Ammunitions.Add(this.ToAmmunition());
            }

            this.CloseConciergeWindow();
            Program.Modify();
        }

        private void FillFields(Ammunition ammunition)
        {
            this.NameComboBox.Text = ammunition.Name;
            this.QuantityUpDown.Value = ammunition.Quantity;
            this.BonusTextBox.Text = ammunition.Bonus;
            this.DamageTypeComboBox.Text = ammunition.DamageType.ToString();
            this.UsedUpDown.Value = ammunition.Used;
            this.ValueUpDown.Value = ammunition.Value;
            this.CoinTypeComboBox.Text = ammunition.CoinType.ToString();

            this.UsedUpDown.Maximum = this.QuantityUpDown.Value;
        }

        private void ClearFields(string name = "")
        {
            this.NameComboBox.Text = name;
            this.QuantityUpDown.Value = 0;
            this.BonusTextBox.Text = string.Empty;
            this.DamageTypeComboBox.Text = DamageTypes.None.ToString();
            this.UsedUpDown.Value = 0;
            this.ValueUpDown.Value = 0;
            this.CoinTypeComboBox.Text = CoinType.Copper.ToString();
        }

        private void UpdateAmmunition(Ammunition ammunition)
        {
            var oldItem = ammunition.DeepCopy();

            ammunition.Name = this.NameComboBox.Text;
            ammunition.Quantity = this.QuantityUpDown.Value;
            ammunition.Bonus = this.BonusTextBox.Text;
            ammunition.DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text);
            ammunition.Used = this.UsedUpDown.Value;
            ammunition.Value = this.ValueUpDown.Value;
            ammunition.CoinType = (CoinType)Enum.Parse(typeof(CoinType), this.CoinTypeComboBox.Text);

            Program.UndoRedoService.AddCommand(new EditCommand<Ammunition>(ammunition, oldItem, this.ConciergePage));
        }

        private Ammunition ToAmmunition()
        {
            this.ItemsAdded = true;

            var ammo = new Ammunition()
            {
                Name = this.NameComboBox.Text,
                Quantity = this.QuantityUpDown.Value,
                Bonus = this.BonusTextBox.Text,
                DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text),
                Used = this.UsedUpDown.Value,
                Value = this.ValueUpDown.Value,
                CoinType = (CoinType)Enum.Parse(typeof(CoinType), this.CoinTypeComboBox.Text),
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Ammunition>(this.Ammunitions, ammo, this.ConciergePage));

            return ammo;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Ammunitions.Add(this.ToAmmunition());
            this.ClearFields();

            this.InvokeApplyChanges();
            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem is Ammunition ammunition)
            {
                this.FillFields(ammunition);
            }
            else
            {
                this.ClearFields(this.NameComboBox.Text);
            }
        }

        private void QuantityUsedUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.UsedUpDown.Maximum = this.QuantityUpDown.Value;
        }
    }
}
