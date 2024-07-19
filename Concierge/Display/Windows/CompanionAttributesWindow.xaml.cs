// <copyright file="CompanionAttributesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Companions;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for CompanionAttributesWindow.xaml.
    /// </summary>
    public partial class CompanionAttributesWindow : ConciergeWindow
    {
        private CompanionAttributes attributes = new ();

        public CompanionAttributesWindow()
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

        public override string WindowName => nameof(CompanionAttributesWindow);

        public override void ShowEdit<T>(T attributes)
        {
            if (attributes is not CompanionAttributes castItem)
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

            this.StrengthUpDown.Value = this.attributes.Strength;
            this.DexterityUpDown.Value = this.attributes.Dexterity;
            this.ConstitutionUpDown.Value = this.attributes.Constitution;
            this.IntelligenceUpDown.Value = this.attributes.Intelligence;
            this.WisdomUpDown.Value = this.attributes.Wisdom;
            this.CharismaUpDown.Value = this.attributes.Charisma;

            Program.NotDrawing();
        }

        private void UpdateAttributes()
        {
            var oldItem = this.attributes.DeepCopy();

            this.attributes.Strength = this.StrengthUpDown.Value;
            this.attributes.Dexterity = this.DexterityUpDown.Value;
            this.attributes.Constitution = this.ConstitutionUpDown.Value;
            this.attributes.Intelligence = this.IntelligenceUpDown.Value;
            this.attributes.Wisdom = this.WisdomUpDown.Value;
            this.attributes.Charisma = this.CharismaUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<CompanionAttributes>(this.attributes, oldItem, this.ConciergePage));
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
