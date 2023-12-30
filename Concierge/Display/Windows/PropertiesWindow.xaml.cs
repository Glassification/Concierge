// <copyright file="PropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
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

            this.AlignmentComboBox.ItemsSource = ComboBoxGenerator.AlignmentTypesComboBox();
            this.RaceComboBox.ItemsSource = ComboBoxGenerator.RacesComboBox();
            this.BackgroundComboBox.ItemsSource = ComboBoxGenerator.BackgroundsComboBox();
            this.Class1Class.ItemsSource = ComboBoxGenerator.DetailedClassesComboBox();
            this.Class2Class.ItemsSource = ComboBoxGenerator.DetailedClassesComboBox();
            this.Class3Class.ItemsSource = ComboBoxGenerator.DetailedClassesComboBox();
            this.CharacterProperties = new CharacterProperties();
            this.OriginalFileName = string.Empty;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.NameTextBox, this.NameTextBackground);
            this.SetMouseOverEvents(this.RaceComboBox);
            this.SetMouseOverEvents(this.BackgroundComboBox);
            this.SetMouseOverEvents(this.AlignmentComboBox);
            this.SetMouseOverEvents(this.SubRaceComboBox);
            this.SetMouseOverEvents(this.Class1Level);
            this.SetMouseOverEvents(this.Class2Level);
            this.SetMouseOverEvents(this.Class3Level);
            this.SetMouseOverEvents(this.Class1Class);
            this.SetMouseOverEvents(this.Class2Class);
            this.SetMouseOverEvents(this.Class3Class);
            this.SetMouseOverEvents(this.Class1Subclass);
            this.SetMouseOverEvents(this.Class2Subclass);
            this.SetMouseOverEvents(this.Class3Subclass);

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

            DisplayUtility.SetControlEnableState(this.Class1Level, false);
            DisplayUtility.SetControlEnableState(this.Class2Level, false);
            DisplayUtility.SetControlEnableState(this.Class3Level, false);

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
        }

        private static void DisableAndBlank(ConciergeComboBox comboBox)
        {
            comboBox.Text = string.Empty;
            DisplayUtility.SetControlEnableState(comboBox, false);
        }

        private static void DisableAndBlank(IntegerUpDownControl integerUpDown)
        {
            integerUpDown.Value = 0;
            DisplayUtility.SetControlEnableState(integerUpDown, false);
        }

        private static void SetIntegerUpDownMax(IntegerUpDownControl control, int other1, int other2)
        {
            control.Maximum = Constants.MaxLevel - other1 - other2;
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

            this.CheckClass1State(this.Class1Class.Text);
            this.CheckClass2State(this.Class2Class.Text);
            this.CheckClass3State(this.Class3Class.Text);

            this.CheckRaceRelations(this.RaceComboBox.Text);

            SetIntegerUpDownMax(this.Class1Level, this.Class2Level.Value, this.Class3Level.Value);
            SetIntegerUpDownMax(this.Class2Level, this.Class1Level.Value, this.Class3Level.Value);
            SetIntegerUpDownMax(this.Class3Level, this.Class2Level.Value, this.Class1Level.Value);

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

            Program.UndoRedoService.AddCommand(new EditCommand<CharacterProperties>(this.CharacterProperties, oldItem, ConciergePage.None));
        }

        private void CheckClass1State(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                DisableAndBlank(this.Class1Subclass);
                DisableAndBlank(this.Class2Level);
                DisableAndBlank(this.Class2Class);
                DisableAndBlank(this.Class2Subclass);
                DisableAndBlank(this.Class3Level);
                DisableAndBlank(this.Class3Class);
                DisableAndBlank(this.Class3Subclass);
            }
            else
            {
                var temp = this.Class1Subclass.Text;
                this.Class1Subclass.ItemsSource = ComboBoxGenerator.SubClassesComboBox(name);
                this.Class1Subclass.Text = temp;
                DisplayUtility.SetControlEnableState(this.Class1Subclass, true);
                DisplayUtility.SetControlEnableState(this.Class2Level, true);
                DisplayUtility.SetControlEnableState(this.Class2Class, true);
            }
        }

        private void CheckClass2State(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                DisableAndBlank(this.Class2Subclass);
                DisableAndBlank(this.Class3Level);
                DisableAndBlank(this.Class3Class);
                DisableAndBlank(this.Class3Subclass);
            }
            else
            {
                var temp = this.Class2Subclass.Text;
                this.Class2Subclass.ItemsSource = ComboBoxGenerator.SubClassesComboBox(name);
                this.Class2Subclass.Text = temp;
                DisplayUtility.SetControlEnableState(this.Class2Subclass, true);
                DisplayUtility.SetControlEnableState(this.Class3Level, true);
                DisplayUtility.SetControlEnableState(this.Class3Class, true);
            }
        }

        private void CheckClass3State(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                DisableAndBlank(this.Class3Subclass);
            }
            else
            {
                var temp = this.Class3Subclass.Text;
                this.Class3Subclass.ItemsSource = ComboBoxGenerator.SubClassesComboBox(name);
                this.Class3Subclass.Text = temp;
                DisplayUtility.SetControlEnableState(this.Class3Subclass, true);
            }
        }

        private void CheckRaceRelations(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                DisableAndBlank(this.SubRaceComboBox);
            }
            else
            {
                var temp = this.SubRaceComboBox.Text;
                this.SubRaceComboBox.ItemsSource = ComboBoxGenerator.SubRacesComboBox(name);
                this.SubRaceComboBox.Text = temp;
                DisplayUtility.SetControlEnableState(this.SubRaceComboBox, true);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateProperties();
            this.InvokeApplyChanges();
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

        private void GenerateNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConciergeWindowService.ShowWindow(typeof(NameGeneratorWindow)) is string name)
            {
                this.NameTextBox.Text = name;
            }
        }

        private void Class1Class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ConciergeComboBox comboBox && comboBox.SelectedValue is DetailedComboBoxItemControl item)
            {
                this.CheckClass1State(item.Text);
            }
        }

        private void Class2Class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ConciergeComboBox comboBox && comboBox.SelectedValue is DetailedComboBoxItemControl item)
            {
                this.CheckClass2State(item.Text);
            }
        }

        private void Class3Class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ConciergeComboBox comboBox && comboBox.SelectedValue is DetailedComboBoxItemControl item)
            {
                this.CheckClass3State(item.Text);
            }
        }

        private void Class1Class_LostFocus(object sender, RoutedEventArgs e)
        {
            this.CheckClass1State(this.Class1Class.Text);
        }

        private void Class2Class_LostFocus(object sender, RoutedEventArgs e)
        {
            this.CheckClass2State(this.Class2Class.Text);
        }

        private void Class3Class_LostFocus(object sender, RoutedEventArgs e)
        {
            this.CheckClass3State(this.Class3Class.Text);
        }

        private void Class1Level_ValueChanged(object sender, RoutedEventArgs e)
        {
            SetIntegerUpDownMax(this.Class2Level, this.Class1Level.Value, this.Class3Level.Value);
            SetIntegerUpDownMax(this.Class3Level, this.Class1Level.Value, this.Class2Level.Value);
        }

        private void Class2Level_ValueChanged(object sender, RoutedEventArgs e)
        {
            SetIntegerUpDownMax(this.Class1Level, this.Class2Level.Value, this.Class3Level.Value);
            SetIntegerUpDownMax(this.Class3Level, this.Class1Level.Value, this.Class2Level.Value);
        }

        private void Class3Level_ValueChanged(object sender, RoutedEventArgs e)
        {
            SetIntegerUpDownMax(this.Class2Level, this.Class1Level.Value, this.Class3Level.Value);
            SetIntegerUpDownMax(this.Class1Level, this.Class3Level.Value, this.Class2Level.Value);
        }

        private void RaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ConciergeComboBox comboBox && comboBox.SelectedValue is DetailedComboBoxItemControl item)
            {
                this.CheckRaceRelations(item.Text);
            }
        }

        private void RaceComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.CheckRaceRelations(this.RaceComboBox.Text);
        }
    }
}
