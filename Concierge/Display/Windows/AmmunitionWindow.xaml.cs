// <copyright file="AmmunitionWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for AmmunitionWindow.xaml.
    /// </summary>
    public partial class AmmunitionWindow : ConciergeWindow
    {
        public AmmunitionWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
            this.NameComboBox.ItemsSource = DefaultItems;
            this.DamageTypeComboBox.ItemsSource = ComboBoxGenerator.DamageTypesComboBox();
            this.CoinTypeComboBox.ItemsSource = ComboBoxGenerator.CoinTypesComboBox();
            this.ConciergePage = ConciergePage.None;
            this.Ammunition = [];
            this.SelectedAmmo = new Ammunition();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.NameComboBox);
            this.SetMouseOverEvents(this.QuantityUpDown);
            this.SetMouseOverEvents(this.UsedUpDown);
            this.SetMouseOverEvents(this.ValueUpDown);
            this.SetMouseOverEvents(this.CoinTypeComboBox);
            this.SetMouseOverEvents(this.RecoverableCheckBox);
            this.SetMouseOverEvents(this.BonusTextBox, this.BonusTextBackground);
            this.SetMouseOverEvents(this.DescriptionTextBox, this.DescriptionTextBackground);
            this.SetMouseOverEvents(this.DamageTypeComboBox);
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Ammunition";

        public override string WindowName => nameof(AmmunitionWindow);

        public bool ItemsAdded { get; private set; }

        private static List<ComboBoxItemControl> DefaultItems => ComboBoxGenerator.SelectorComboBox(Defaults.Ammunition, Program.CustomItemService.GetCustomItems<Ammunition>());

        private bool Editing { get; set; }

        private Ammunition SelectedAmmo { get; set; }

        private List<Ammunition> Ammunition { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Ammunition = Program.CcsFile.Character.Equipment.Ammunition;
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
            this.Ammunition = castItem;
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
                this.Ammunition.Add(this.ToAmmunition());
            }

            this.CloseConciergeWindow();
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
            this.RecoverableCheckBox.IsChecked = ammunition.Recoverable;
            this.DescriptionTextBox.Text = ammunition.Description;

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
            this.RecoverableCheckBox.IsChecked = false;
            this.DescriptionTextBox.Text = string.Empty;
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
            ammunition.Recoverable = this.RecoverableCheckBox.IsChecked ?? false;
            ammunition.Description = this.DescriptionTextBox.Text;

            if (!ammunition.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Ammunition>(ammunition, oldItem, this.ConciergePage));
            }
        }

        private Ammunition Create()
        {
            return new Ammunition()
            {
                Name = this.NameComboBox.Text,
                Quantity = this.QuantityUpDown.Value,
                Bonus = this.BonusTextBox.Text,
                DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text),
                Used = this.UsedUpDown.Value,
                Value = this.ValueUpDown.Value,
                CoinType = (CoinType)Enum.Parse(typeof(CoinType), this.CoinTypeComboBox.Text),
                Recoverable = this.RecoverableCheckBox.IsChecked ?? false,
                Description = this.DescriptionTextBox.Text,
            };
        }

        private Ammunition ToAmmunition()
        {
            this.ItemsAdded = true;
            var ammo = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<Ammunition>(this.Ammunition, ammo, this.ConciergePage));

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
            this.Ammunition.Add(this.ToAmmunition());
            this.ClearFields();

            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem is ComboBoxItemControl item && item.Item is Ammunition ammunition)
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            Program.CustomItemService.AddCustomItem(this.Create());
            this.ClearFields();
            this.NameComboBox.ItemsSource = DefaultItems;
        }
    }
}
