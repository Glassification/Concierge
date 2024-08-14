// <copyright file="AdventurerWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Collections.Generic;
    using System.Windows;

    using Concierge.Character.Companions;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for AdventurerWindow.xaml.
    /// </summary>
    public partial class AdventurerWindow : ConciergeWindow
    {
        private bool itemsAdded;
        private bool isEditing;
        private Adventurer adventurer = Adventurer.Empty;
        private List<Adventurer> adventurers = [];

        public AdventurerWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.RaceComboBox.ItemsSource = ComboBoxGenerator.RacesComboBox();
            this.ClassComboBox.ItemsSource = ComboBoxGenerator.ClassesComboBox();
            this.StatusComboBox.ItemsSource = ComboBoxGenerator.PartyStatusComboBox();
            this.TypeComboBox.ItemsSource = ComboBoxGenerator.PartyTypeComboBox();
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.RaceComboBox);
            this.SetMouseOverEvents(this.ClassComboBox);
            this.SetMouseOverEvents(this.TypeComboBox);
            this.SetMouseOverEvents(this.StatusComboBox);
            this.SetMouseOverEvents(this.LevelUpDown);
            this.SetMouseOverEvents(this.PlayerNameTextBox, this.PlayerNameTextBackground);
            this.SetMouseOverEvents(this.CharacterNameTextBox, this.CharacterNameTextBackground);
        }

        public override string HeaderText => $"{(this.isEditing ? "Edit" : "Add")} Adventurer";

        public override string WindowName => nameof(AdventurerWindow);

        public override void ShowEdit<T>(T item)
        {
            if (item is not Adventurer adventurer)
            {
                return;
            }

            this.isEditing = true;
            this.adventurer = adventurer;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(adventurer);
            this.ShowConciergeWindow();
        }

        public override bool ShowAdd<T>(T item)
        {
            if (item is not List<Adventurer> adventurers)
            {
                return false;
            }

            this.isEditing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.adventurers = adventurers;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.itemsAdded;
        }

        protected override void ReturnAndClose()
        {
            if (this.PlayerNameTextBox.Text.IsNullOrWhiteSpace())
            {
                this.CloseConciergeWindow();
            }

            this.Result = ConciergeResult.OK;
            if (this.isEditing)
            {
                this.UpdateAdventurer(this.adventurer);
            }
            else
            {
                var adventurer = this.ToAdventurer();
                this.adventurers.Add(adventurer);
                Program.UndoRedoService.AddCommand(new AddCommand<Adventurer>(this.adventurers, adventurer, this.ConciergePage));
            }

            this.CloseConciergeWindow();
        }

        private void FillFields(Adventurer adventurer)
        {
            Program.Drawing();

            this.PlayerNameTextBox.Text = adventurer.PlayerName;
            this.CharacterNameTextBox.Text = adventurer.CharacterName;
            this.LevelUpDown.Value = adventurer.Level;
            this.RaceComboBox.Text = adventurer.Race;
            this.ClassComboBox.Text = adventurer.Class;
            this.StatusComboBox.Text = adventurer.Status.ToString();
            this.TypeComboBox.Text = adventurer.Type.PascalCase();

            Program.NotDrawing();
        }

        private void ClearFields()
        {
            Program.Drawing();

            this.PlayerNameTextBox.Text = string.Empty;
            this.CharacterNameTextBox.Text = string.Empty;
            this.LevelUpDown.Value = 0;
            this.RaceComboBox.Text = string.Empty;
            this.ClassComboBox.Text = string.Empty;
            this.StatusComboBox.Text = PartyStatus.Alive.ToString();
            this.TypeComboBox.Text = PartyType.PartyMember.PascalCase();

            Program.NotDrawing();
        }

        private void UpdateAdventurer(Adventurer adventurer)
        {
            var oldItem = adventurer.DeepCopy();

            adventurer.PlayerName = this.PlayerNameTextBox.Text;
            adventurer.CharacterName = this.CharacterNameTextBox.Text;
            adventurer.Level = this.LevelUpDown.Value;
            adventurer.Race = this.RaceComboBox.Text;
            adventurer.Class = this.ClassComboBox.Text;
            adventurer.Status = this.StatusComboBox.Text.ToEnum<PartyStatus>();
            adventurer.Type = this.TypeComboBox.Text.ToEnum<PartyType>();

            Program.UndoRedoService.AddCommand(new EditCommand<Adventurer>(adventurer, oldItem, this.ConciergePage));
        }

        private Adventurer ToAdventurer()
        {
            this.itemsAdded = true;
            var item = this.Create();

            return item;
        }

        private Adventurer Create()
        {
            return new Adventurer()
            {
                PlayerName = this.PlayerNameTextBox.Text,
                CharacterName = this.CharacterNameTextBox.Text,
                Level = this.LevelUpDown.Value,
                Race = this.RaceComboBox.Text,
                Class = this.ClassComboBox.Text,
                Status = this.StatusComboBox.Text.ToEnum<PartyStatus>(),
                Type = this.TypeComboBox.Text.ToEnum<PartyType>(),
            };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.PlayerNameTextBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            var adventurer = this.ToAdventurer();
            this.adventurers.Add(adventurer);
            Program.UndoRedoService.AddCommand(new AddCommand<Adventurer>(this.adventurers, adventurer, this.ConciergePage));

            this.ClearFields();
            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
