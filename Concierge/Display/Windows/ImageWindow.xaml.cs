// <copyright file="ImageWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using Concierge.Persistence;
    using Concierge.Persistence.Enums;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for ImageWindow.xaml.
    /// </summary>
    public partial class ImageWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService = new ();
        private readonly ImageEncoding imageEncoding = new (Program.ErrorService);

        private Portrait image = new ();
        private bool isDrawing;
        private string base64 = string.Empty;

        public ImageWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.FillTypeComboBox.ItemsSource = ComboBoxGenerator.StretchLevelComboBox();
            this.ConciergePage = ConciergePage.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.UseCustomImageCheckBox);
            this.SetMouseOverEvents(this.ClearImageButton);
            this.SetMouseOverEvents(this.OpenImageButton);
            this.SetMouseOverEvents(this.FillTypeComboBox);
        }

        public override string HeaderText => "Edit Image";

        public override string WindowName => nameof(ImageWindow);

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T characterImage)
        {
            if (characterImage is not Portrait castItem)
            {
                return;
            }

            this.image = castItem;
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
            this.isDrawing = true;

            var useImage = this.image.UseCustomImage;
            var stretch = this.image.Stretch;

            this.FillTypeComboBox.Text = stretch.PascalCase();
            this.ImageNameTextBox.Text = this.image.Name;
            this.UseCustomImageCheckBox.IsChecked = useImage;
            this.HorizontalPreview.Stretch = stretch;
            this.VerticalPreview.Stretch = stretch;
            this.base64 = this.image.Encoded;

            this.SetEnabledState(useImage);

            this.isDrawing = false;
        }

        private void ClearFields()
        {
            this.isDrawing = true;

            this.FillTypeComboBox.Text = Stretch.None.PascalCase();
            this.ImageNameTextBox.Text = string.Empty;
            this.UseCustomImageCheckBox.IsChecked = false;
            this.HorizontalPreview.Stretch = Stretch.None;
            this.VerticalPreview.Stretch = Stretch.None;
            this.base64 = string.Empty;

            this.SetEnabledState(false);

            this.isDrawing = false;
        }

        private void UpdateCharacterImage()
        {
            var oldItem = this.image.DeepCopy();

            this.image.Stretch = this.FillTypeComboBox.Text.ToEnum<Stretch>();
            this.image.Name = this.ImageNameTextBox.Text;
            this.image.UseCustomImage = this.UseCustomImageCheckBox.IsChecked ?? false;
            this.image.Encoded = this.base64;

            Program.UndoRedoService.AddCommand(new EditCommand<Portrait>(this.image, oldItem, this.ConciergePage));
        }

        private void SetEnabledState(bool isEnabled)
        {
            DisplayUtility.SetControlEnableState(this.FillTypeComboBox, isEnabled);
            DisplayUtility.SetControlEnableState(this.OpenImageButton, isEnabled);
            DisplayUtility.SetControlEnableState(this.ImageNameTextBox, isEnabled);
            DisplayUtility.SetControlEnableState(this.ImageNameTextBackground, isEnabled);

            var renderImage = isEnabled && !this.base64.IsNullOrWhiteSpace();
            this.HorizontalPreview.Source = renderImage ? this.imageEncoding.Decode(this.base64) : null;
            this.VerticalPreview.Source = renderImage ? this.imageEncoding.Decode(this.base64) : null;

            this.ImageNameTextBackground.IsEnabled = false;
        }

        private bool IsValidImage()
        {
            if (this.base64.IsNullOrWhiteSpace() && (this.UseCustomImageCheckBox.IsChecked ?? false))
            {
                ConciergeMessageBox.Show(
                    "Please select an image, or disable custom images before continuing.",
                    "Warning",
                    ConciergeButtons.Ok,
                    ConciergeIcons.Warning);
                return false;
            }

            return true;
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.fileAccessService.OpenFile((int)ImageFiltersIndex.Png, FileConstants.ImageOpenFilter, ImageFiltersIndex.Png.ToString().ToLower());
            if (fileName.IsNullOrWhiteSpace())
            {
                return;
            }

            var base64 = this.imageEncoding.Encode(fileName);
            var image = this.imageEncoding.Decode(base64);
            if (image is null)
            {
                return;
            }

            this.HorizontalPreview.Source = image;
            this.VerticalPreview.Source = image;
            this.ImageNameTextBox.Text = Path.GetFileName(fileName);
            this.base64 = base64;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsValidImage())
            {
                this.ReturnAndClose();
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsValidImage())
            {
                this.UpdateCharacterImage();
                this.InvokeApplyChanges();
            }
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
            if (!this.isDrawing)
            {
                this.SetEnabledState(true);
            }
        }

        private void UseCustomImageCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!this.isDrawing)
            {
                this.SetEnabledState(false);
            }
        }

        private void FillTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (e.AddedItems[0] is ComboBoxItemControl item)
            {
                var stretch = item.ToString().ToEnum<Stretch>();
                this.HorizontalPreview.Stretch = stretch;
                this.VerticalPreview.Stretch = stretch;
            }
        }

        private void ClearImageButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearFields();
        }
    }
}
