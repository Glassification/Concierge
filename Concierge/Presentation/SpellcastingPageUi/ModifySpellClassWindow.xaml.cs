// <copyright file="ModifySpellClassWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.SpellcastingPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Enums;
    using Concierge.Characters.Spellcasting;

    /// <summary>
    /// Interaction logic for ModifySpellClassWindow.xaml.
    /// </summary>
    public partial class ModifySpellClassWindow : Window
    {
        public ModifySpellClassWindow()
        {
            this.InitializeComponent();
            this.ClassNameComboBox.ItemsSource = Enum.GetValues(typeof(ClassType)).Cast<ClassType>();
            this.AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Abilities)).Cast<Abilities>();
        }

        private bool Editing { get; set; }

        private Guid SelectedClassId { get; set; }

        public void AddClass()
        {
            this.HeaderTextBlock.Text = "Add Magic Class";
            this.Editing = false;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.ClearFields();

            this.ShowDialog();
        }

        public void EditClass(MagicClass magicClass)
        {
            this.HeaderTextBlock.Text = "Edit Magic Class";
            this.SelectedClassId = magicClass.Id;
            this.Editing = true;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.FillFields(magicClass);

            this.ShowDialog();
        }

        private void FillFields(MagicClass magicClass)
        {
            this.ClassNameComboBox.Text = magicClass.Name;
            this.AbilityComboBox.Text = magicClass.Ability.ToString();
            this.AttackTextBox.Text = magicClass.Attack.ToString();
            this.SaveTextBox.Text = magicClass.Save.ToString();
            this.LevelUpDown.Value = magicClass.Level;
            this.CantripsUpDown.Value = magicClass.KnownCantrips;
            this.SpellsUpDown.Value = magicClass.KnownSpells;
            this.PreparedTextBox.Text = magicClass.PreparedSpells.ToString();
        }

        private void ClearFields()
        {
            this.ClassNameComboBox.Text = string.Empty;
            this.AbilityComboBox.Text = Abilities.NONE.ToString();
            this.AttackTextBox.Text = string.Empty;
            this.SaveTextBox.Text = string.Empty;
            this.LevelUpDown.Value = 0;
            this.CantripsUpDown.Value = 0;
            this.SpellsUpDown.Value = 0;
            this.PreparedTextBox.Text = string.Empty;
        }

        private void UpdateClass(MagicClass magicClass)
        {
            magicClass.Name = this.ClassNameComboBox.Text;
            magicClass.Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text);
            magicClass.Level = this.LevelUpDown.Value ?? 0;
            magicClass.KnownCantrips = this.CantripsUpDown.Value ?? 0;
            magicClass.KnownSpells = this.SpellsUpDown.Value ?? 0;

            Program.Modified = true;
        }

        private MagicClass ToClass()
        {
            var magicClass = new MagicClass()
            {
                Name = this.ClassNameComboBox.Text,
                Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text),
                Level = this.LevelUpDown.Value ?? 0,
                KnownSpells = this.SpellsUpDown.Value ?? 0,
                KnownCantrips = this.CantripsUpDown.Value ?? 0,
            };

            return magicClass;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Editing)
            {
                this.UpdateClass(Program.CcsFile.Character.GetMagicClassById(this.SelectedClassId));
            }
            else
            {
                Program.CcsFile.Character.MagicClasses.Add(this.ToClass());
                Program.Modified = true;
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.MagicClasses.Add(this.ToClass());
            Program.Modified = true;
            this.ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
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
