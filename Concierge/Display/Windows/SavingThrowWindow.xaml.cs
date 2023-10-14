// <copyright file="SavingThrowWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.AbilitySaves;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for SavingThrowWindow.xaml.
    /// </summary>
    public partial class SavingThrowWindow : ConciergeWindow
    {
        public SavingThrowWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.StrengthComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.DexterityComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.ConstitutionComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.IntelligenceComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.WisdomComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.CharismaComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();

            this.ConciergePage = ConciergePage.None;
            this.SavingThrow = new SavingThrows();
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

        private SavingThrows SavingThrow { get; set; }

        public override void ShowEdit<T>(T savingThrow)
        {
            if (savingThrow is not SavingThrows castItem)
            {
                return;
            }

            this.SavingThrow = castItem;
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
            this.StrengthComboBox.Text = this.SavingThrow.Strength.CheckOverride.ToString();
            this.DexterityComboBox.Text = this.SavingThrow.Dexterity.CheckOverride.ToString();
            this.ConstitutionComboBox.Text = this.SavingThrow.Constitution.CheckOverride.ToString();
            this.IntelligenceComboBox.Text = this.SavingThrow.Intelligence.CheckOverride.ToString();
            this.WisdomComboBox.Text = this.SavingThrow.Wisdom.CheckOverride.ToString();
            this.CharismaComboBox.Text = this.SavingThrow.Charisma.CheckOverride.ToString();
        }

        private void UpdateSavingThrows()
        {
            var oldSavingThrow = this.SavingThrow.DeepCopy();

            this.SavingThrow.Strength.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.StrengthComboBox.Text);
            this.SavingThrow.Dexterity.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.DexterityComboBox.Text);
            this.SavingThrow.Constitution.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.ConstitutionComboBox.Text);
            this.SavingThrow.Intelligence.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.IntelligenceComboBox.Text);
            this.SavingThrow.Wisdom.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.WisdomComboBox.Text);
            this.SavingThrow.Charisma.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.CharismaComboBox.Text);

            Program.UndoRedoService.AddCommand(new EditCommand<SavingThrows>(this.SavingThrow, oldSavingThrow, this.ConciergePage));
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
