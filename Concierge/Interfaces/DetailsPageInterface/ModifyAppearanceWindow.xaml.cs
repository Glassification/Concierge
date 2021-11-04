// <copyright file="ModifyAppearanceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyAppearanceWindow.xaml.
    /// </summary>
    public partial class ModifyAppearanceWindow : Window, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyAppearanceWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.GenderComboBox.ItemsSource = Enum.GetValues(typeof(Gender)).Cast<Gender>();
            this.conciergePage = conciergePage;
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private Appearance Appearance { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Appearance = Program.CcsFile.Character.Appearance;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit(Appearance appearance)
        {
            this.Appearance = appearance;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void FillFields()
        {
            this.AgeUpDown.UpdatingValue();

            this.GenderComboBox.Text = this.Appearance.Gender;
            this.AgeUpDown.Value = this.Appearance.Age;
            this.HeightTextBox.Text = this.Appearance.Height;
            this.WeightTextBox.Text = this.Appearance.Weight;
            this.SkinColourTextBox.Text = this.Appearance.SkinColour;
            this.EyeColourTextBox.Text = this.Appearance.EyeColour;
            this.HairColourTextBox.Text = this.Appearance.HairColour;
            this.DistinguishingMarksTextBox.Text = this.Appearance.DistinguishingMarks;
        }

        private void UpdateAppearance()
        {
            var oldItem = this.Appearance.DeepCopy() as Appearance;

            this.Appearance.Gender = this.GenderComboBox.Text;
            this.Appearance.Age = this.AgeUpDown.Value ?? 0;
            this.Appearance.Height = this.HeightTextBox.Text;
            this.Appearance.Weight = this.WeightTextBox.Text;
            this.Appearance.SkinColour = this.SkinColourTextBox.Text;
            this.Appearance.EyeColour = this.EyeColourTextBox.Text;
            this.Appearance.HairColour = this.HairColourTextBox.Text;
            this.Appearance.DistinguishingMarks = this.DistinguishingMarksTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Appearance>(this.Appearance, oldItem, this.conciergePage));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdateAppearance();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateAppearance();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
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
