// <copyright file="SavingThrowWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Aspects;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for SavingThrowWindow.xaml.
    /// </summary>
    public partial class SavingThrowWindow : ConciergeWindow
    {
        private Attributes attributes = new ();

        public SavingThrowWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.StrengthComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.DexterityComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.ConstitutionComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.IntelligenceComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.WisdomComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.CharismaComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();

            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.StrengthComboBox);
            this.SetMouseOverEvents(this.DexterityComboBox);
            this.SetMouseOverEvents(this.ConstitutionComboBox);
            this.SetMouseOverEvents(this.IntelligenceComboBox);
            this.SetMouseOverEvents(this.WisdomComboBox);
            this.SetMouseOverEvents(this.CharismaComboBox);
        }

        public override string HeaderText => "Edit Saving Throw Checks";

        public override string WindowName => nameof(SavingThrowWindow);

        public override void ShowEdit<T>(T savingThrow)
        {
            if (savingThrow is not Attributes castItem)
            {
                return;
            }

            this.attributes = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.UpdateSavingThrows();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
            Program.Drawing();

            this.StrengthComboBox.Text = this.attributes.Strength.SaveOverride.ToString();
            this.DexterityComboBox.Text = this.attributes.Dexterity.SaveOverride.ToString();
            this.ConstitutionComboBox.Text = this.attributes.Constitution.SaveOverride.ToString();
            this.IntelligenceComboBox.Text = this.attributes.Intelligence.SaveOverride.ToString();
            this.WisdomComboBox.Text = this.attributes.Wisdom.SaveOverride.ToString();
            this.CharismaComboBox.Text = this.attributes.Charisma.SaveOverride.ToString();

            Program.NotDrawing();
        }

        private void UpdateSavingThrows()
        {
            var oldSavingThrow = this.attributes.DeepCopy();

            this.attributes.Strength.SaveOverride = this.StrengthComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Dexterity.SaveOverride = this.DexterityComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Constitution.SaveOverride = this.ConstitutionComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Intelligence.SaveOverride = this.IntelligenceComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Wisdom.SaveOverride = this.WisdomComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Charisma.SaveOverride = this.CharismaComboBox.Text.ToEnum<StatusChecks>();

            Program.UndoRedoService.AddCommand(new EditCommand<Attributes>(this.attributes, oldSavingThrow, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSavingThrows();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }
    }
}
