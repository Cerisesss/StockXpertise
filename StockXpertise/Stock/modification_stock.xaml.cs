using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace StockXpertise.Stock
{
    /// <summary>
    /// Logique d'interaction pour modification_stock.xaml
    /// </summary>
    public partial class modification_stock : Page
    {

        private Article selectedData;
        // Stock le chemin de l'image
        private string imagePath;
        // Suit l'état du bouton Supprimer_image
        private bool supprimerImageClicked = false;
        // Stock le chemin initial de l'image
        private string initialImagePath;


        public modification_stock(Article selectedData)
        {
            InitializeComponent();

            // Initialise initialImagePath avec le chemin de l'image existante dans votre contrôle Image au chargement initial
            initialImagePath = selectedData.Image;

            this.selectedData = selectedData;
            LoadData();
        }

        public void LoadData()
        {
            // Récupère les données de l'article pour les afficher dans les TextBoxs
            nom_avant.Text = selectedData.Nom;
            famille_avant.Text = selectedData.Famille;
            code_barre_avant.Text = selectedData.CodeBarre;
            code_barre_apres.Text = selectedData.CodeBarre;
            description_avant.Text = selectedData.Description;
            quantite_avant.Text = selectedData.Quantite.ToString();
            prix_HT_avant.Text = selectedData.PrixHT.ToString();
            prix_TTC_avant.Text = selectedData.PrixTTC.ToString();

            string imagePath = selectedData.Image;

            if (!string.IsNullOrEmpty(imagePath))
            {
                // Crée une instance de BitmapImage
                BitmapImage bitmap = new BitmapImage();

                // Source de l'image grâce au chemin
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();

                // Image affecté au contrôle Image
                image_avant.Source = bitmap;
                image_apres.Source = bitmap;
            }
        }

        private void enregistrer_modification(object sender, RoutedEventArgs e)
        {
            // Récupérer le texte entrer dans les TextBoxs
            string nouveauNom = nom_apres.Text;
            string nouvelleFamille = famille_apres.Text;
            string nouvelledescription = description_apres.Text;
            string nouvelleQuantite = quantite_apres.Text;
            string nouveauPrixHT = prix_HT_apres.Text;
            string nouveauPrixTTC = prix_TTC_apres.Text;

            string query;

            if (string.IsNullOrEmpty(nouveauNom) && string.IsNullOrEmpty(nouvelleFamille) && string.IsNullOrEmpty(nouvelledescription) && string.IsNullOrEmpty(nouvelleQuantite) && string.IsNullOrEmpty(nouveauPrixHT) && string.IsNullOrEmpty(nouveauPrixTTC) && string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Veuillez remplir au moins un champ");
                return;
            }
            else
            {
                // Vérifier si les TextBoxs ne sont pas vides pour enregistrer que les données qui ont été modifiées
                if (!string.IsNullOrEmpty(nouveauNom))
                {
                    query = "UPDATE articles SET nom = '" + nouveauNom + "' WHERE id_articles = " + selectedData.Id;
                    ConfigurationDB.ExecuteQuery(query);
                }
                if (!string.IsNullOrEmpty(nouvelleFamille))
                {
                    query = "UPDATE articles SET famille = '" + nouvelleFamille + "' WHERE id_articles = " + selectedData.Id;
                    ConfigurationDB.ExecuteQuery(query);
                }
                if (!string.IsNullOrEmpty(nouvelledescription))
                {
                    query = "UPDATE articles SET description = '" + nouvelledescription + "' WHERE id_articles = " + selectedData.Id;
                    ConfigurationDB.ExecuteQuery(query);
                }
                if (!string.IsNullOrEmpty(nouvelleQuantite))
                {
                    query = "UPDATE produit SET quantite_stock = '" + nouvelleQuantite + "' WHERE id_articles = " + selectedData.Id;
                    ConfigurationDB.ExecuteQuery(query);
                }
                if (!string.IsNullOrEmpty(nouveauPrixHT))
                {
                    query = "UPDATE articles SET prix_ht = '" + nouveauPrixHT + "' WHERE id_articles = " + selectedData.Id;
                    ConfigurationDB.ExecuteQuery(query);
                }
                if (!string.IsNullOrEmpty(nouveauPrixTTC))
                {
                    query = "UPDATE articles SET prix_ttc = '" + nouveauPrixTTC + "' WHERE id_articles = " + selectedData.Id;
                    ConfigurationDB.ExecuteQuery(query);
                }

                // Si l'utilisateur a supprimé l'image existante et a ajouté une nouvelle image
                if (initialImagePath != imagePath)
                {
                    // Mettre à jour le chemin de l'image dans la base de données
                    query = "UPDATE articles SET image = '" + imagePath + "' WHERE id_articles = " + selectedData.Id;
                    ConfigurationDB.ExecuteQuery(query);
                }

                // Retourner à la page Stock après avoir enregistrer les modifications
                affichage_stock stock_display = new affichage_stock();
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = stock_display;
                }
            }
        }

        private void annuler(object sender, RoutedEventArgs e)
        {
            // Retourner à la page Stock sans enregistrer les modifications
            affichage_stock stock_display = new affichage_stock();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = stock_display;
            }
        }

        private void Ajouter_image(object sender, RoutedEventArgs e)
        {
            // Ajouter une image à l'article que si l'ulisateur à supprimer l'image actuelle

            // Si l'utilisateur a appuyé sur le bouton Supprimer_image
            if (supprimerImageClicked)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                // Filtre pour les fichiers image
                openFileDialog.Filter = "Fichiers images (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedImagePath = openFileDialog.FileName;

                    // Emplacement de destination pour télécgarger l'image
                    string destinationFolderPath = @"C:\Users\pitsy\Desktop\stockxpertise\StockXpertise\Images\";

                    // Générer un nom de fichier unique pour éviter les collisions
                    string uniqueFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(selectedImagePath);

                    // Copier le fichier sélectionné vers l'emplacement de destination
                    string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, uniqueFileName);
                    File.Copy(selectedImagePath, destinationFilePath, true);

                    // Enregistrer le chemin de l'image sélectionnée
                    imagePath = destinationFilePath;

                    // Mettre à jour l'image dans le contrôle Image
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        Uri uri = new Uri(imagePath);
                        image_apres.Source = new BitmapImage(uri);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez d'abord supprimer l'ancienne image avant d'ajouter une nouvelle.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Supprimer_image(object sender, RoutedEventArgs e)
        {
            // Supprimer l'image de l'article uniquement dans la page modification_stock et non dans la base de données
            // Marquer que le bouton Supprimer_image a été cliqué
            supprimerImageClicked = true;
            // Effacer le chemin de l'image
            imagePath = "";
            // Effacer l'image dans le contrôle Image
            image_apres.Source = null;
        }
    }
}