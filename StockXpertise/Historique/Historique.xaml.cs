using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StockXpertise
{
    public partial class Historique : Page
    {
        public Historique()
        {
            InitializeComponent();
            Loaded += Historique_Loaded;
        }

        private void Historique_Loaded(object sender, RoutedEventArgs e)
        {
            // Charger les données initiales
            comboBoxAffichage.ItemsSource = new string[] { "Quantité des stocks modifiée (tous)", "Action d'utilisateur", "Entrées", "Sorties", "Transactions (caissier)" };
            comboBoxAffichage.SelectedIndex = 0;
            ChargerComboBoxUtilisateur();

            // Gestionnaire d'événement pour le changement de sélection
            comboBoxAffichage.SelectionChanged += (s, args) => ChargerDonnees();

            // Gestionnaire d'événement pour le double-clic sur une cellule
            dataGridHistorique.MouseDoubleClick += DataGridHistorique_MouseDoubleClick;
        }

        private void ChargerDonnees()
        {
            string selectedItem = comboBoxAffichage.SelectedItem as string;

            // Si l'option "Action d'utilisateur" est sélectionnée
            if (selectedItem == "Action d'utilisateur")
            {
                // Exemple de logique pour obtenir le nom de l'utilisateur sélectionné depuis la ComboBox
                string selectedUserName = comboBoxUtilisateur.SelectedItem as string;

                // Récupérer l'ID de l'utilisateur
                int userId = StockManager.GetUserId(selectedUserName);

                // Charger les données en fonction de l'option sélectionnée
                dataGridHistorique.ItemsSource = StockManager.ChargerModificationsStock(selectedItem, userId);
            }
            else
            {
                // Charger les données en fonction de l'option sélectionnée
                dataGridHistorique.ItemsSource = StockManager.ChargerModificationsStock(selectedItem);
            }
        }

        private void DataGridHistorique_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGridHistorique.SelectedItem != null)
            {
                StockModificationItem selectedItem = (StockModificationItem)dataGridHistorique.SelectedItem;
                MessageBox.Show($"Double-clic sur l'utilisateur : {selectedItem.Utilisateur}");
            }
        }

        private void ChargerComboBoxUtilisateur()
        {
            comboBoxUtilisateur.ItemsSource = StockManager.ChargerUtilisateurs();
        }
    }
}
