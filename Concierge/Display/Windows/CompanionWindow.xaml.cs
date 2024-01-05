// <copyright file="CompanionWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for CompanionWindow.xaml.
    /// </summary>
    public partial class CompanionWindow : ConciergeWindow
    {
        public CompanionWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.VisionComboBox.ItemsSource = ComboBoxGenerator.VisionComboBox();
            this.CreatureSizeComboBox.ItemsSource = ComboBoxGenerator.CreatureSizesComboBox();
            this.ConciergePage = ConciergePage.None;
            this.Properties = new CompanionProperties();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.NameTextBox, this.NameTextBackground);
            this.SetMouseOverEvents(this.AcUpDown);
            this.SetMouseOverEvents(this.VisionComboBox);
            this.SetMouseOverEvents(this.CreatureSizeComboBox);
            this.SetMouseOverEvents(this.PerceptionUpDown);
            this.SetMouseOverEvents(this.MovementUpDown);
            this.SetMouseOverEvents(this.InitiativeUpDown);
        }

        public override string HeaderText => "Edit Companion Properties";

        public override string WindowName => nameof(CompanionWindow);

        private CompanionProperties Properties { get; set; }

        public override void ShowEdit<T>(T properties)
        {
            if (properties is not CompanionProperties castItem)
            {
                return;
            }

            this.Properties = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.UpdateCompanion();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
            this.NameTextBox.Text = this.Properties.Name;
            this.AcUpDown.Value = this.Properties.ArmorClass;
            this.PerceptionUpDown.Value = this.Properties.Perception;
            this.VisionComboBox.Text = this.Properties.Vision.ToString().FormatFromPascalCase();
            this.MovementUpDown.Value = this.Properties.Movement;
            this.CreatureSizeComboBox.Text = this.Properties.CreatureSize.ToString();
            this.InitiativeUpDown.Value = this.Properties.Initiative;
        }

        private void UpdateCompanion()
        {
            var oldItem = this.Properties.DeepCopy();

            this.Properties.Name = this.NameTextBox.Text;
            this.Properties.ArmorClass = this.AcUpDown.Value;
            this.Properties.Perception = this.PerceptionUpDown.Value;
            this.Properties.Vision = this.VisionComboBox.Text.Strip(" ").ToEnum<VisionTypes>();
            this.Properties.Movement = this.MovementUpDown.Value;
            this.Properties.CreatureSize = this.CreatureSizeComboBox.Text.ToEnum<CreatureSizes>();
            this.Properties.Initiative = this.InitiativeUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<CompanionProperties>(this.Properties, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateCompanion();
            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }
    }
}
