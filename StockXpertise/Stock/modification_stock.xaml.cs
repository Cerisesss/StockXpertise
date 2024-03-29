﻿using Microsoft.Win32;
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
using AForge.Video.DirectShow;
using AForge.Video;
using System.Drawing;
using ZXing;
using System.Text.RegularExpressions;
using System.Globalization;
using StockXpertise.User;

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
        private string selectedImagePath;
        private FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice videoCaptureDevice;

        public modification_stock(Article selectedData)
        {
            InitializeComponent();

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
            description_avant.Text = selectedData.Description;
            quantite_avant.Text = selectedData.Quantite.ToString();
            prix_HT_avant.Text = selectedData.PrixHT.ToString();
            prix_TTC_avant.Text = selectedData.PrixTTC.ToString();
            code_emplacement_avant.Text = selectedData.Code_emplacement.ToString();
            image_avant.Source = selectedData.Image;
            image_apres.Source = selectedData.Image;
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
            string code_barre = code_barre_apres.Text;
            string code_emplacement = code_emplacement_apres.Text;
            int quantite_avant = selectedData.Quantite;

            if (string.IsNullOrEmpty(code_barre) && string.IsNullOrEmpty(nouveauNom) && string.IsNullOrEmpty(nouvelleFamille) && string.IsNullOrEmpty(nouvelledescription) && string.IsNullOrEmpty(nouvelleQuantite) && string.IsNullOrEmpty(nouveauPrixHT) && string.IsNullOrEmpty(nouveauPrixTTC) && string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Veuillez remplir au moins un champ", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(nouvelleFamille) && !Regex.IsMatch(nouvelleFamille, "^[a-zA-Z]+$"))
                {
                    MessageBox.Show("Famille ne contient que des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool convertPrixHT = Int32.TryParse(nouveauPrixHT, out var prixHT);
                bool convertPrixTTC = Int32.TryParse(nouveauPrixTTC, out var prixTTC);
                bool convertQuantite = Int32.TryParse(nouvelleQuantite, out var quantite);

                Query_Stock verif = new Query_Stock(code_barre);
                bool verif_code_barre = verif.Verif_Code_Barre();

                if (!string.IsNullOrEmpty(nouveauPrixHT) && !convertPrixHT)
                {
                    MessageBox.Show("Le prix HT doit etre numérique.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!string.IsNullOrEmpty(nouveauPrixTTC) && !convertPrixTTC)
                {
                    MessageBox.Show("Le prix TTC doit etre numerique.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!string.IsNullOrEmpty(nouvelleQuantite) && !convertQuantite)
                {
                    MessageBox.Show("La quantité doit être numérique.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!string.IsNullOrEmpty(code_barre) && verif_code_barre)
                {
                    MessageBox.Show("Le code barre existe déjà.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Query_Stock query_Update = new Query_Stock(selectedData.Id, nouveauNom, nouvelleFamille, prixHT, prixTTC, nouvelledescription, code_barre, quantite, code_emplacement, imagePath);

                // Si les TextBoxs ne sont pas vides, n'enregistrer que les données qui ont été modifiées
                if (!string.IsNullOrEmpty(code_emplacement))
                {
                    Query_Stock query_Update_emplacement = new Query_Stock(selectedData.Id, quantite_avant, code_emplacement);
                    query_Update_emplacement.Update_Emplacement();
                }
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
                if (!string.IsNullOrEmpty(code_barre))
                {
                    query_Update.Update_CodeBarre();
                }

                // Si l'utilisateur a supprimé l'image existante et a ajouté une nouvelle image
                if (initialImagePath != imagePath)
                {
                    // Mettre à jour le chemin de l'image dans la base de données
                    query_Update.Update_ImagePath();
                }

                // Copier le fichier sélectionné vers l'emplacement de destination
                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    // Emplacement de destination pour télécharger l'image
                    string destinationFolderPath = @"C:\Users\pitsy\Desktop\stockxpertise\StockXpertise\";

                    // Obtenez le nom du fichier à partir du chemin complet
                    string fileName = System.IO.Path.GetFileName(selectedImagePath);

                    string randomFileName;
                    string destinationFilePath;

                    // Générer un nom de fichier aléatoire et vérifier s'il existe déjà
                    do
                    {
                        randomFileName = "Images\\" + Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fileName);
                        destinationFilePath = System.IO.Path.Combine(destinationFolderPath, randomFileName);
                    } while (File.Exists(destinationFilePath));

                    // Copier le fichier vers la destination
                    File.Copy(selectedImagePath, destinationFilePath, true);

                    // Mettre à jour le chemin de l'image dans la base de données avec le nom de fichier aléatoire
                    randomFileName = "\\" + randomFileName;
                    query_Update.Update_ImagePath(randomFileName);
                }

                MessageBox.Show("Modification effectuée avec succès", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

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
            // Ajouter une image à l'article que si l'utilisateur a supprimé l'image actuelle

            // Si l'utilisateur a appuyé sur le bouton Supprimer_image
            if (supprimerImageClicked)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                // Filtre pour les fichiers image
                openFileDialog.Filter = "Fichiers images (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";

                if (openFileDialog.ShowDialog() == true)
                {
                    // Mettre à jour le chemin du fichier sélectionné
                    selectedImagePath = openFileDialog.FileName;

                    // Mettre à jour l'image dans le contrôle Image
                    if (!string.IsNullOrEmpty(selectedImagePath))
                    {
                        Uri uri = new Uri(selectedImagePath);
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
            //BitmapImage image = selectedData.Image;

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

        private void btnScanner_Click(object sender, RoutedEventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                using (Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    BarcodeReader reader = new BarcodeReader();
                    reader.AutoRotate = true;
                    reader.Options.TryInverted = true;
                    reader.Options.TryHarder = true;

                    reader.Options = new ZXing.Common.DecodingOptions
                    {
                        PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.All_1D }
                    };

                    //lit le code barre et le stock dans result
                    var result = reader.Decode(bitmap);

                    //si result est different de null alors on affiche le code barre dans le textbox
                    if (result != null)
                    {
                        //Dispatcher.Invoke permet d'executer du code dans le thread de l'interface utilisateur (methode object de wpf provenant de dispatcher)
                        code_barre_apres.Dispatcher.Invoke(() =>
                        {
                            // Afficher le code barre dans le textbox
                            code_barre_apres.Text = result.ToString();
                        });

                        if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
                        {
                            // Stopp la caméra
                            videoCaptureDevice.Stop();
                            videoCaptureDevice = null;
                        }
                    }

                    videoScan.Dispatcher.Invoke(() =>
                    {
                        // Libérer l'image précédente
                        if (videoScan.Source is BitmapSource previousBitmapSource)
                        {
                            //freez l'image
                            previousBitmapSource.Freeze();

                            // Libérer la mémoire
                            videoScan.Source = null;
                        }

                        // Convertit le Bitmap actuel en BitmapSource et l'affiche dans l'interface (image)
                        videoScan.Source = ConvertBitmapToBitmapSource(bitmap);
                    });

                    // Force une collecte des objets non référencés, permettant de libérer de la mémoire
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
        {
            var memoryStream = new MemoryStream();

            // Enregistre le Bitmap dans le MemoryStream au format BMP
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

            // Initialise le BitmapImage avec le MemoryStream contenant l'image BMP
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}