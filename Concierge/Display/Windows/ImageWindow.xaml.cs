// <copyright file="ImageWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Character;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Persistence;
    using Concierge.Persistence.Enums;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for ImageWindow.xaml.
    /// </summary>
    public partial class ImageWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService;

        public ImageWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.fileAccessService = new FileAccessService();
            this.FillTypeComboBox.ItemsSource = ComboBoxGenerator.StretchLevelComboBox();
            this.ConciergePage = ConciergePage.None;
            this.CharacterImage = new CharacterImage();
            this.OriginalFileName = string.Empty;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.UseCustomImageCheckBox);
            this.SetMouseOverEvents(this.ImageSourceTextBox, this.ImageSourceTextBackground);
            this.SetMouseOverEvents(this.FillTypeComboBox);
        }

        public override string HeaderText => "Edit Image";

        public override string WindowName => nameof(ImageWindow);

        private string OriginalFileName { get; set; }

        private bool IsDrawing { get; set; }

        private CharacterImage CharacterImage { get; set; }

        public override ConciergeResult ShowWizardSetup(string buttonText)
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
            this.Result = ConciergeResult.OK;
            this.UpdateCharacterImage();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
            this.IsDrawing = true;

            this.ImageSourceTextBox.Text = this.OriginalFileName = this.CharacterImage.Path;
            this.FillTypeComboBox.Text = this.CharacterImage.Stretch.ToString().FormatFromPascalCase();
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

            this.CharacterImage.Stretch = this.FillTypeComboBox.Text.Strip(" ").ToEnum<Stretch>();
            this.CharacterImage.UseCustomImage = this.UseCustomImageCheckBox.IsChecked ?? false;

            Program.UndoRedoService.AddCommand(new EditCommand<CharacterImage>(this.CharacterImage, oldItem, this.ConciergePage));
        }

        private void SetEnabledState(bool isEnabled)
        {
            DisplayUtility.SetControlEnableState(this.ImageSourceTextBox, isEnabled);
            DisplayUtility.SetControlEnableState(this.FillTypeComboBox, isEnabled);
            DisplayUtility.SetControlEnableState(this.OpenImageButton, isEnabled);
            DisplayUtility.SetControlEnableState(this.ImageSourceTextBackground, isEnabled);
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.fileAccessService.OpenFile((int)ImageFiltersIndex.Png, FileConstants.ImageOpenFilter, ImageFiltersIndex.Png.ToString().ToLower());

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
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
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
