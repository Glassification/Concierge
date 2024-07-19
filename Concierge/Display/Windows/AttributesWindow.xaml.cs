// <copyright file="AttributesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Aspects;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for AttributesWindow.xaml.
    /// </summary>
    public partial class AttributesWindow : ConciergeWindow
    {
        private Attributes attributes = new ();

        public AttributesWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.StrengthUpDown);
            this.SetMouseOverEvents(this.DexterityUpDown);
            this.SetMouseOverEvents(this.ConstitutionUpDown);
            this.SetMouseOverEvents(this.IntelligenceUpDown);
            this.SetMouseOverEvents(this.WisdomUpDown);
            this.SetMouseOverEvents(this.CharismaUpDown);
        }

        public override string HeaderText => "Edit Attributes";

        public override string WindowName => nameof(AttributesWindow);

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.attributes = Program.CcsFile.Character.Attributes;
            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T attributes)
        {
            if (attributes is not Attributes castItem)
            {
                return;
            }

            this.attributes = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            this.UpdateAttributes();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
            Program.Drawing();

            this.StrengthUpDown.Value = this.attributes.Strength.Score;
            this.DexterityUpDown.Value = this.attributes.Dexterity.Score;
            this.ConstitutionUpDown.Value = this.attributes.Constitution.Score;
            this.IntelligenceUpDown.Value = this.attributes.Intelligence.Score;
            this.WisdomUpDown.Value = this.attributes.Wisdom.Score;
            this.CharismaUpDown.Value = this.attributes.Charisma.Score;

            Program.NotDrawing();
        }

        private void UpdateAttributes()
        {
            var oldItem = this.attributes.DeepCopy();

            this.attributes.Strength.Score = this.StrengthUpDown.Value;
            this.attributes.Dexterity.Score = this.DexterityUpDown.Value;
            this.attributes.Constitution.Score = this.ConstitutionUpDown.Value;
            this.attributes.Intelligence.Score = this.IntelligenceUpDown.Value;
            this.attributes.Wisdom.Score = this.WisdomUpDown.Value;
            this.attributes.Charisma.Score = this.CharismaUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<Attributes>(this.attributes, oldItem, this.ConciergePage));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateAttributes();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void GenerateAttributesButton_Click(object sender, RoutedEventArgs e)
        {
            this.NonBlockingWindow = ConciergeWindowService.ShowNonBlockingWindow(typeof(AttributeRollWindow));
        }
    }
}
