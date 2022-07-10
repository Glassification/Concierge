// <copyright file="ModifyPropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces
{
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Services;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyPropertiesWindow.xaml.
    /// </summary>
    public partial class ModifyPropertiesWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService;

        public ModifyPropertiesWindow()
        {
            this.InitializeComponent();
            this.fileAccessService = new FileAccessService();
            this.AlignmentComboBox.ItemsSource = Constants.Alignment;
            this.RaceComboBox.ItemsSource = Constants.Races;
            this.BackgroundComboBox.ItemsSource = Constants.Backgrounds;
            this.Class1ComboBox.ItemsSource = Constants.Classes;
            this.Class2ComboBox.ItemsSource = Constants.Classes;
            this.Class3ComboBox.ItemsSource = Constants.Classes;
            this.CharacterProperties = new CharacterProperties();
            this.OriginalFileName = string.Empty;

            Program.Logger.Info($"Initialized {nameof(ModifyPropertiesWindow)}.");
        }

        public override string HeaderText => "Edit Character Properties";

        private CharacterProperties CharacterProperties { get; set; }

        private string OriginalFileName { get; set; }

        private bool IsDrawing { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;
            this.CharacterProperties = Program.CcsFile.Character.Properties;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T properties)
        {
            if (properties is not CharacterProperties castItem)
            {
                return;
            }

            this.CharacterProperties = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateProperties();
            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields()
        {
            this.IsDrawing = true;

            this.NameTextBox.Text = this.CharacterProperties.Name;
            this.RaceComboBox.Text = this.CharacterProperties.Race;
            this.BackgroundComboBox.Text = this.CharacterProperties.Background;
            this.AlignmentComboBox.Text = this.CharacterProperties.Alignment;
            this.Level1UpDown.Value = this.CharacterProperties.Class1.Level;
            this.Level2UpDown.Value = this.CharacterProperties.Class2.Level;
            this.Level3UpDown.Value = this.CharacterProperties.Class3.Level;
            this.Class1ComboBox.Text = this.CharacterProperties.Class1.Name;
            this.Class2ComboBox.Text = this.CharacterProperties.Class2.Name;
            this.Class3ComboBox.Text = this.CharacterProperties.Class3.Name;
            this.ImageSourceTextBox.Text = this.OriginalFileName = this.CharacterProperties.CharacterIcon.Path;
            this.UseCustomImageCheckBox.IsChecked = this.CharacterProperties.CharacterIcon.UseCustomImage;

            this.SetEnabledState(this.CharacterProperties.CharacterIcon.UseCustomImage);

            this.IsDrawing = false;
        }

        private void UpdateProperties()
        {
            var oldItem = this.CharacterProperties.DeepCopy();

            this.CharacterProperties.Name = this.NameTextBox.Text;
            this.CharacterProperties.Race = this.RaceComboBox.Text;
            this.CharacterProperties.Background = this.BackgroundComboBox.Text;
            this.CharacterProperties.Alignment = this.AlignmentComboBox.Text;
            this.CharacterProperties.Class1.Level = this.Level1UpDown.Value;
            this.CharacterProperties.Class2.Level = this.Level2UpDown.Value;
            this.CharacterProperties.Class3.Level = this.Level3UpDown.Value;
            this.CharacterProperties.Class1.Name = this.Class1ComboBox.Text;
            this.CharacterProperties.Class2.Name = this.Class2ComboBox.Text;
            this.CharacterProperties.Class3.Name = this.Class3ComboBox.Text;

            this.CharacterProperties.CharacterIcon.UseCustomImage = this.UseCustomImageCheckBox.IsChecked ?? false;
            this.CharacterProperties.CharacterIcon.Stretch = Stretch.UniformToFill;
            if (!this.OriginalFileName.Equals(this.ImageSourceTextBox.Text))
            {
                this.CharacterProperties.CharacterIcon.EncodeImage(this.ImageSourceTextBox.Text);
            }

            Program.UndoRedoService.AddCommand(new EditCommand<CharacterProperties>(this.CharacterProperties, oldItem, ConciergePage.None));
        }

        private void SetEnabledState(bool isEnabled)
        {
            this.ImageSourceTextBox.IsEnabled = isEnabled;
            this.OpenImageButton.IsEnabled = isEnabled;
            this.ImageSourceTextBoxBackground.IsEnabled = isEnabled;

            this.OpenImageButton.Opacity = isEnabled ? 1 : 0.5;
            this.ImageSourceLabel.Opacity = isEnabled ? 1 : 0.5;
            this.ImageSourceTextBoxBackground.Opacity = isEnabled ? 1 : 0.5;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateProperties();
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

        private void UseCustomImageCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsDrawing)
            {
                this.SetEnabledState(true);
            }
        }

        private void UseCustomImageCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!this.IsDrawing)
            {
                this.SetEnabledState(false);
            }
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.fileAccessService.OpenImage();

            if (!fileName.IsNullOrWhiteSpace())
            {
                this.ImageSourceTextBox.Text = fileName;
            }
        }
    }
}
