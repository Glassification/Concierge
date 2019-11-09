using Concierge.Utility;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.DialogBoxes
{
    /// <summary>
    /// Interaction logic for MondifyConditionsWindow.xaml
    /// </summary>
    public partial class MondifyConditionsWindow : Window
    {
        public MondifyConditionsWindow()
        {
            InitializeComponent();

            BlindedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            CharmedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            DeafenedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            FatiguedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ExhaustionType)).Cast<Constants.ExhaustionType>();
            FrightenedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            GrappledComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            IncapacitatedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            InvisibleComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            ParalyzedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            PetrifiedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            PoisonedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            ProneComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            RestrainedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            StunnedComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
            UnconsciousComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ConditionTypes)).Cast<Constants.ConditionTypes>();
        }

        public void ShowEdit()
        {
            FillFields();
            ShowDialog();
        }

        private void FillFields()
        {
            BlindedComboBox.Text = Program.Character.Vitality.Conditions.Blinded.Equals("Cured") ? "Cured" : "Afflicted";
            CharmedComboBox.Text = Program.Character.Vitality.Conditions.Charmed.Equals("Cured") ? "Cured" : "Afflicted";
            DeafenedComboBox.Text = Program.Character.Vitality.Conditions.Deafened.Equals("Cured") ? "Cured" : "Afflicted";
            FatiguedComboBox.Text = Program.Character.Vitality.Conditions.Fatigued;
            FrightenedComboBox.Text = Program.Character.Vitality.Conditions.Frightened.Equals("Cured") ? "Cured" : "Afflicted";
            GrappledComboBox.Text = Program.Character.Vitality.Conditions.Grappled.Equals("Cured") ? "Cured" : "Afflicted";
            IncapacitatedComboBox.Text = Program.Character.Vitality.Conditions.Incapacitated.Equals("Cured") ? "Cured" : "Afflicted";
            InvisibleComboBox.Text = Program.Character.Vitality.Conditions.Invisible.Equals("Cured") ? "Cured" : "Afflicted";
            ParalyzedComboBox.Text = Program.Character.Vitality.Conditions.Paralyzed.Equals("Cured") ? "Cured" : "Afflicted";
            PetrifiedComboBox.Text = Program.Character.Vitality.Conditions.Petrified.Equals("Cured") ? "Cured" : "Afflicted";
            PoisonedComboBox.Text = Program.Character.Vitality.Conditions.Poisoned.Equals("Cured") ? "Cured" : "Afflicted";
            ProneComboBox.Text = Program.Character.Vitality.Conditions.Prone.Equals("Cured") ? "Cured" : "Afflicted";
            RestrainedComboBox.Text = Program.Character.Vitality.Conditions.Restrained.Equals("Cured") ? "Cured" : "Afflicted";
            StunnedComboBox.Text = Program.Character.Vitality.Conditions.Stunned.Equals("Cured") ? "Cured" : "Afflicted";
            UnconsciousComboBox.Text = Program.Character.Vitality.Conditions.Unconscious.Equals("Cured") ? "Cured" : "Afflicted";
            EncumbranceTextBox.Text = Program.Character.Vitality.Conditions.Encumbrance;
        }

        private void UpdateConditions()
        {
            Program.Character.Vitality.Conditions.Blinded = BlindedComboBox.Text.Equals("Cured") ? "Cured" : "Blinded";
            Program.Character.Vitality.Conditions.Charmed = CharmedComboBox.Text.Equals("Cured") ? "Cured" : "Charmed";
            Program.Character.Vitality.Conditions.Deafened = DeafenedComboBox.Text.Equals("Cured") ? "Cured" : "Deafened";
            Program.Character.Vitality.Conditions.Fatigued = FatiguedComboBox.Text;
            Program.Character.Vitality.Conditions.Frightened = FrightenedComboBox.Text.Equals("Cured") ? "Cured" : "Frightened";
            Program.Character.Vitality.Conditions.Grappled = GrappledComboBox.Text.Equals("Cured") ? "Cured" : "Grappled";
            Program.Character.Vitality.Conditions.Incapacitated = IncapacitatedComboBox.Text.Equals("Cured") ? "Cured" : "Incapacitated";
            Program.Character.Vitality.Conditions.Invisible = InvisibleComboBox.Text.Equals("Cured") ? "Cured" : "Invisible";
            Program.Character.Vitality.Conditions.Paralyzed = ParalyzedComboBox.Text.Equals("Cured") ? "Cured" : "Paralyzed";
            Program.Character.Vitality.Conditions.Petrified = PetrifiedComboBox.Text.Equals("Cured") ? "Cured" : "Petrified";
            Program.Character.Vitality.Conditions.Poisoned = PoisonedComboBox.Text.Equals("Cured") ? "Cured" : "Poisoned";
            Program.Character.Vitality.Conditions.Prone = ProneComboBox.Text.Equals("Cured") ? "Cured" : "Prone";
            Program.Character.Vitality.Conditions.Restrained = RestrainedComboBox.Text.Equals("Cured") ? "Cured" : "Restrained";
            Program.Character.Vitality.Conditions.Stunned = StunnedComboBox.Text.Equals("Cured") ? "Cured" : "Stunned";
            Program.Character.Vitality.Conditions.Unconscious = UnconsciousComboBox.Text.Equals("Cured") ? "Cured" : "Unconscious";
        }

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
            UpdateConditions();
            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateConditions();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
