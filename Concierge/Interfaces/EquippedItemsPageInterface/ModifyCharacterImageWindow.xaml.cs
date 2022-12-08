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
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for ModifyCharacterImageWindow.xaml.
    /// </summary>
    public partial class ModifyCharacterImageWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService;

        public ModifyCharacterImageWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.fileAccessService = new FileAccessService();
            this.FillTypeComboBox.ItemsSource = StringUtility.FormatEnumForDisplay(typeof(Stretch));
            this.ConciergePage = ConciergePage.None;
            this.CharacterImage = new CharacterImage();
            this.OriginalFileName = string.Empty;
        }

        public override string HeaderText => "Edit Image";

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
            if (characterImage is not CharacterImage castItem)
            {
                return;
            }

            this.CharacterImage = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;
            this.UpdateCharacterImage();
            this.CloseConciergeWindow();

            Program.Modify();
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
            this.ImageSourceTextBoxBackground.IsEnabled = isEnabled;

            this.FillTypeComboBox.Opacity = isEnabled ? 1 : 0.5;
            this.OpenImageButton.Opacity = isEnabled ? 1 : 0.5;
            this.ImageSourceLabel.Opacity = isEnabled ? 1 : 0.5;
            this.FillTypeLabel.Opacity = isEnabled ? 1 : 0.5;
            this.ImageSourceTextBoxBackground.Opacity = isEnabled ? 1 : 0.5;
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
            this.ReturnAndClose();
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
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
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
    }
}
