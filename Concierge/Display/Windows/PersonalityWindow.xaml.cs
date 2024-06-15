// <copyright file="PersonalityWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Details;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for PersonalityWindow.xaml.
    /// </summary>
    public partial class PersonalityWindow : ConciergeWindow
    {
        public PersonalityWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.Personality = new Personality();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.Trait1TextBox, this.Trait1TextBackground);
            this.SetMouseOverEvents(this.Trait2TextBox, this.Trait2TextBackground);
            this.SetMouseOverEvents(this.IdealTextBox, this.IdealTextBackground);
            this.SetMouseOverEvents(this.BondTextBox, this.BondTextBackground);
            this.SetMouseOverEvents(this.FlawTextBox, this.FlawTextBackground);
            this.SetMouseOverEvents(this.NotesTextBox, this.NotesTextBackground);
        }

        public override string HeaderText => "Edit Personality";

        public override string WindowName => nameof(PersonalityWindow);

        private Personality Personality { get; set; }

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.Personality = Program.CcsFile.Character.Detail.Personality;
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
            this.Result = ConciergeResult.OK;

            this.UpdatePersonality();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
            Program.Drawing();

            this.Trait1TextBox.Text = this.Personality.Trait1;
            this.Trait2TextBox.Text = this.Personality.Trait2;
            this.IdealTextBox.Text = this.Personality.Ideal;
            this.BondTextBox.Text = this.Personality.Bond;
            this.FlawTextBox.Text = this.Personality.Flaw;
            this.NotesTextBox.Text = this.Personality.Notes;

            Program.NotDrawing();
        }

        private void UpdatePersonality()
        {
            var oldItem = this.Personality.DeepCopy();

            this.Personality.Trait1 = this.Trait1TextBox.Text;
            this.Personality.Trait2 = this.Trait2TextBox.Text;
            this.Personality.Ideal = this.IdealTextBox.Text;
            this.Personality.Bond = this.BondTextBox.Text;
            this.Personality.Flaw = this.FlawTextBox.Text;
            this.Personality.Notes = this.NotesTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Personality>(this.Personality, oldItem, this.ConciergePage));
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
            this.UpdatePersonality();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
