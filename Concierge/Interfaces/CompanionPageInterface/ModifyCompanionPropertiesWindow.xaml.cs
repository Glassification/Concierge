// <copyright file="ModifyCompanionPropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.CompanionPageInterface
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyHealthWindow.xaml.
    /// </summary>
    public partial class ModifyCompanionPropertiesWindow : ConciergeWindow
    {
        public ModifyCompanionPropertiesWindow()
        {
            this.InitializeComponent();
            this.VisionComboBox.ItemsSource = Enum.GetValues(typeof(VisionTypes)).Cast<VisionTypes>();
            this.ConciergePage = ConciergePage.None;
        }

        private CompanionProperties Properties { get; set; }

        public override void ShowEdit<T>(T properties)
        {
            var castItem = properties as CompanionProperties;
            this.Properties = castItem;

            this.FillFields();
            this.ShowConciergeWindow();
        }

        private void FillFields()
        {
            this.AcUpDown.UpdatingValue();
            this.PerceptionUpDown.UpdatingValue();
            this.MovementUpDown.UpdatingValue();

            this.NameTextBox.Text = this.Properties.Name;
            this.AcUpDown.Value = this.Properties.ArmorClass;
            this.PerceptionUpDown.Value = this.Properties.Perception;
            this.VisionComboBox.Text = this.Properties.Vision.ToString();
            this.MovementUpDown.Value = this.Properties.Movement;
        }

        private void UpdateCompanion()
        {
            var oldItem = this.Properties.DeepCopy();

            this.Properties.Name = this.NameTextBox.Text;
            this.Properties.ArmorClass = this.AcUpDown.Value ?? 0;
            this.Properties.Perception = this.PerceptionUpDown.Value ?? 0;
            this.Properties.Vision = (VisionTypes)Enum.Parse(typeof(VisionTypes), this.VisionComboBox.Text);
            this.Properties.Movement = this.MovementUpDown.Value ?? 0;

            Program.UndoRedoService.AddCommand(new EditCommand<CompanionProperties>(this.Properties, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.HideConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.HideConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateCompanion();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateCompanion();
            this.HideConciergeWindow();

            Program.Modify();
        }
    }
}
