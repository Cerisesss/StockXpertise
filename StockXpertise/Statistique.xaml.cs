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
    /// Logique d'interaction pour Statistique.xaml
    /// </summary>
    public partial class Statistique : Page
    {
        public Statistique()
        {
            InitializeComponent();
            Loaded += Statistique_Loaded;
            MainFrame.Visibility = Visibility.Visible;

            comboBoxAffichage_mode.Items.Add("Tableau");
            comboBoxAffichage_mode.Items.Add("Barres");
            comboBoxAffichage_mode.Items.Add("Circulaire");
            comboBoxAffichage_mode.Items.Add("Lignes");
            comboBoxAffichage_mode.Items.Add("Nuage de points");


            comboBoxAffichage.Items.Add("Jour");
            comboBoxAffichage.Items.Add("Semaine");
            comboBoxAffichage.Items.Add("Mois");
            comboBoxAffichage.Items.Add("Année");

            comboBoxAffichage_mode.SelectedItem = "Tableau";
            comboBoxAffichage.SelectedItem = "Jour";


        }

        private void Statistique_Loaded(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Visible; // Affiche la page une fois chargée
        }

        private void comboBoxAffichage_stat_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void generation_pdf(object sender, RoutedEventArgs e)
        {
            Statgénérate statistique = new Statgénérate(); 
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                statistique.PrixAchat = checkboxPrixAchat.IsChecked ?? false;
                statistique.PrixVente = checkboxPrixVente.IsChecked ?? false;
                statistique.ArticlesVendus = checkboxArticlesVendus.IsChecked ?? false;
                statistique.Marge = checkboxMarge.IsChecked ?? false;
                statistique.Top10Produits = checkboxTop10Produits.IsChecked ?? false;
                statistique.StockNegatif = checkboxStockNegatif.IsChecked ?? false;
                statistique.date = comboBoxAffichage.SelectedItem.ToString();

                parentWindow.Content = statistique;
                statistique.GenerateTable();
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_affichage(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_trier_par(object sender, SelectionChangedEventArgs e)
        {

        }

        private void framePrincipal_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
