// <copyright file="PropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
    using Concierge.Persistence;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml.
    /// </summary>
    public partial class PropertiesWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService;

        public PropertiesWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.fileAccessService = new FileAccessService();

            this.AlignmentComboBox.ItemsSource = Defaults.Alignment;
            this.RaceComboBox.ItemsSource = Defaults.Races;
            this.SubRaceComboBox.ItemsSource = Defaults.Subrace;
            this.BackgroundComboBox.ItemsSource = Defaults.Backgrounds;
            this.Class1Class.ItemsSource = Defaults.Classes;
            this.Class2Class.ItemsSource = Defaults.Classes;
            this.Class3Class.ItemsSource = Defaults.Classes;
            this.Class1Subclass.ItemsSource = Defaults.Subclass;
            this.Class2Subclass.ItemsSource = Defaults.Subclass;
            this.Class3Subclass.ItemsSource = Defaults.Subclass;
            this.CharacterProperties = new CharacterProperties();
            this.OriginalFileName = string.Empty;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.NameTextBox);
            this.SetFocusEvents(this.RaceComboBox);
            this.SetFocusEvents(this.BackgroundComboBox);
            this.SetFocusEvents(this.AlignmentComboBox);
            this.SetFocusEvents(this.SubRaceComboBox);
            this.SetFocusEvents(this.Class1Level);
            this.SetFocusEvents(this.Class2Level);
            this.SetFocusEvents(this.Class3Level);
            this.SetFocusEvents(this.Class1Class);
            this.SetFocusEvents(this.Class2Class);
            this.SetFocusEvents(this.Class3Class);
            this.SetFocusEvents(this.Class1Subclass);
            this.SetFocusEvents(this.Class2Subclass);
            this.SetFocusEvents(this.Class3Subclass);
            this.SetFocusEvents(this.ImageSourceTextBox);
            this.SetFocusEvents(this.UseCustomImageCheckBox);

            Program.Logger.Info($"Initialized {nameof(PropertiesWindow)}.");
        }

        public override string HeaderText => "Edit Character Properties";

        public override string WindowName => nameof(PropertiesWindow);

        private CharacterProperties CharacterProperties { get; set; }

        private string OriginalFileName { get; set; }

        private bool IsDrawing { get; set; }

        private bool IsChanging { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;
            this.CharacterProperties = Program.CcsFile.Character.Properties;

            SetLevelEnableState(this.Class1Level, false);
            SetLevelEnableState(this.Class2Level, false);
            SetLevelEnableState(this.Class3Level, false);

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

        private static void SetLevelEnableState(IntegerUpDownControl level, bool isEnabled)
        {
            level.IsEnabled = isEnabled;
            level.Opacity = isEnabled ? 1 : 0.5;
        }

        private void FillFields()
        {
            this.IsDrawing = true;

            this.NameTextBox.Text = this.CharacterProperties.Name;
            this.RaceComboBox.Text = this.CharacterProperties.Race.Name;
            this.SubRaceComboBox.Text = this.CharacterProperties.Race.Subrace;
            this.BackgroundComboBox.Text = this.CharacterProperties.Background;
            this.AlignmentComboBox.Text = this.CharacterProperties.Alignment;
            this.Class1Level.Value = this.CharacterProperties.Class1.Level;
            this.Class2Level.Value = this.CharacterProperties.Class2.Level;
            this.Class3Level.Value = this.CharacterProperties.Class3.Level;
            this.Class1Class.Text = this.CharacterProperties.Class1.Name;
            this.Class2Class.Text = this.CharacterProperties.Class2.Name;
            this.Class3Class.Text = this.CharacterProperties.Class3.Name;
            this.Class1Subclass.Text = this.CharacterProperties.Class1.Subclass;
            this.Class2Subclass.Text = this.CharacterProperties.Class2.Subclass;
            this.Class3Subclass.Text = this.CharacterProperties.Class3.Subclass;
            this.ImageSourceTextBox.Text = this.OriginalFileName = this.CharacterProperties.CharacterIcon.Path;
            this.UseCustomImageCheckBox.IsChecked = this.CharacterProperties.CharacterIcon.UseCustomImage;

            this.SetImageEnabledState(this.CharacterProperties.CharacterIcon.UseCustomImage);

            this.IsDrawing = false;
        }

        private void UpdateProperties()
        {
            var oldItem = this.CharacterProperties.DeepCopy();

            this.CharacterProperties.Name = this.NameTextBox.Text;
            this.CharacterProperties.Race.Name = this.RaceComboBox.Text;
            this.CharacterProperties.Race.Subrace = this.SubRaceComboBox.Text;
            this.CharacterProperties.Background = this.BackgroundComboBox.Text;
            this.CharacterProperties.Alignment = this.AlignmentComboBox.Text;
            this.CharacterProperties.Class1.Level = this.Class1Level.Value;
            this.CharacterProperties.Class2.Level = this.Class2Level.Value;
            this.CharacterProperties.Class3.Level = this.Class3Level.Value;
            this.CharacterProperties.Class1.Name = this.Class1Class.Text;
            this.CharacterProperties.Class2.Name = this.Class2Class.Text;
            this.CharacterProperties.Class3.Name = this.Class3Class.Text;
            this.CharacterProperties.Class1.Subclass = this.Class1Subclass.Text;
            this.CharacterProperties.Class2.Subclass = this.Class2Subclass.Text;
            this.CharacterProperties.Class3.Subclass = this.Class3Subclass.Text;

            this.CharacterProperties.CharacterIcon.UseCustomImage = this.UseCustomImageCheckBox.IsChecked ?? false;
            this.CharacterProperties.CharacterIcon.Stretch = Stretch.UniformToFill;
            if (!this.OriginalFileName.Equals(this.ImageSourceTextBox.Text))
            {
                this.CharacterProperties.CharacterIcon.EncodeImage(this.ImageSourceTextBox.Text);
            }

            Program.UndoRedoService.AddCommand(new EditCommand<CharacterProperties>(this.CharacterProperties, oldItem, ConciergePage.None));
        }

        private void SetImageEnabledState(bool isEnabled)
        {
            this.OpenImageButton.IsEnabled = isEnabled;
            this.ImageSourceTextBoxBackground.IsEnabled = isEnabled;

            this.OpenImageButton.Opacity = isEnabled ? 1 : 0.5;
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
                this.SetImageEnabledState(true);
            }
        }

        private void UseCustomImageCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!this.IsDrawing)
            {
                this.SetImageEnabledState(false);
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

        private void SubRaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedText = string.Empty;
            foreach (var item in e.AddedItems)
            {
                changedText = item.ToString() ?? string.Empty;
            }

            if (changedText.IsNullOrWhiteSpace())
            {
                return;
            }

            Action a = () => this.SubRaceComboBox.Text = Race.FormatSubRace(changedText);
            this.Dispatcher.BeginInvoke(a);
        }

        private void SubclassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ConciergeComboBox comboBox)
            {
                return;
            }

            var changedText = string.Empty;
            foreach (var item in e.AddedItems)
            {
                changedText = item.ToString() ?? string.Empty;
            }

            if (changedText.IsNullOrWhiteSpace())
            {
                return;
            }

            Action a = () => comboBox.Text = CharacterClass.FormatSubclass(changedText);
            this.Dispatcher.BeginInvoke(a);
        }

        private void GenerateNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConciergeWindowService.ShowWindow(typeof(NameGeneratorWindow)) is string name)
            {
                this.NameTextBox.Text = name;
            }
        }
    }
}
