using StockXpertise.Stock;
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
    /// Logique d'interaction pour inventaire.xaml
    /// </summary>
    public partial class inventaire : Page
    {
        private DataInventaire selectedData;
        public inventaire(DataInventaire selectedData)
        {
            InitializeComponent();

            this.selectedData = selectedData;
            LoadData();
        }

        public void LoadData()
        {
            // Récupérer les données de l'article pour les afficher dans les TextBoxs
            stock_actuel.Text = selectedData.Quantite_stock.ToString();
            emplacement_actuel.Text = selectedData.Code;
        }

        private void Annuler(object sender, RoutedEventArgs e)
        {
            // Retourner à la page Stock sans enregistrer les modifications
            Stock.Stock_inventaire stock_inventaire = new Stock.Stock_inventaire();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = stock_inventaire;
        }
        }

        private void Sauvegarder(object sender, RoutedEventArgs e)
        {
            // Récupérer le texte entrer dans les TextBoxs
            string emplacementReel = emplacement_reel.Text;
            string stockReel = stock_reel.Text;

            // Retourner à la page Stock après avoir enregistrer les modifications
            Stock.Storage stock = new Stock.Storage();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
        {
                parentWindow.Content = stock;
            }

        }
    }
}