// <copyright file="ClassResourceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for ClassResourceWindow.xaml.
    /// </summary>
    public partial class ClassResourceWindow : ConciergeWindow
    {
        private bool editing;
        private ClassResource classResource = new ();
        private List<ClassResource> classResources = [];

        public ClassResourceWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePages.None;
            this.ResourceNameComboBox.ItemsSource = DefaultItems;
            this.RecoveryComboBox.ItemsSource = ComboBoxGenerator.RecoveryComboBox();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.ResourceNameComboBox);
            this.SetMouseOverEvents(this.PoolUpDown);
            this.SetMouseOverEvents(this.SpentUpDown);
            this.SetMouseOverEvents(this.RecoveryComboBox);
            this.SetMouseOverEvents(this.NotesTextBox, this.NotesTextBackground);
        }

        public override string HeaderText => $"{(this.editing ? "Edit" : "Add")} Resource";

        public override string WindowName => nameof(ClassResourceWindow);

        public bool ItemsAdded { get; private set; }

        private static List<ComboBoxItemControl> DefaultItems => ComboBoxGenerator.SelectorComboBox(Defaults.Resources, Program.CustomItemService.GetItems<ClassResource>());

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.classResources = Program.CcsFile.Character.Vitality.ClassResources;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T classResources)
        {
            if (classResources is not List<ClassResource> castItem)
            {
                return false;
            }

            this.classResources = castItem;
            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T classResource)
        {
            if (classResource is not ClassResource castItem)
            {
                return;
            }

            this.editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.classResource = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(this.classResource);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            this.UpdateClassResource();
            this.CloseConciergeWindow();
        }

        private void FillFields(ClassResource resource)
        {
            Program.Drawing();

            this.ResourceNameComboBox.Text = resource.Type;
            this.PoolUpDown.Value = resource.Total;
            this.SpentUpDown.Value = resource.Spent;
            this.RecoveryComboBox.Text = resource.Recovery.PascalCase();
            this.NotesTextBox.Text = resource.Note;

            this.SpentUpDown.Maximum = this.PoolUpDown.Value;
            Program.NotDrawing();
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();

            this.ResourceNameComboBox.Text = name;
            this.PoolUpDown.Value = 0;
            this.SpentUpDown.Value = 0;
            this.RecoveryComboBox.Text = Recovery.None.ToString();
            this.NotesTextBox.Text = string.Empty;

            Program.NotDrawing();
        }

        private ClassResource Create()
        {
            return new ClassResource()
            {
                Type = this.ResourceNameComboBox.Text,
                Total = this.PoolUpDown.Value,
                Spent = this.SpentUpDown.Value,
                Recovery = this.RecoveryComboBox.Text.ToEnum<Recovery>(),
                Note = this.NotesTextBox.Text,
            };
        }

        private ClassResource ToClassResource()
        {
            this.ItemsAdded = true;

            var resource = this.Create();
            Program.UndoRedoService.AddCommand(new AddCommand<ClassResource>(this.classResources, resource, this.ConciergePage));

            return resource;
        }

        private void UpdateClassResource()
        {
            if (this.editing)
            {
                var oldItem = this.classResource.DeepCopy();

                this.classResource.Type = this.ResourceNameComboBox.Text;
                this.classResource.Total = this.PoolUpDown.Value;
                this.classResource.Spent = this.SpentUpDown.Value;
                this.classResource.Recovery = this.RecoveryComboBox.Text.ToEnum<Recovery>();
                this.classResource.Note = this.NotesTextBox.Text;

                if (!this.classResource.IsCustom)
                {
                    Program.UndoRedoService.AddCommand(new EditCommand<ClassResource>(this.classResource, oldItem, this.ConciergePage));
                }
            }
            else
            {
                this.classResources.Add(this.ToClassResource());
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateClassResource();

            if (!this.editing)
            {
                this.ClearFields();
            }

            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void PoolSpentUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.SpentUpDown.Maximum = this.PoolUpDown.Value;
        }

        private void ResourceNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isLocked = this.LockButton.IsChecked ?? false;
            if (this.ResourceNameComboBox.SelectedItem is ComboBoxItemControl item && item.Item is ClassResource resource && !isLocked)
            {
                this.FillFields(resource);
            }
            else if (!isLocked)
            {
                this.ClearFields(this.ResourceNameComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourceNameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.ShowWarning("Could not save the Resource.\nA name is required before saving a custom item.");
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
            this.ClearFields();
            this.ResourceNameComboBox.ItemsSource = DefaultItems;
        }

        private void LockButton_Checked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.Lock;
        }

        private void LockButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.LockOpenVariant;
        }
    }
}
