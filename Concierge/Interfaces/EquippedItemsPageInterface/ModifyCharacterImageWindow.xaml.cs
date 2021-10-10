// <copyright file="ModifyCharacterImageWindow.xaml.cs" company="Thomas Beckett">
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

    using Concierge.Character.Characteristics;
    using Concierge.Interfaces.Enums;
    using Concierge.Services;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyCharacterImageWindow.xaml.
    /// </summary>
    public partial class ModifyCharacterImageWindow : Window, IConciergeModifyWindow
    {
        private readonly FileAccessService fileAccessService;
        private readonly BackgroundWorker toolTipTimer = new ();

        public ModifyCharacterImageWindow(string toolTipText)
        {
            this.InitializeComponent();

            this.fileAccessService = new FileAccessService();
            this.FillTypeComboBox.ItemsSource = Enum.GetValues(typeof(Stretch)).Cast<Stretch>();

            this.InformationHover.ToolTip = new ToolTip()
            {
                Content = toolTipText,
            };

            this.toolTipTimer.DoWork += this.BackgroundWorker_DoWork;
            this.toolTipTimer.RunWorkerCompleted += this.BackgroundWorker_RunWorkerCompleted;
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private ConciergeWindowResult Result { get; set; }

        private string OriginalFileName { get; set; }

        private bool IsDrawing { get; set; }

        private CharacterImage CharacterImage { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CharacterImage = Program.CcsFile.Character.CharacterImage;

            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        public void ShowEdit(CharacterImage characterImage)
        {
            this.ApplyButton.Visibility = Visibility.Visible;
            this.CharacterImage = characterImage;

            this.FillFields();
            this.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            (this.InformationHover.ToolTip as ToolTip).IsOpen = false;
            this.Hide();
        }

        private void FillFields()
        {
            this.IsDrawing = true;

            this.ImageSourceTextBox.Text = this.OriginalFileName = this.CharacterImage.Path;
            this.FillTypeComboBox.Text = this.CharacterImage.Stretch.ToString();
            this.UseCustomImageCheckBox.IsChecked = this.CharacterImage.UseCustomImage;

            this.SetEnabledState(this.CharacterImage.UseCustomImage);

            this.IsDrawing = false;
        }

        private void UpdateCharacterImage()
        {
            if (!this.OriginalFileName.Equals(this.ImageSourceTextBox.Text))
            {
                this.CharacterImage.EncodeImage(this.ImageSourceTextBox.Text);
            }

            this.CharacterImage.Stretch = (Stretch)Enum.Parse(typeof(Stretch), this.FillTypeComboBox.Text);
            this.CharacterImage.UseCustomImage = this.UseCustomImageCheckBox.IsChecked ?? false;
        }

        private void SetEnabledState(bool isEnabled)
        {
            this.ImageSourceTextBox.IsEnabled = isEnabled;
            this.FillTypeComboBox.IsEnabled = isEnabled;
            this.OpenImageButton.IsEnabled = isEnabled;
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
