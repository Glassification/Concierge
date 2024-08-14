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
        private CompanionProperties properties = new ();

        public CompanionWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.VisionComboBox.ItemsSource = ComboBoxGenerator.VisionComboBox();
            this.CreatureSizeComboBox.ItemsSource = ComboBoxGenerator.CreatureSizesComboBox();
            this.ConciergePage = ConciergePages.None;
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

        public override void ShowEdit<T>(T properties)
        {
            if (properties is not CompanionProperties castItem)
            {
                return;
            }

            this.properties = castItem;
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
            Program.Drawing();

            this.NameTextBox.Text = this.properties.Name;
            this.AcUpDown.Value = this.properties.ArmorClass;
            this.PerceptionUpDown.Value = this.properties.Perception;
            this.VisionComboBox.Text = this.properties.Vision.PascalCase();
            this.MovementUpDown.Value = this.properties.Movement;
            this.CreatureSizeComboBox.Text = this.properties.CreatureSize.ToString();
            this.InitiativeUpDown.Value = this.properties.Initiative;

            Program.NotDrawing();
        }

        private void UpdateCompanion()
        {
            var oldItem = this.properties.DeepCopy();

            this.properties.Name = this.NameTextBox.Text;
            this.properties.ArmorClass = this.AcUpDown.Value;
            this.properties.Perception = this.PerceptionUpDown.Value;
            this.properties.Vision = this.VisionComboBox.Text.ToEnum<VisionTypes>();
            this.properties.Movement = this.MovementUpDown.Value;
            this.properties.CreatureSize = this.CreatureSizeComboBox.Text.ToEnum<CreatureSizes>();
            this.properties.Initiative = this.InitiativeUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<CompanionProperties>(this.properties, oldItem, this.ConciergePage));
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
