// <copyright file="ModifyPropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.CompanionPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Enums;

    /// <summary>
    /// Interaction logic for ModifyHealthWindow.xaml.
    /// </summary>
    public partial class ModifyPropertiesWindow : Window
    {
        public ModifyPropertiesWindow()
        {
            this.InitializeComponent();
            this.VisionComboBox.ItemsSource = Enum.GetValues(typeof(VisionTypes)).Cast<VisionTypes>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        public void ShowEdit()
        {
            this.FillFields();
            this.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        private void FillFields()
        {
            var companion = Program.CcsFile.Character.Companion;

            this.AcUpDown.UpdatingValue();
            this.PerceptionUpDown.UpdatingValue();
            this.MovementUpDown.UpdatingValue();

            this.NameTextBox.Text = companion.Name;
            this.AcUpDown.Value = companion.ArmorClass;
            this.PerceptionUpDown.Value = companion.Perception;
            this.VisionComboBox.Text = companion.Vision.ToString();
            this.MovementUpDown.Value = companion.Movement;
        }

        private void UpdateCompanion()
        {
            var companion = Program.CcsFile.Character.Companion;

            companion.Name = this.NameTextBox.Text;
            companion.ArmorClass = this.AcUpDown.Value ?? 0;
            companion.Perception = this.PerceptionUpDown.Value ?? 0;
            companion.Vision = (VisionTypes)Enum.Parse(typeof(VisionTypes), this.VisionComboBox.Text);
            companion.Movement = this.MovementUpDown.Value ?? 0;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateCompanion();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateCompanion();
            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
