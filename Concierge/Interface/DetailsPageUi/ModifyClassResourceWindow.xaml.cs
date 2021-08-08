// <copyright file="ModifyClassResourceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.DetailsPageUi
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Status;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyProficiencyWindow.xaml.
    /// </summary>
    public partial class ModifyClassResourceWindow : Window
    {
        public ModifyClassResourceWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Resource";

        private ClassResource ClassResource { get; set; }

        private List<ClassResource> ClassResources { get; set; }

        public void ShowAdd(List<ClassResource> classResources)
        {
            this.ClassResources = classResources;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClearFields();
            this.ShowDialog();
        }

        public void ShowEdit(ClassResource classResource)
        {
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClassResource = classResource;
            this.FillFields();
            this.ShowDialog();
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

        private ClassResource CreateClassResource()
        {
            return new ClassResource()
            {
                Type = this.ResourceTextBox.Text,
                Total = this.PoolUpDown.Value ?? 0,
                Spent = this.SpentUpDown.Value ?? 0,
            };
        }

        private void UpdateClassResource()
        {
            if (this.Editing)
            {
                this.ClassResource.Type = this.ResourceTextBox.Text;
                this.ClassResource.Total = this.PoolUpDown.Value ?? 0;
                this.ClassResource.Spent = this.SpentUpDown.Value ?? 0;
            }
            else
            {
                this.ClassResources.Add(this.CreateClassResource());
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.TapNavigation();

            this.UpdateClassResource();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.TapNavigation();

            this.UpdateClassResource();

            if (!this.Editing)
            {
                this.ClearFields();
            }

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}
