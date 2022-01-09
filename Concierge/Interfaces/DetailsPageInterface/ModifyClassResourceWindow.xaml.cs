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
            var castItem = classResources as List<ClassResource>;
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
            var castItem = classResource as ClassResource;
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClassResource = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowConciergeWindow();
        }

        private void FillFields()
        {
            this.PoolUpDown.UpdatingValue();
            this.SpentUpDown.UpdatingValue();

            this.ResourceTextBox.Text = this.ClassResource.Type;
            this.PoolUpDown.Value = this.ClassResource.Total;
            this.SpentUpDown.Value = this.ClassResource.Spent;
        }

        private void ClearFields()
        {
            this.PoolUpDown.UpdatingValue();
            this.SpentUpDown.UpdatingValue();

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
                Total = this.PoolUpDown.Value ?? 0,
                Spent = this.SpentUpDown.Value ?? 0,
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
                this.ClassResource.Total = this.PoolUpDown.Value ?? 0;
                this.ClassResource.Spent = this.SpentUpDown.Value ?? 0;

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
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateClassResource();
            this.HideConciergeWindow();

            Program.Modify();
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
            this.HideConciergeWindow();
        }
    }
}
