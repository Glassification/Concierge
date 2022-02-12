// <copyright file="ModifyClassResourceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyProficiencyWindow.xaml.
    /// </summary>
    public partial class ModifyClassResourceWindow : ConciergeWindow
    {
        public ModifyClassResourceWindow()
        {
            this.InitializeComponent();
            this.ConciergePage = ConciergePage.None;
            this.ClassResource = new ClassResource();
            this.ClassResources = new List<ClassResource>();
        }

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Resource";

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

            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateClassResource();
            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields()
        {
            this.ResourceTextBox.Text = this.ClassResource.Type;
            this.PoolUpDown.Value = this.ClassResource.Total;
            this.SpentUpDown.Value = this.ClassResource.Spent;
        }

        private void ClearFields()
        {
            this.ResourceTextBox.Text = string.Empty;
            this.PoolUpDown.Value = 0;
            this.SpentUpDown.Value = 0;
        }

        private ClassResource ToClassResource()
        {
            this.ItemsAdded = true;

            var resource = new ClassResource()
            {
                Type = this.ResourceTextBox.Text,
                Total = this.PoolUpDown.Value,
                Spent = this.SpentUpDown.Value,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<ClassResource>(this.ClassResources, resource, this.ConciergePage));

            return resource;
        }

        private void UpdateClassResource()
        {
            if (this.Editing)
            {
                var oldItem = this.ClassResource.DeepCopy();

                this.ClassResource.Type = this.ResourceTextBox.Text;
                this.ClassResource.Total = this.PoolUpDown.Value;
                this.ClassResource.Spent = this.SpentUpDown.Value;

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
    }
}
