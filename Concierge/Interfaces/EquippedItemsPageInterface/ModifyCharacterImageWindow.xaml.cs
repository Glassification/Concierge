﻿// <copyright file="ModifyCharacterImageWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.EquippedItemsPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Interfaces.Enums;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for ModifyCharacterImageWindow.xaml.
    /// </summary>
    public partial class ModifyCharacterImageWindow : Window, IConciergeWindow
    {
        private readonly FileAccessService fileAccessService;
        private readonly BackgroundWorker toolTipTimer = new ();

        public ModifyCharacterImageWindow()
        {
            this.InitializeComponent();

            this.fileAccessService = new FileAccessService();
            this.FillTypeComboBox.ItemsSource = Enum.GetValues(typeof(Stretch)).Cast<Stretch>();

            this.InformationHover.ToolTip = new ToolTip()
            {
                Content = "768x1024 image ratio is recomended",
            };

            this.toolTipTimer.DoWork += this.BackgroundWorker_DoWork;
            this.toolTipTimer.RunWorkerCompleted += this.BackgroundWorker_RunWorkerCompleted;
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private ConciergeWindowResult Result { get; set; }

        private string OriginalFileName { get; set; }

        private bool IsDrawing { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.Draw();
            this.ShowDialog();

            return this.Result;
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        public void ShowWindow()
        {
            this.ApplyButton.Visibility = Visibility.Visible;

            this.Draw();
            this.ShowDialog();
        }

        public void Draw()
        {
            this.IsDrawing = true;

            var characterImage = Program.CcsFile.Character.CharacterImage;

            this.ImageSourceTextBox.Text = this.OriginalFileName = characterImage.Path;
            this.FillTypeComboBox.Text = characterImage.Stretch.ToString();
            this.UseCustomImageCheckBox.IsChecked = characterImage.UseCustomImage;

            this.SetEnabledState(characterImage.UseCustomImage);

            this.IsDrawing = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            (this.InformationHover.ToolTip as ToolTip).IsOpen = false;
            this.Hide();
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            var fileName = this.fileAccessService.OpenImage();

            this.ImageSourceTextBox.Text = fileName;
        }

        private void UpdateCharacterImage()
        {
            var characterImage = Program.CcsFile.Character.CharacterImage;

            if (!this.OriginalFileName.Equals(this.ImageSourceTextBox.Text))
            {
                characterImage.EncodeImage(this.ImageSourceTextBox.Text);
            }

            characterImage.Stretch = (Stretch)Enum.Parse(typeof(Stretch), this.FillTypeComboBox.Text);
            characterImage.UseCustomImage = this.UseCustomImageCheckBox.IsChecked ?? false;
        }

        private void SetEnabledState(bool isEnabled)
        {
            this.ImageSourceTextBox.IsEnabled = isEnabled;
            this.FillTypeComboBox.IsEnabled = isEnabled;
            this.OpenImageButton.IsEnabled = isEnabled;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.Result = ConciergeWindowResult.OK;
            this.UpdateCharacterImage();
            (this.InformationHover.ToolTip as ToolTip).IsOpen = false;

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateCharacterImage();
            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            (this.InformationHover.ToolTip as ToolTip).IsOpen = false;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            (this.InformationHover.ToolTip as ToolTip).IsOpen = false;
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    (this.InformationHover.ToolTip as ToolTip).IsOpen = false;
                    this.Hide();
                    break;
            }
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

        private void InformationHover_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (this.InformationHover.ToolTip as ToolTip).IsOpen = true;

            if (!this.toolTipTimer.IsBusy)
            {
                this.toolTipTimer.RunWorkerAsync();
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (this.InformationHover.ToolTip as ToolTip).IsOpen = false;
        }
    }
}