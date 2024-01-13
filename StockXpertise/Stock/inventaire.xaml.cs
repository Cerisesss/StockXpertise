using StockXpertise.Stock;
using System;
using System.Collections;
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

namespace StockXpertise.Stock
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
            // Récupère les données de l'article pour les afficher dans les TextBoxs
            stock_actuel.Text = selectedData.Quantite_stock.ToString();
            emplacement_actuel.Text = selectedData.Code;
            nom_item.Text = selectedData.Nom;
        }

        private void Annuler(object sender, RoutedEventArgs e)
        {
            // Retourne à la page Stock sans enregistrer les modifications
            affichage_inventaire inventory_display = new affichage_inventaire();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = inventory_display;
            }
        }

        private void Sauvegarder(object sender, RoutedEventArgs e)
        {
            // Récupère le texte entrer dans les TextBoxs
            string emplacementReel = emplacement_reel.Text;
            string stockReel = stock_reel.Text;

            if(string.IsNullOrEmpty(emplacementReel) && string.IsNullOrEmpty(stockReel))
            {
                MessageBox.Show("Veuillez remplir au moins un champ");
                return;
            }
            else
            {
                Int32.TryParse(stockReel, out var quantite);

                Query_Stock query_Update = new Query_Stock(quantite, emplacementReel, selectedData.Id_produit);

                if (!string.IsNullOrEmpty(stockReel))
                {
                    query_Update.Update_Quantite_Reel();
                }

                if (!string.IsNullOrEmpty(emplacementReel))
                {
                    query_Update.Update_Code_Reel();
                }

                // Retourner à la page affichage_iventaire après avoir enregistrer les modifications
                affichage_inventaire inventory_display = new affichage_inventaire();
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = inventory_display;
                }
            }
        }
    }
}