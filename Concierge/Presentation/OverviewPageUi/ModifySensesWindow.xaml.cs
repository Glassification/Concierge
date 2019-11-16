using Concierge.Utility;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.OverviewPageUi
{
    /// <summary>
    /// Interaction logic for ModifySensesWindow.xaml
    /// </summary>
    public partial class ModifySensesWindow : Window
    {
        public ModifySensesWindow()
        {
            InitializeComponent();
            VisionComboBox.ItemsSource = Enum.GetValues(typeof(Constants.VisionTypes)).Cast<Constants.VisionTypes>();
        }

        public void EditSenses()
        {
            FillFields();
            ShowDialog();
        }

        private void FillFields()
        {
            InitiativeTextBox.Text = Program.Character.Initiative.ToString();
            InitiativeBonusUpDown.Value = Program.Character.Details.InitiativeBonus;
            PerceptionTextBox.Text = Program.Character.PassivePerception.ToString();
            PerceptionBonusUpDown.Value = Program.Character.Details.PerceptionBonus;
            VisionComboBox.Text = Program.Character.Details.Vision.ToString();
            MovementTextBox.Text = Program.Character.Details.Movement.ToString();
            BaseMovementUpDown.Value = Program.Character.Details.BaseMovement;
        }

        private void UpdateSenses()
        {
            Program.Character.Details.InitiativeBonus = InitiativeBonusUpDown.Value ?? 0;
            Program.Character.Details.PerceptionBonus = PerceptionBonusUpDown.Value ?? 0;
            Program.Character.Details.Vision = (Constants.VisionTypes)Enum.Parse(typeof(Constants.VisionTypes), VisionComboBox.Text);
            Program.Character.Details.BaseMovement = BaseMovementUpDown.Value ?? 0;
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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSenses();
            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSenses();
            FillFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
