// <copyright file="ModifyClassResourceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyProficiencyWindow.xaml.
    /// </summary>
    public partial class ModifyClassResourceWindow : Window, IConciergeModifyWindow
    {
        public ModifyClassResourceWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Resource";

        private ClassResource ClassResource { get; set; }

        private List<ClassResource> ClassResources { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClassResources = Program.CcsFile.Character.ClassResources;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowAdd(List<ClassResource> classResources)
        {
            this.ClassResources = classResources;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowDialog();
        }

        public void ShowEdit(ClassResource classResource)
        {
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClassResource = classResource;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
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

            Program.UndoRedoService.AddCommand(new AddCommand<ClassResource>(this.ClassResources, resource));

            return resource;
        }

        private void UpdateClassResource()
        {
            if (this.Editing)
            {
                var oldItem = this.ClassResource.DeepCopy() as ClassResource;

                this.ClassResource.Type = this.ResourceTextBox.Text;
                this.ClassResource.Total = this.PoolUpDown.Value ?? 0;
                this.ClassResource.Spent = this.SpentUpDown.Value ?? 0;

                Program.UndoRedoService.AddCommand(new EditCommand<ClassResource>(this.ClassResource, oldItem));
            }
            else
            {
                this.ClassResources.Add(this.ToClassResource());
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdateClassResource();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateClassResource();

            if (!this.Editing)
            {
                this.ClearFields();
            }

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
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
