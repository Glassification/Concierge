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

namespace Concierge.Presentation
{
    /// <summary>
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Page
    {
        public DetailsPage()
        {
            InitializeComponent();
        }

        public void Draw()
        {
            DrawWealth();
        }

        private void DrawWealth()
        {
            TotalWealthField.Text = "¤ " + string.Format("{0:0.00}", Program.Character.Wealth.TotalValue);

            CopperField.Text = Program.Character.Wealth.Copper.ToString();
            SilverField.Text = Program.Character.Wealth.Silver.ToString();
            ElectrumField.Text = Program.Character.Wealth.Electrum.ToString();
            GoldField.Text = Program.Character.Wealth.Gold.ToString();
            PlatinumField.Text = Program.Character.Wealth.Platinum.ToString();
        }
    }
}
