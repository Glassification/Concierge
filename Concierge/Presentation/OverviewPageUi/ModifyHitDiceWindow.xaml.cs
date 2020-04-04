namespace Concierge.Presentation.OverviewPageUi
{
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
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for ModifyHitDiceWindow.xaml
    /// </summary>
    public partial class ModifyHitDiceWindow : Window
    {
        public ModifyHitDiceWindow()
        {
            InitializeComponent();
        }

        public void ModifyHitDice()
        {
            SetHitDice();

            ShowDialog();
        }

        private void SetHitDice()
        {
            TotalD6UpDown.Value = Program.Character.Vitality.HitDice.TotalD6;
            TotalD8UpDown.Value = Program.Character.Vitality.HitDice.TotalD8;
            TotalD10UpDown.Value = Program.Character.Vitality.HitDice.TotalD10;
            TotalD12UpDown.Value = Program.Character.Vitality.HitDice.TotalD12;

            UsedD6UpDown.Value = Program.Character.Vitality.HitDice.SpentD6;
            UsedD8UpDown.Value = Program.Character.Vitality.HitDice.SpentD8;
            UsedD10UpDown.Value = Program.Character.Vitality.HitDice.SpentD10;
            UsedD12UpDown.Value = Program.Character.Vitality.HitDice.SpentD12;
        }

        private void GetHitDice()
        {
            Program.Character.Vitality.HitDice.TotalD6 = TotalD6UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.TotalD8 = TotalD8UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.TotalD10 = TotalD10UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.TotalD12 = TotalD12UpDown.Value ?? 0;

            Program.Character.Vitality.HitDice.SpentD6 = UsedD6UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.SpentD8 = UsedD8UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.SpentD10 = UsedD10UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.SpentD12 = UsedD12UpDown.Value ?? 0;
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            GetHitDice();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            GetHitDice();
            Hide();
        }
    }
}
