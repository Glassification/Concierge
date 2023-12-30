// <copyright file="ProficiencyWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for ProficiencyWindow.xaml.
    /// </summary>
    public partial class ProficiencyWindow : ConciergeWindow
    {
        public ProficiencyWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ProficiencyComboBox.ItemsSource = ComboBoxGenerator.ProficiencyTypesComboBox();
            this.ProficiencyTextComboBox.ItemsSource = GenerateComboBoxItems(ProficiencyTypes.Weapon);
            this.ConciergePage = ConciergePage.None;
            this.SelectedProficiencies = [];
            this.SelectedProficiency = new Proficiency();

            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.ProficiencyComboBox);
            this.SetMouseOverEvents(this.ProficiencyTextComboBox);
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Proficiency";

        public override string WindowName => nameof(ProficiencyWindow);

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private Proficiency SelectedProficiency { get; set; }

        private List<Proficiency> SelectedProficiencies { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.SelectedProficiencies = Program.CcsFile.Character.Characteristic.Proficiencies;
            this.CancelButton.Content = buttonText;

            this.SetEnabledState(true);
            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T proficiencies)
        {
            if (proficiencies is not List<Proficiency> castItem)
            {
                return false;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedProficiencies = castItem;
            this.ItemsAdded = false;

            this.SetEnabledState(true);
            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T proficiency)
        {
            if (proficiency is not Proficiency castItem)
            {
                return;
            }

            this.Editing = true;
            this.SelectedProficiency = castItem;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.SetEnabledState(false);
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateProficiency(this.SelectedProficiency);
            }
            else if (!this.ProficiencyComboBox.Text.IsNullOrWhiteSpace())
            {
                this.SelectedProficiencies.Add(this.ToProficiency());
            }

            this.CloseConciergeWindow();
        }

        private static CompositeCollection GenerateComboBoxItems(ProficiencyTypes proficiencyType)
        {
            var items = new CompositeCollection();
            switch (proficiencyType)
            {
                case ProficiencyTypes.Armor:
                    items.Add(new CollectionContainer() { Collection = ComboBoxGenerator.ArmorTypeComboBox() });
                    items.Add(new Separator());
                    items.Add(new ComboBoxItemControl(PackIconKind.Shield, Brushes.SteelBlue, "Shields"));
                    break;
                case ProficiencyTypes.Tool:
                    items.Add(new CollectionContainer() { Collection = CreateTools(Defaults.Tools, ConciergeBrushes.Mint, PackIconKind.Tools) });
                    items.Add(new Separator());
                    items.Add(new CollectionContainer() { Collection = CreateTools(Defaults.Games, ConciergeBrushes.Deer, PackIconKind.Cards) });
                    items.Add(new Separator());
                    items.Add(new CollectionContainer() { Collection = CreateTools(Defaults.Instruments, Brushes.IndianRed, PackIconKind.GuitarAcoustic) });
                    break;
                case ProficiencyTypes.Weapon:
                    items.Add(new ComboBoxItemControl(PackIconKind.Sword, Brushes.IndianRed, Proficiency.MartialMelee));
                    items.Add(new ComboBoxItemControl(PackIconKind.BowArrow, Brushes.Orange, Proficiency.MartialRanged));
                    items.Add(new ComboBoxItemControl(PackIconKind.Sword, Brushes.IndianRed, Proficiency.SimpleMelee));
                    items.Add(new ComboBoxItemControl(PackIconKind.BowArrow, Brushes.Orange, Proficiency.SimpleRanged));
                    items.Add(new Separator());
                    items.Add(new CollectionContainer() { Collection = ComboBoxGenerator.WeaponTypesComboBox() });
                    break;
            }

            var customItems = Program.CustomItemService.GetCustomItems<Proficiency>();
            if (!customItems.IsEmpty())
            {
                items.Insert(0, new Separator());
                foreach (var item in customItems)
                {
                    items.Insert(0, new ComboBoxItemControl(PackIconKind.ListStatus, Brushes.PowderBlue, item.Name));
                }
            }

            return items;
        }

        private static List<ComboBoxItemControl> CreateTools(ReadOnlyCollection<string> tools, Brush color, PackIconKind icon)
        {
            var comboBoxItems = new List<ComboBoxItemControl>();
            foreach (var tool in tools)
            {
                comboBoxItems.Add(new ComboBoxItemControl(icon, color, tool));
            }

            return comboBoxItems;
        }

        private void SetEnabledState(bool isEnabled)
        {
            DisplayUtility.SetControlEnableState(this.ProficiencyComboBox, isEnabled);
            DisplayUtility.SetControlEnableState(this.TypeLabel, isEnabled);
        }

        private void FillFields()
        {
            this.ProficiencyTextComboBox.Text = this.SelectedProficiency.Name;
            this.ProficiencyComboBox.Text = this.SelectedProficiency.ProficiencyType.ToString();
        }

        private Proficiency Create()
        {
            return new Proficiency()
            {
                Name = this.ProficiencyTextComboBox.Text,
                ProficiencyType = (ProficiencyTypes)Enum.Parse(typeof(ProficiencyTypes), this.ProficiencyComboBox.Text),
            };
        }

        private Proficiency ToProficiency()
        {
            this.ItemsAdded = true;
            var proficiency = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<Proficiency>(this.SelectedProficiencies, proficiency, this.ConciergePage));

            return proficiency;
        }

        private void UpdateProficiency(Proficiency proficiency)
        {
            var oldItem = proficiency.DeepCopy();

            proficiency.Name = this.ProficiencyTextComboBox.Text;
            proficiency.ProficiencyType = (ProficiencyTypes)Enum.Parse(typeof(ProficiencyTypes), this.ProficiencyComboBox.Text);

            if (!proficiency.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Proficiency>(proficiency, oldItem, this.ConciergePage));
            }
        }

        private void ClearFields()
        {
            this.ProficiencyTextComboBox.Text = string.Empty;
            this.ProficiencyComboBox.Text = ProficiencyTypes.Weapon.ToString();
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
            if (this.ProficiencyComboBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            this.SelectedProficiencies.Add(this.ToProficiency());
            this.ClearFields();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ProficiencyComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.Show(
                    "Could not save the Proficiency.\nA name is required before saving a custom item.",
                    "Warning",
                    ConciergeWindowButtons.Ok,
                    ConciergeWindowIcons.Alert);
                return;
            }

            Program.CustomItemService.AddCustomItem(this.Create());
            this.ClearFields();
            this.ProficiencyTextComboBox.ItemsSource = GenerateComboBoxItems(ProficiencyTypes.Weapon);
        }

        private void ProficiencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ConciergeComboBox comboBox && comboBox.SelectedItem is ComboBoxItemControl control && control.Tag is ProficiencyTypes proficiency)
            {
                this.ProficiencyTextComboBox.ItemsSource = GenerateComboBoxItems(proficiency);
            }
        }
    }
}
