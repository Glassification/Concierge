using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.SpellcastingPageUi
{
    /// <summary>
    /// Interaction logic for SpellcastingSelectionWindow.xaml
    /// </summary>
    public partial class SpellcastingSelectionWindow : Window
    {
        public SpellcastingSelectionWindow()
        {
            InitializeComponent();
        }

        public Constants.PopupButtons ShowPopup()
        {
            ShowDialog();

            return ButtonPress;
        }

        private Constants.PopupButtons ButtonPress { get; set; }

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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.Cancel;
            Hide();
        }

        private void SpellClassButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.AddMagicClass;
            Hide();
        }

        private void SpellButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.AddSpell;
            Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButtons.Cancel;
            Hide();
        }
    }
}
