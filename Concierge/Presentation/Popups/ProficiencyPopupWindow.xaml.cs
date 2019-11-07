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

namespace Concierge.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for ProficiencyPopupWindow.xaml
    /// </summary>
    public partial class ProficiencyPopupWindow : Window
    {
        public ProficiencyPopupWindow()
        {
            InitializeComponent();
        }

        public Constants.PopupButtons ShowPopup()
        {
            ShowDialog();

            return ButtonPress;
        }

        private Constants.PopupButtons ButtonPress { get; set; }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.Cancel;
            Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.Cancel;
            Hide();
        }

        private void WeaponButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.WeaponProficiency;
            Hide();
        }

        private void ArmorButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.ArmorProficiency;
            Hide();

        }

        private void ShieldButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.ShieldProficiency;
            Hide();
        }

        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.ToolProficiency;
            Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    ButtonPress = Constants.PopupButtons.Cancel;
                    Hide();
                    break;
            }
        }
    }
}
