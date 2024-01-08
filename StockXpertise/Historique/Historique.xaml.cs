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
            comboBoxAffichage.ItemsSource = new string[] {
                "Quantité des stocks modifiée (tous)",
                "Action d'utilisateur",
                "Rentrez",
                "Sortie",
                "Transaction (caissier)"
            };
            comboBoxAffichage.SelectedIndex = 0;
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

            switch (selectedItem)
            {
                case "Quantité des stocks modifiée (tous)":
                case "Rentrez":
                case "Sortie":
                case "Transaction (caissier)":
                    dataGridHistorique.ItemsSource = StockManager.ChargerModificationsStock(selectedItem);
                    break;

                case "Action d'utilisateur":
                    if (comboBoxUtilisateur.SelectedItem != null)
                    {
                        string selectedUserName = comboBoxUtilisateur.SelectedItem as string;
                        int userId = StockManager.GetUserId(selectedUserName);
                        dataGridHistorique.ItemsSource = StockManager.ChargerModificationsStock(selectedItem, userId);
                    }
                    else
                    {
                        MessageBox.Show("Veuillez sélectionner un utilisateur.");
                    }
                    break;
            }
            Console.WriteLine($"Nombre d'éléments chargés : {dataGridHistorique.Items.Count}");
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
