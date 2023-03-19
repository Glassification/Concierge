// <copyright file="ArmorWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Utility.Units;

    /// <summary>
    /// Interaction logic for ArmorWindow.xaml.
    /// </summary>
    public partial class ArmorWindow : ConciergeWindow
    {
        public ArmorWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(ArmorType)).Cast<ArmorType>();
            this.StealthComboBox.ItemsSource = Enum.GetValues(typeof(ArmorStealth)).Cast<ArmorStealth>();
            this.ConciergePage = ConciergePage.None;
            this.SelectedArmor = new Armor();
        }

        public override string HeaderText => "Edit Armor";

        public override string WindowName => nameof(ArmorWindow);

        private Armor SelectedArmor { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.SelectedArmor = Program.CcsFile.Character.Armor;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T armor)
        {
            if (armor is not Armor castItem)
            {
                return;
            }

            this.SelectedArmor = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateArmor(this.SelectedArmor);
            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields()
        {
            this.EquipedTextBox.Text = this.SelectedArmor.Equiped;
            this.TypeComboBox.Text = this.SelectedArmor.Type.ToString();
            this.ArmorClassUpDown.Value = this.SelectedArmor.ArmorClass;
            this.WeightUpDown.Value = this.SelectedArmor.Weight.Value;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.StrengthUpDown.Value = this.SelectedArmor.Strength;
            this.StealthComboBox.Text = this.SelectedArmor.Stealth.ToString();

            this.ShieldTextBox.Text = this.SelectedArmor.Shield;
            this.ShieldArmorClassUpDown.Value = this.SelectedArmor.ShieldArmorClass;
            this.ShieldWeightUpDown.Value = this.SelectedArmor.ShieldWeight.Value;
            this.ShieldWeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.MiscArmorClassUpDown.Value = this.SelectedArmor.MiscArmorClass;
            this.MagicArmorClassUpDown.Value = this.SelectedArmor.MagicArmorClass;
        }

        private void UpdateArmor(Armor armor)
        {
            var oldItem = armor.DeepCopy();

            armor.Equiped = this.EquipedTextBox.Text;
            armor.Type = (ArmorType)Enum.Parse(typeof(ArmorType), this.TypeComboBox.Text);
            armor.ArmorClass = this.ArmorClassUpDown.Value;
            armor.Weight.Value = this.WeightUpDown.Value;
            armor.Strength = this.StrengthUpDown.Value;
            armor.Stealth = (ArmorStealth)Enum.Parse(typeof(ArmorStealth), this.StealthComboBox.Text);
            armor.Shield = this.ShieldTextBox.Text;
            armor.ShieldArmorClass = this.ShieldArmorClassUpDown.Value;
            armor.ShieldWeight.Value = this.ShieldWeightUpDown.Value;
            armor.MiscArmorClass = this.MiscArmorClassUpDown.Value;
            armor.MagicArmorClass = this.MagicArmorClassUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<Armor>(armor, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateArmor(this.SelectedArmor);
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
