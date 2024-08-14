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
        private Personality personality = new ();

        public PersonalityWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePages.None;
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

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.personality = Program.CcsFile.Character.Detail.Personality;
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

            this.personality = castItem;
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

            this.Trait1TextBox.Text = this.personality.Trait1;
            this.Trait2TextBox.Text = this.personality.Trait2;
            this.IdealTextBox.Text = this.personality.Ideal;
            this.BondTextBox.Text = this.personality.Bond;
            this.FlawTextBox.Text = this.personality.Flaw;
            this.NotesTextBox.Text = this.personality.Notes;

            Program.NotDrawing();
        }

        private void UpdatePersonality()
        {
            var oldItem = this.personality.DeepCopy();

            this.personality.Trait1 = this.Trait1TextBox.Text;
            this.personality.Trait2 = this.Trait2TextBox.Text;
            this.personality.Ideal = this.IdealTextBox.Text;
            this.personality.Bond = this.BondTextBox.Text;
            this.personality.Flaw = this.FlawTextBox.Text;
            this.personality.Notes = this.NotesTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Personality>(this.personality, oldItem, this.ConciergePage));
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
