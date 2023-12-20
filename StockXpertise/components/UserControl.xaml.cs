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

namespace StockXpertise.components
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void b4_Click(object sender, RoutedEventArgs e)
        {
            // Redirection vers la page statistique.xaml
            Statistique statistique = new Statistique();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = statistique;
            }
        }

        /*private void b1_Click(object sender, RoutedEventArgs e)
        {
            Stock stock = new Stock();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = stock;
            }
        }

        private void inventaire_Click(object sender, RoutedEventArgs e)
        {
            //Stock_inventaire stock_inventaire = new Stock_inventaire();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                //parentWindow.Content = stock_inventaire;
            }
        }*/

        private void b6_Click(object sender, RoutedEventArgs e)
        {
            Profile.Profile profile = new Profile.Profile();

            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = profile;
            }
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            fournisseur fournisseur = new fournisseur();

            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = fournisseur;
            }
        }
    }
}
