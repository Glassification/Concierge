// <copyright file="ModifyPersonalityWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyPersonalityWindow.xaml.
    /// </summary>
    public partial class ModifyPersonalityWindow : ConciergeWindow
    {
        public ModifyPersonalityWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.Personality = new Personality();
        }

        public override string HeaderText => "Edit Personality";

        private Personality Personality { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Personality = Program.CcsFile.Character.Personality;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T personality)
        {
            if (personality is not Personality castItem)
            {
                return;
            }

            this.Personality = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdatePersonality();
            this.CloseConciergeWindow();

            Program.Modify();
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
            var oldItem = this.Personality.DeepCopy();

            this.Personality.Trait1 = this.Trait1TextBox.Text;
            this.Personality.Trait2 = this.Trait2TextBox.Text;
            this.Personality.Ideal = this.IdealTextBox.Text;
            this.Personality.Bond = this.BondTextBox.Text;
            this.Personality.Flaw = this.FlawTextBox.Text;
            this.Personality.Background = this.BackgroundTextBox.Text;
            this.Personality.Notes = this.NotesTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Personality>(this.Personality, oldItem, this.ConciergePage));
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
            this.UpdatePersonality();
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
