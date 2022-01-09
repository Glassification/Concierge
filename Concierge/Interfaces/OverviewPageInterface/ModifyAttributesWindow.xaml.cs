// <copyright file="ModifyAttributesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System;
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyAttributesWindow.xaml.
    /// </summary>
    public partial class ModifyAttributesWindow : ConciergeWindow
    {
        public ModifyAttributesWindow()
        {
            this.InitializeComponent();
            this.ConciergePage = ConciergePage.None;
        }

        private Attributes Attributes { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Attributes = Program.CcsFile.Character.Attributes;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T attributes)
        {
            var castItem = attributes as Attributes;
            this.Attributes = castItem;

            this.FillFields();
            this.ShowConciergeWindow();
        }

        private void FillFields()
        {
            this.StrengthUpDown.UpdatingValue();
            this.DexterityUpDown.UpdatingValue();
            this.ConstitutionUpDown.UpdatingValue();
            this.IntelligenceUpDown.UpdatingValue();
            this.WisdomUpDown.UpdatingValue();
            this.CharismaUpDown.UpdatingValue();

            this.StrengthUpDown.Value = this.Attributes.Strength;
            this.DexterityUpDown.Value = this.Attributes.Dexterity;
            this.ConstitutionUpDown.Value = this.Attributes.Constitution;
            this.IntelligenceUpDown.Value = this.Attributes.Intelligence;
            this.WisdomUpDown.Value = this.Attributes.Wisdom;
            this.CharismaUpDown.Value = this.Attributes.Charisma;
        }

        private void UpdateAttributes()
        {
            var oldItem = this.Attributes.DeepCopy();

            this.Attributes.Strength = this.StrengthUpDown.Value ?? 0;
            this.Attributes.Dexterity = this.DexterityUpDown.Value ?? 0;
            this.Attributes.Constitution = this.ConstitutionUpDown.Value ?? 0;
            this.Attributes.Intelligence = this.IntelligenceUpDown.Value ?? 0;
            this.Attributes.Wisdom = this.WisdomUpDown.Value ?? 0;
            this.Attributes.Charisma = this.CharismaUpDown.Value ?? 0;

            Program.UndoRedoService.AddCommand(new EditCommand<Attributes>(this.Attributes, oldItem, this.ConciergePage));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateAttributes();
            this.HideConciergeWindow();

            Program.Modify();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateAttributes();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }
    }
}
