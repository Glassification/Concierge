// <copyright file="ModifyPersonalityWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyPersonalityWindow.xaml.
    /// </summary>
    public partial class ModifyPersonalityWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyPersonalityWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.conciergePage = conciergePage;
        }

        private Personality Personality { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Personality = Program.CcsFile.Character.Personality;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public void ShowEdit(Personality personality)
        {
            this.Personality = personality;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowConciergeWindow();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void FillFields()
        {
            this.Trait1TextBox.Text = this.Personality.Trait1;
            this.Trait2TextBox.Text = this.Personality.Trait2;
            this.IdealTextBox.Text = this.Personality.Ideal;
            this.BondTextBox.Text = this.Personality.Bond;
            this.FlawTextBox.Text = this.Personality.Flaw;
            this.BackgroundTextBox.Text = this.Personality.Background;
            this.NotesTextBox.Text = this.Personality.Notes;
        }

        private void UpdatePersonality()
        {
            var oldItem = this.Personality.DeepCopy() as Personality;

            this.Personality.Trait1 = this.Trait1TextBox.Text;
            this.Personality.Trait2 = this.Trait2TextBox.Text;
            this.Personality.Ideal = this.IdealTextBox.Text;
            this.Personality.Bond = this.BondTextBox.Text;
            this.Personality.Flaw = this.FlawTextBox.Text;
            this.Personality.Background = this.BackgroundTextBox.Text;
            this.Personality.Notes = this.NotesTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Personality>(this.Personality, oldItem, this.conciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdatePersonality();
            this.HideConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdatePersonality();

            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
        }
    }
}
