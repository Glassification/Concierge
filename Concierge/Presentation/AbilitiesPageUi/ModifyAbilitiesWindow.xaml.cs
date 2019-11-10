using Concierge.Characters.Collections;
using System;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.AbilitiesPageUi
{
    /// <summary>
    /// Interaction logic for ModifyAbilitiesWindow.xaml
    /// </summary>
    public partial class ModifyAbilitiesWindow : Window
    {

        #region Constructor

        public ModifyAbilitiesWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public void ShowEdit(Ability ability)
        {
            HeaderTextBlock.Text = "Edit Ability";
            SelectedAbilityId = ability.ID;
            Editing = true;
            FillFields(ability);
            ButtonApply.Visibility = Visibility.Collapsed;

            ShowDialog();
        }

        public void ShowAdd()
        {
            HeaderTextBlock.Text = "Add Ability";
            Editing = false;
            ClearFields();
            ButtonApply.Visibility = Visibility.Visible;

            ShowDialog();
        }

        private void FillFields(Ability ability)
        {
            NameTextBox.Text = ability.Name;
            LevelTextBox.Text = ability.Level;
            UsesTextBox.Text = ability.Uses;
            RecoveryTextBox.Text = ability.Recovery;
            ActionTextBox.Text = ability.Action;
            NotesTextBox.Text = ability.Note;
        }

        private void ClearFields()
        {
            NameTextBox.Text = string.Empty;
            LevelTextBox.Text = string.Empty;
            UsesTextBox.Text = string.Empty;
            RecoveryTextBox.Text = string.Empty;
            ActionTextBox.Text = string.Empty;
            NotesTextBox.Text = string.Empty;
        }

        private Ability ToAbility()
        {
            Ability ability = new Ability()
            {
                Name = NameTextBox.Text,
                Level = LevelTextBox.Text,
                Uses = UsesTextBox.Text,
                Recovery = RecoveryTextBox.Text,
                Action = ActionTextBox.Text,
                Note = NotesTextBox.Text
            };

            return ability;
        }

        private void UpdateAbility(Ability ability)
        {
            ability.Name = NameTextBox.Text;
            ability.Level = LevelTextBox.Text;
            ability.Uses = UsesTextBox.Text;
            ability.Recovery = RecoveryTextBox.Text;
            ability.Action = ActionTextBox.Text;
            ability.Note = NotesTextBox.Text;
        }

        #endregion

        #region Accessors

        private bool Editing { get; set; }
        private Guid SelectedAbilityId { get; set; }

        #endregion

        #region Events

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Abilities.Add(ToAbility());
            ClearFields();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                UpdateAbility(Program.Character.GetAbilityById(SelectedAbilityId));
            }
            else
            {
                Program.Character.Abilities.Add(ToAbility());
            }

            Hide();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        #endregion
    }
}
