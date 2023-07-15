// <copyright file="ProficiencyWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for ProficiencyWindow.xaml.
    /// </summary>
    public partial class ProficiencyWindow : ConciergeWindow
    {
        public ProficiencyWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ProficiencyComboBox.ItemsSource = Enum.GetValues(typeof(ProficiencyTypes)).Cast<ProficiencyTypes>();
            this.ProficiencyTextComboBox.ItemsSource = GenerateComboBoxItems();
            this.ConciergePage = ConciergePage.None;
            this.SelectedProficiencies = new List<Proficiency>();
            this.SelectedProficiency = new Proficiency();

            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.ProficiencyComboBox);
            this.SetFocusEvents(this.ProficiencyTextComboBox);
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

            Program.Modify();
        }

        private static CompositeCollection GenerateComboBoxItems()
        {
            return new CompositeCollection
            {
                Proficiency.MartialMelee,
                Proficiency.MartialRanged,
                Proficiency.SimpleMelee,
                Proficiency.SimpleRanged,
                new Separator(),
                new CollectionContainer() { Collection = Defaults.Weapons },
                new Separator(),
                ArmorType.Light.GetDescription(),
                ArmorType.Medium.GetDescription(),
                ArmorType.Heavy.GetDescription(),
                ArmorType.Massive.GetDescription(),
                new Separator(),
                new CollectionContainer() { Collection = Defaults.Tools },
                new Separator(),
                new CollectionContainer() { Collection = Defaults.Games },
                new Separator(),
                new CollectionContainer() { Collection = Defaults.Instruments },
            };
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

        private Proficiency ToProficiency()
        {
            this.ItemsAdded = true;

            var proficiency = new Proficiency()
            {
                Name = this.ProficiencyTextComboBox.Text,
                ProficiencyType = (ProficiencyTypes)Enum.Parse(typeof(ProficiencyTypes), this.ProficiencyComboBox.Text),
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Proficiency>(this.SelectedProficiencies, proficiency, this.ConciergePage));

            return proficiency;
        }

        private void UpdateProficiency(Proficiency proficiency)
        {
            var oldItem = proficiency.DeepCopy();

            proficiency.Name = this.ProficiencyTextComboBox.Text;
            proficiency.ProficiencyType = (ProficiencyTypes)Enum.Parse(typeof(ProficiencyTypes), this.ProficiencyComboBox.Text);

            Program.UndoRedoService.AddCommand(new EditCommand<Proficiency>(proficiency, oldItem, this.ConciergePage));
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

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
