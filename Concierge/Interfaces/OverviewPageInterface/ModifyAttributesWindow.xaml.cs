// <copyright file="ModifyAttributesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
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
            this.ForceRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.Attributes = new Attributes();
        }

        public override string HeaderText => "Edit Attributes";

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
            if (attributes is not Attributes castItem)
            {
                return;
            }

            this.Attributes = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateAttributes();
            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields()
        {
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

            this.Attributes.Strength = this.StrengthUpDown.Value;
            this.Attributes.Dexterity = this.DexterityUpDown.Value;
            this.Attributes.Constitution = this.ConstitutionUpDown.Value;
            this.Attributes.Intelligence = this.IntelligenceUpDown.Value;
            this.Attributes.Wisdom = this.WisdomUpDown.Value;
            this.Attributes.Charisma = this.CharismaUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<Attributes>(this.Attributes, oldItem, this.ConciergePage));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
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
            this.CloseConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }
    }
}
