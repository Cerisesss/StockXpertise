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

namespace StockXpertise
{
    /// <summary>
    /// Logique d'interaction pour affichageStock.xaml
    /// </summary>
    public partial class affichageStock : Page
    {
        public affichageStock(object selectedData)
        {
            InitializeComponent();
        }

        public void LoadData(string selectedItem)
        {

        }

        private void enregistrer_modification(object sender, RoutedEventArgs e)
        {

        }

        private void annuler(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Annulation des modifications");

            // Vérifie si la navigation est possible
            if (NavigationService.CanGoBack)
            {
                // Reviens à la fenêtre précédente
                NavigationService.GoBack();
            }
            else
            {
                Console.WriteLine("Impossible de revenir en arrière");
            }
        }

        private void Ajouter_image(object sender, RoutedEventArgs e)
        {

        }

        private void Supprimer_image(object sender, RoutedEventArgs e)
        {

        }

        private void Regenerer_code_barre(object sender, RoutedEventArgs e)
        {

        }
    }
}