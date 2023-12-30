// <copyright file="ClassResourceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
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

    /// <summary>
    /// Interaction logic for ClassResourceWindow.xaml.
    /// </summary>
    public partial class ClassResourceWindow : ConciergeWindow
    {
        public ClassResourceWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.ResourceNameComboBox.ItemsSource = DefaultItems;
            this.RecoveryComboBox.ItemsSource = ComboBoxGenerator.RecoveryComboBox();
            this.ClassResource = new ClassResource();
            this.ClassResources = [];
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.ResourceNameComboBox);
            this.SetMouseOverEvents(this.PoolUpDown);
            this.SetMouseOverEvents(this.SpentUpDown);
            this.SetMouseOverEvents(this.RecoveryComboBox);
            this.SetMouseOverEvents(this.NotesTextBox, this.NotesTextBackground);
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Resource";

        public override string WindowName => nameof(ClassResourceWindow);

        public bool ItemsAdded { get; private set; }

        private static List<ComboBoxItemControl> DefaultItems => ComboBoxGenerator.SelectorComboBox(Defaults.Resources, Program.CustomItemService.GetCustomItems<ClassResource>());

        private bool Editing { get; set; }

        private ClassResource ClassResource { get; set; }

        private List<ClassResource> ClassResources { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClassResources = Program.CcsFile.Character.Vitality.ClassResources;
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

            this.ClassResources = castItem;
            this.Editing = false;
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

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClassResource = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(this.ClassResource);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateClassResource();
            this.CloseConciergeWindow();
        }

        private void FillFields(ClassResource resource)
        {
            this.ResourceNameComboBox.Text = resource.Type;
            this.PoolUpDown.Value = resource.Total;
            this.SpentUpDown.Value = resource.Spent;
            this.RecoveryComboBox.Text = resource.Recovery.ToString().FormatFromPascalCase();
            this.NotesTextBox.Text = resource.Note;

            this.SpentUpDown.Maximum = this.PoolUpDown.Value;
        }

        private void ClearFields(string name = "")
        {
            this.ResourceNameComboBox.Text = name;
            this.PoolUpDown.Value = 0;
            this.SpentUpDown.Value = 0;
            this.RecoveryComboBox.Text = Recovery.None.ToString();
            this.NotesTextBox.Text = string.Empty;
        }

        private ClassResource Create()
        {
            return new ClassResource()
            {
                Type = this.ResourceNameComboBox.Text,
                Total = this.PoolUpDown.Value,
                Spent = this.SpentUpDown.Value,
                Recovery = (Recovery)Enum.Parse(typeof(Recovery), this.RecoveryComboBox.Text.Strip(" ")),
                Note = this.NotesTextBox.Text,
            };
        }

        private ClassResource ToClassResource()
        {
            this.ItemsAdded = true;

            var resource = this.Create();
            Program.UndoRedoService.AddCommand(new AddCommand<ClassResource>(this.ClassResources, resource, this.ConciergePage));

            return resource;
        }

        private void UpdateClassResource()
        {
            if (this.Editing)
            {
                var oldItem = this.ClassResource.DeepCopy();

                this.ClassResource.Type = this.ResourceNameComboBox.Text;
                this.ClassResource.Total = this.PoolUpDown.Value;
                this.ClassResource.Spent = this.SpentUpDown.Value;
                this.ClassResource.Recovery = (Recovery)Enum.Parse(typeof(Recovery), this.RecoveryComboBox.Text.Strip(" "));
                this.ClassResource.Note = this.NotesTextBox.Text;

                if (!this.ClassResource.IsCustom)
                {
                    Program.UndoRedoService.AddCommand(new EditCommand<ClassResource>(this.ClassResource, oldItem, this.ConciergePage));
                }
            }
            else
            {
                this.ClassResources.Add(this.ToClassResource());
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateClassResource();

            if (!this.Editing)
            {
                this.ClearFields();
            }

            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void PoolSpentUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.SpentUpDown.Maximum = this.PoolUpDown.Value;
        }

        private void ResourceNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ResourceNameComboBox.SelectedItem is ComboBoxItemControl item && item.Item is ClassResource resource)
            {
                this.FillFields(resource);
            }
            else
            {
                this.ClearFields(this.ResourceNameComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourceNameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.Show(
                    "Could not save the Resource.\nA name is required before saving a custom item.",
                    "Warning",
                    ConciergeWindowButtons.Ok,
                    ConciergeWindowIcons.Alert);
                return;
            }

            Program.CustomItemService.AddCustomItem(this.Create());
            this.ClearFields();
            this.ResourceNameComboBox.ItemsSource = DefaultItems;
        }
    }
}
