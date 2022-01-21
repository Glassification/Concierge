// <copyright file="ModifyProficiencyWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyProficiencyWindow.xaml.
    /// </summary>
    public partial class ModifyProficiencyWindow : ConciergeWindow
    {
        public ModifyProficiencyWindow()
        {
            this.InitializeComponent();
            this.ProficiencyComboBox.ItemsSource = Enum.GetValues(typeof(ProficiencyTypes)).Cast<ProficiencyTypes>();
            this.ConciergePage = ConciergePage.None;
            this.SelectedProficiencies = new List<Proficiency>();
            this.SelectedProficiency = new Proficiency();
        }

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private Proficiency SelectedProficiency { get; set; }

        private List<Proficiency> SelectedProficiencies { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Proficiency";

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.SelectedProficiencies = Program.CcsFile.Character.Proficiency;
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

        private void SetEnabledState(bool isEnabled)
        {
            this.ProficiencyComboBox.IsEnabled = isEnabled;
            this.TypeLabel.IsEnabled = isEnabled;

            this.ProficiencyComboBox.Opacity = isEnabled ? 1 : 0.5;
            this.TypeLabel.Opacity = isEnabled ? 1 : 0.5;
        }

        private void FillFields()
        {
            this.ProficiencyTextBox.Text = this.SelectedProficiency.Name;
            this.ProficiencyComboBox.Text = this.SelectedProficiency.ProficiencyType.ToString();
        }

        private Proficiency ToProficiency()
        {
            this.ItemsAdded = true;

            var proficiency = new Proficiency()
            {
                Name = this.ProficiencyTextBox.Text,
                ProficiencyType = (ProficiencyTypes)Enum.Parse(typeof(ProficiencyTypes), this.ProficiencyComboBox.Text),
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Proficiency>(this.SelectedProficiencies, proficiency, this.ConciergePage));

            return proficiency;
        }

        private void UpdateProficiency(Proficiency proficiency)
        {
            var oldItem = proficiency.DeepCopy();

            proficiency.Name = this.ProficiencyTextBox.Text;
            proficiency.ProficiencyType = (ProficiencyTypes)Enum.Parse(typeof(ProficiencyTypes), this.ProficiencyComboBox.Text);

            Program.UndoRedoService.AddCommand(new EditCommand<Proficiency>(proficiency, oldItem, this.ConciergePage));
        }

        private void ClearFields()
        {
            this.ProficiencyTextBox.Text = string.Empty;
            this.ProficiencyComboBox.Text = string.Empty;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateProficiency(this.SelectedProficiency);
            }
            else
            {
                this.SelectedProficiencies.Add(this.ToProficiency());
            }

            this.HideConciergeWindow();

            Program.Modify();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedProficiencies.Add(this.ToProficiency());
            this.ClearFields();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
        }
    }
}
