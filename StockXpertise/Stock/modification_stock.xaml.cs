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
using iText.Barcodes.Dmcode;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

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

            if (Application.Current.Properties["role"].ToString() == "Admin" || Application.Current.Properties["role"].ToString() == "admin")
            {
                button_supprimer.Visibility = Visibility.Visible;
            }
            else
            {
                button_supprimer.Visibility = Visibility.Hidden;
            }
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
                Int32.TryParse(nouveauPrixHT, out var prixHT);
                Int32.TryParse(nouveauPrixTTC, out var prixTTC);
                Int32.TryParse(nouvelleQuantite, out var quantite);

                Query_Stock query_Update = new Query_Stock(selectedData.Id, nouveauNom, nouvelleFamille, prixHT, prixTTC, nouvelledescription, quantite);

                // Vérifier si les TextBoxs ne sont pas vides pour enregistrer que les données qui ont été modifiées
                if (!string.IsNullOrEmpty(nouveauNom))
                {
                    query_Update.Update_Name();
                }
                if (!string.IsNullOrEmpty(nouvelleFamille))
                {
                    query_Update.Update_Famille();
                }
                if (!string.IsNullOrEmpty(nouvelledescription))
                {
                    query_Update.Update_Description();
                }
                if (!string.IsNullOrEmpty(nouvelleQuantite))
                {
                    query_Update.Update_Quantite();
                }
                if (!string.IsNullOrEmpty(nouveauPrixHT))
                {
                    query_Update.Update_PrixHT();
                }
                if (!string.IsNullOrEmpty(nouveauPrixTTC))
                {
                    query_Update.Update_PrixTTC();
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

        private void deleteArticle(object sender, RoutedEventArgs e)
        {
            string nom = selectedData.Nom;
            string famille = selectedData.Famille;
            string code_barre = selectedData.CodeBarre;
            string description = selectedData.Description;
            string quantite_string = selectedData.Quantite.ToString();
            string prix_HT_string = selectedData.PrixHT.ToString();
            string prix_TTC_string = selectedData.PrixTTC.ToString();

            int quantite = Convert.ToInt32(quantite_string);
            int prix_HT = Convert.ToInt32(prix_HT_string);
            int prix_TTC = Convert.ToInt32(prix_TTC_string);

            // requete pour ajouter un article
            Query_Stock query_insert = new Query_Stock(nom, famille, prix_HT, prix_TTC, description, code_barre, quantite);
            query_insert.Delete_Stock();

            //redirection vers la page affichage_stock.xaml
            affichage_stock stock = new affichage_stock();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = stock;
            }
        }
    }
}