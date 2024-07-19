// <copyright file="AugmentationWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Data;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for AugmentationWindow.xaml.
    /// </summary>
    public partial class AugmentationWindow : ConciergeWindow
    {
        private int oldQuantity;
        private int oldTotal;
        private CustomIcon customIcon = CustomIcon.Empty;

        public AugmentationWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
            this.NameComboBox.ItemsSource = DefaultItems;
            this.DamageTypeComboBox.ItemsSource = ComboBoxGenerator.DamageTypesComboBox();
            this.TypeComboBox.ItemsSource = ComboBoxGenerator.AugmentTypeComboBox();
            this.ConciergePage = ConciergePages.None;
            this.Augmentation = [];
            this.SelectedAugment = new Augment();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.NameComboBox);
            this.SetMouseOverEvents(this.QuantityUpDown);
            this.SetMouseOverEvents(this.UsedUpDown);
            this.SetMouseOverEvents(this.TypeComboBox);
            this.SetMouseOverEvents(this.RecoverableCheckBox);
            this.SetMouseOverEvents(this.BonusTextBox, this.BonusTextBackground);
            this.SetMouseOverEvents(this.DescriptionTextBox, this.DescriptionTextBackground);
            this.SetMouseOverEvents(this.DamageTypeComboBox);
            this.SetMouseOverEvents(this.EditIconButton);
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Augmentation";

        public override string WindowName => nameof(AugmentationWindow);

        public bool ItemsAdded { get; private set; }

        private static List<DetailedComboBoxItemControl> DefaultItems => ComboBoxGenerator.DetailedSelectorComboBox(Defaults.Augmentation, Program.CustomItemService.GetItems<Augment>());

        private bool Editing { get; set; }

        private Augment SelectedAugment { get; set; }

        private List<Augment> Augmentation { get; set; }

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Augmentation = Program.CcsFile.Character.Equipment.Augmentation;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T augmentation)
        {
            if (augmentation is not List<Augment> castItem)
            {
                return false;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Augmentation = castItem;
            this.ItemsAdded = false;

            this.SetQuantityState(false);
            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T augment)
        {
            if (augment is not Augment castItem)
            {
                return;
            }

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedAugment = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            if (this.Editing)
            {
                this.UpdateAmmunition(this.SelectedAugment);
            }
            else
            {
                this.Augmentation.Add(this.ToAugment());
            }

            this.CloseConciergeWindow();
        }

        private void FillFields(Augment augment)
        {
            Program.Drawing();
            this.SetQuantityState(augment.Recoverable);
            this.RecoverableCheckBox.UpdatingValue();

            this.NameComboBox.Text = augment.Name;
            this.QuantityUpDown.Value = this.oldQuantity = augment.Quantity;
            this.BonusTextBox.Text = augment.Damage;
            this.DamageTypeComboBox.Text = augment.DamageType.ToString();
            this.TypeComboBox.Text = augment.Type.ToString();
            this.UsedUpDown.Value = this.oldTotal = augment.Total;
            this.RecoverableCheckBox.IsChecked = augment.Recoverable;
            this.DescriptionTextBox.Text = augment.Description;
            this.SetIconFields(augment.Icon);

            this.UsedUpDown.Maximum = this.QuantityUpDown.Value;
            this.RecoverableCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();

            this.NameComboBox.Text = name;
            this.QuantityUpDown.Value = 0;
            this.BonusTextBox.Text = string.Empty;
            this.DamageTypeComboBox.Text = DamageTypes.None.ToString();
            this.TypeComboBox.Text = AugmentType.None.ToString();
            this.UsedUpDown.Value = 0;
            this.RecoverableCheckBox.IsChecked = false;
            this.DescriptionTextBox.Text = string.Empty;
            this.SetIconFields(CustomIcon.Empty);

            Program.NotDrawing();
        }

        private void SetIconFields(CustomIcon icon)
        {
            this.IconSymbol.Kind = icon.Kind;
            this.IconSymbol.Foreground = icon.Color.Brush;
            this.IconTextBox.Text = icon.Name;
            this.customIcon = icon;
        }

        private void UpdateAmmunition(Augment augment)
        {
            var oldItem = augment.DeepCopy();

            augment.Name = this.NameComboBox.Text;
            augment.Quantity = this.QuantityUpDown.Value;
            augment.Damage = this.BonusTextBox.Text;
            augment.DamageType = this.DamageTypeComboBox.Text.ToEnum<DamageTypes>();
            augment.Type = this.TypeComboBox.Text.ToEnum<AugmentType>();
            augment.Total = this.UsedUpDown.Value;
            augment.Recoverable = this.RecoverableCheckBox.IsChecked ?? false;
            augment.Description = this.DescriptionTextBox.Text;
            augment.Icon.Kind = this.IconSymbol.Kind;
            augment.Icon.Color = this.customIcon.Color;

            if (!augment.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Augment>(augment, oldItem, this.ConciergePage));
            }
        }

        private void SetQuantityState(bool state)
        {
            DisplayUtility.SetControlEnableState(this.UsedUpDown, state);
            DisplayUtility.SetControlEnableState(this.QuantityUpDown, state);
            DisplayUtility.SetControlEnableState(this.SlashLabel, state);
        }

        private Augment Create()
        {
            return new Augment()
            {
                Name = this.NameComboBox.Text,
                Quantity = this.QuantityUpDown.Value,
                Damage = this.BonusTextBox.Text,
                DamageType = this.DamageTypeComboBox.Text.ToEnum<DamageTypes>(),
                Type = this.TypeComboBox.Text.ToEnum<AugmentType>(),
                Total = this.UsedUpDown.Value,
                Recoverable = this.RecoverableCheckBox.IsChecked ?? false,
                Description = this.DescriptionTextBox.Text,
                Icon = new CustomIcon(this.customIcon.Color, this.IconSymbol.Kind),
            };
        }

        private Augment ToAugment()
        {
            this.ItemsAdded = true;
            var augment = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<Augment>(this.Augmentation, augment, this.ConciergePage));

            return augment;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Augmentation.Add(this.ToAugment());
            this.ClearFields();

            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isLocked = this.LockButton.IsChecked ?? false;
            if (this.NameComboBox.SelectedItem is DetailedComboBoxItemControl item && item.Item is Augment ammunition && !isLocked)
            {
                this.FillFields(ammunition);
            }
            else if (!isLocked)
            {
                this.ClearFields(this.NameComboBox.Text);
            }
        }

        private void QuantityUsedUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.UsedUpDown.Maximum = this.QuantityUpDown.Value;
            if (this.QuantityUpDown.Delta > 0 && this.QuantityUpDown.Value - 1 == this.UsedUpDown.Value)
            {
                this.UsedUpDown.Value = this.QuantityUpDown.Value;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.ShowWarning("Could not save the Augmentation.\nA name is required before saving a custom item.");
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
            this.ClearFields();
            this.NameComboBox.ItemsSource = DefaultItems;
        }

        private void RecoverableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.QuantityUpDown.Value = this.oldQuantity;
            this.UsedUpDown.Value = this.oldTotal;

            this.SetQuantityState(true);
        }

        private void RecoverableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.oldQuantity = this.QuantityUpDown.Value;
            this.oldTotal = this.UsedUpDown.Value;
            this.UsedUpDown.Value = 0;
            this.QuantityUpDown.Value = 0;

            this.SetQuantityState(false);
        }

        private void DamageTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ConciergeComboBox comboBox || comboBox.SelectedItem is not ComboBoxItemControl control)
            {
                return;
            }

            this.DamageLabel.Text = control.ToString().Equals("Healing") ? "Healing:" : "Damage:";
        }

        private void EditIconButton_Click(object sender, RoutedEventArgs e)
        {
            this.customIcon = ConciergeWindowService.ShowIconWindow(typeof(IconPickerWindow), this.customIcon);

            this.IconSymbol.Kind = this.customIcon.Kind;
            this.IconSymbol.Foreground = this.customIcon.Color.Brush;
            this.IconTextBox.Text = this.customIcon.Name;
        }

        private void LockButton_Checked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.Lock;
        }

        private void LockButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.LockOpenVariant;
        }
    }
}
