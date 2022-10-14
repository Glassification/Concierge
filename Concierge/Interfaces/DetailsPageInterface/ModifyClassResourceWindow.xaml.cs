// <copyright file="ModifyClassResourceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for ModifyProficiencyWindow.xaml.
    /// </summary>
    public partial class ModifyClassResourceWindow : ConciergeWindow
    {
        public ModifyClassResourceWindow()
        {
            this.InitializeComponent();
            this.ConciergePage = ConciergePage.None;
            this.ResourceNameComboBox.ItemsSource = Constants.Resources;
            this.RecoveryComboBox.ItemsSource = StringUtility.FormatEnumForDisplay(typeof(Recovery));
            this.ClassResource = new ClassResource();
            this.ClassResources = new List<ClassResource>();
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Resource";

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private ClassResource ClassResource { get; set; }

        private List<ClassResource> ClassResources { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClassResources = Program.CcsFile.Character.ClassResources;
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

            Program.Modify();
        }

        private void FillFields(ClassResource resource)
        {
            this.ResourceNameComboBox.Text = resource.Type;
            this.PoolUpDown.Value = resource.Total;
            this.SpentUpDown.Value = resource.Spent;
            this.RecoveryComboBox.Text = resource.Recovery.ToString().FormatFromEnum();
            this.NotesTextBox.Text = resource.Note;

            this.SpentUpDown.Maximum = this.PoolUpDown.Value;
        }

        private void ClearFields()
        {
            this.ResourceNameComboBox.Text = string.Empty;
            this.PoolUpDown.Value = 0;
            this.SpentUpDown.Value = 0;
            this.RecoveryComboBox.Text = Recovery.None.ToString().FormatFromEnum();
            this.NotesTextBox.Text = string.Empty;
        }

        private ClassResource ToClassResource()
        {
            this.ItemsAdded = true;

            var resource = new ClassResource()
            {
                Type = this.ResourceNameComboBox.Text,
                Total = this.PoolUpDown.Value,
                Spent = this.SpentUpDown.Value,
                Recovery = (Recovery)Enum.Parse(typeof(Recovery), this.RecoveryComboBox.Text.Strip(" ")),
                Note = this.NotesTextBox.Text,
            };

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

                Program.UndoRedoService.AddCommand(new EditCommand<ClassResource>(this.ClassResource, oldItem, this.ConciergePage));
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

            Program.Modify();
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

        private void ResourceNameComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.ResourceNameComboBox.SelectedItem is ClassResource resource)
            {
                this.FillFields(resource);
            }
        }
    }
}
