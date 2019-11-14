using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Concierge.Presentation.SpellcastingPageUi
{
    /// <summary>
    /// Interaction logic for ModifySpellClassWindow.xaml
    /// </summary>
    public partial class ModifySpellClassWindow : Window
    {
        public ModifySpellClassWindow()
        {
            InitializeComponent();
            ClassNameComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ClassTypes)).Cast<Constants.ClassTypes>();
            AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Constants.Abilities)).Cast<Constants.Abilities>();
        }

        public void AddClass()
        {
            HeaderTextBlock.Text = "Add Magic Class";
            Editing = false;
            ApplyButton.Visibility = Visibility.Visible;
            ClearFields();

            ShowDialog();
        }

        public void EditClass(MagicClass magicClass)
        {
            HeaderTextBlock.Text = "Edit Magic Class";
            SelectedClassId = magicClass.ID;
            Editing = true;
            ApplyButton.Visibility = Visibility.Collapsed;
            FillFields(magicClass);

            ShowDialog();
        }

        private void FillFields(MagicClass magicClass)
        {
            ClassNameComboBox.Text = magicClass.Name;
            AbilityComboBox.Text = magicClass.Ability.ToString();
            AttackTextBox.Text = magicClass.Attack.ToString();
            SaveTextBox.Text = magicClass.Save.ToString();
            LevelUpDown.Value = magicClass.Level;
            CantripsUpDown.Value = magicClass.KnownCantrips;
            SpellsUpDown.Value = magicClass.KnownSpells;
            PreparedTextBox.Text = magicClass.PreparedSpells.ToString();
        }

        private void ClearFields()
        {
            ClassNameComboBox.Text = string.Empty;
            AbilityComboBox.Text = Constants.Abilities.NONE.ToString();
            AttackTextBox.Text = string.Empty;
            SaveTextBox.Text = string.Empty;
            LevelUpDown.Value = 0;
            CantripsUpDown.Value = 0;
            SpellsUpDown.Value = 0;
            PreparedTextBox.Text = string.Empty;
        }

        private void UpdateClass(MagicClass magicClass)
        {
            magicClass.Name = ClassNameComboBox.Text;
            magicClass.Ability = (Constants.Abilities)Enum.Parse(typeof(Constants.Abilities), AbilityComboBox.Text);
            magicClass.Level = LevelUpDown.Value ?? 0;
            magicClass.KnownCantrips = CantripsUpDown.Value ?? 0;
            magicClass.KnownSpells = SpellsUpDown.Value ?? 0;
        }

        private MagicClass ToClass()
        {
            MagicClass magicClass = new MagicClass()
            {
                Name = ClassNameComboBox.Text,
                Ability = (Constants.Abilities)Enum.Parse(typeof(Constants.Abilities), AbilityComboBox.Text),
                Level = LevelUpDown.Value ?? 0,
                KnownSpells = SpellsUpDown.Value ?? 0,
                KnownCantrips = CantripsUpDown.Value ?? 0
            };

            return magicClass;
        }

        private bool Editing { get; set; }
        private Guid SelectedClassId { get; set; }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                UpdateClass(Program.Character.GetMagicClassById(SelectedClassId));
            }
            else
            {
                Program.Character.MagicClasses.Add(ToClass());
            }

            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.MagicClasses.Add(ToClass());
            ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
