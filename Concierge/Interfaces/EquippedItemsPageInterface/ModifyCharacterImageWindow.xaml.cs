// <copyright file="ModifyCharacterImageWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.EquippedItemsPageInterface
{
    using System;
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
    /// Interaction logic for ModifyCharacterImageWindow.xaml.
    /// </summary>
    public partial class ModifyCharacterImageWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService;

        public ModifyCharacterImageWindow()
        {
            this.InitializeComponent();

            this.fileAccessService = new FileAccessService();
            this.FillTypeComboBox.ItemsSource = Utilities.FormatEnumForDisplay(typeof(Stretch));
            this.ConciergePage = ConciergePage.None;
        }

        private string OriginalFileName { get; set; }

        private bool IsDrawing { get; set; }

        private CharacterImage CharacterImage { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CharacterImage = Program.CcsFile.Character.CharacterImage;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T characterImage)
        {
            var castItem = characterImage as CharacterImage;
            this.CharacterImage = castItem;

            this.FillFields();
            this.ShowConciergeWindow();
        }

        private void FillFields()
        {
            this.IsDrawing = true;

            this.ImageSourceTextBox.Text = this.OriginalFileName = this.CharacterImage.Path;
            this.FillTypeComboBox.Text = this.CharacterImage.Stretch.ToString().FormatFromEnum();
            this.UseCustomImageCheckBox.IsChecked = this.CharacterImage.UseCustomImage;

            this.SetEnabledState(this.CharacterImage.UseCustomImage);

            this.IsDrawing = false;
        }

        private void UpdateCharacterImage()
        {
            var oldItem = this.CharacterImage.DeepCopy();

            if (!this.OriginalFileName.Equals(this.ImageSourceTextBox.Text))
            {
                this.CharacterImage.EncodeImage(this.ImageSourceTextBox.Text);
            }

            this.CharacterImage.Stretch = (Stretch)Enum.Parse(typeof(Stretch), this.FillTypeComboBox.Text.Strip(" "));
            this.CharacterImage.UseCustomImage = this.UseCustomImageCheckBox.IsChecked ?? false;

            Program.UndoRedoService.AddCommand(new EditCommand<CharacterImage>(this.CharacterImage, oldItem, this.ConciergePage));
        }

        private void SetEnabledState(bool isEnabled)
        {
            this.ImageSourceTextBox.IsEnabled = isEnabled;
            this.FillTypeComboBox.IsEnabled = isEnabled;
            this.OpenImageButton.IsEnabled = isEnabled;

            this.FillTypeComboBox.Opacity = isEnabled ? 1 : 0.5;
            this.OpenImageButton.Opacity = isEnabled ? 1 : 0.5;
            this.ImageSourceLabel.Opacity = isEnabled ? 1 : 0.5;
            this.FillTypeLabel.Opacity = isEnabled ? 1 : 0.5;
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.fileAccessService.OpenImage();

            if (!fileName.IsNullOrWhiteSpace())
            {
                this.ImageSourceTextBox.Text = fileName;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;
            this.UpdateCharacterImage();
            this.HideConciergeWindow();

            Program.Modify();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateCharacterImage();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
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
    }
}
