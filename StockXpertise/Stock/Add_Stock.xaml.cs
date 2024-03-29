﻿using AForge.Video;
using AForge.Video.DirectShow;
using MySql.Data.MySqlClient;
using StockXpertise.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;
using ZXing.Common;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using MessageBox = System.Windows.MessageBox;

namespace StockXpertise.Stock
{
    /// <summary>
    /// Logique d'interaction pour Add_Stock.xaml
    /// </summary>
    public partial class Add_Stock : Page
    {
        List<Fournisseur> fournisseurs = new List<Fournisseur>();

        private string selectedImagePath;

        string nom;
        string famille;
        string prixHT_string;
        string prixTTC_string;
        string prixAchat_string;
        string description;
        string code_barre;
        string quantite_string;
        string emplacement;
        string nomFournisseur;
        string prenomFournisseur;
        string randomFileName;

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        DateTime lastMemoryReleaseTime = DateTime.MinValue;

        public Add_Stock()
        {
            InitializeComponent();

            string query = "SELECT * FROM fournisseur";
            MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

            // Rempli la liste de fournisseur
            while (reader.Read())
            {
                Fournisseur fournisseur = new Fournisseur(reader["nom"].ToString(), reader["prenom"].ToString());
                fournisseurs.Add(fournisseur);
            }

            FournisseurListBox.ItemsSource = fournisseurs;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //recuperation des données des champs
            nom = nomStock.Text;
            famille = familleStock.Text;
            prixHT_string = prixHtStock.Text;
            prixTTC_string = prixTtcStock.Text;
            prixAchat_string = prixAchatStock.Text;
            description = descriptionStock.Text;
            code_barre = CodeBarreStock.Text;
            quantite_string = quantiteStock.Text;
            emplacement = emplacementStock.Text;

            //condition pour verifier si les champs sont vides
            //si c'est le cas alors on affiche un message
            //sinon on execute la requete
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(famille) || string.IsNullOrEmpty(code_barre) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(quantite_string) || string.IsNullOrEmpty(emplacement) || FournisseurListBox.SelectedIndex < 0 || string.IsNullOrEmpty(prixHT_string) || string.IsNullOrEmpty(prixTTC_string) || string.IsNullOrEmpty(prixAchat_string) || string.IsNullOrEmpty(selectedImagePath))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool result_prixHT = Int32.TryParse(prixHT_string, out var prixHT);
                bool result_prixTTC = Int32.TryParse(prixTTC_string, out var prixTTC);
                bool result_prixAchat = Int32.TryParse(prixAchat_string, out var prixAchat);
                bool result_quantite = Int32.TryParse(quantite_string, out var quantite);

                Query_Stock verif = new Query_Stock(code_barre);
                bool verif_code_barre = verif.Verif_Code_Barre();

                if (!Regex.IsMatch(famille, "^[a-zA-Z]+$"))
                {
                    MessageBox.Show("La famille doit être en lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(code_barre) && verif_code_barre)
                {
                    MessageBox.Show("Le code barre existe déjà.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!result_prixHT || !result_prixTTC || !result_prixAchat || !result_quantite)
                {
                    MessageBox.Show("Le prix HT, TTC, achat et la quantité doivent être numérique !", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(selectedImagePath))
                {
                    MessageBox.Show("Veuillez ajouter une image.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    // Pour copier le fichier sélectionné vers l'emplacement de destination

                    // Emplacement de destination pour télécharger l'image
                    string destinationFolderPath = @"C:\Users\pitsy\Desktop\stockxpertise\StockXpertise\";

                    // Nom du fichier à partir du chemin complet
                    string fileName = System.IO.Path.GetFileName(selectedImagePath);

                    string destinationFilePath;

                    // Générer le nom du fichier aléatoirement et vérifie s'il existe déjà
                    do
                    {
                        randomFileName = "Images\\" + Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fileName);
                        destinationFilePath = System.IO.Path.Combine(destinationFolderPath, randomFileName);
                    } while (File.Exists(destinationFilePath));

                    // Copie le fichier vers la destination
                    File.Copy(selectedImagePath, destinationFilePath, true);

                    // Maj du chemin de l'image dans la base de données avec le nom de fichier aléatoire
                    randomFileName = "\\" + randomFileName;
                    //query_Update.Update_ImagePath(randomFileName);
                }

                // requete pour ajouter un article
                Query_Stock query_insert = new Query_Stock(nom, famille, prixHT, prixTTC, prixAchat, description, code_barre, quantite, emplacement, nomFournisseur, prenomFournisseur, randomFileName);
                query_insert.Insert_Stock();

                //redirection vers la page affichage_stock.xaml
                affichage_stock stock = new affichage_stock();
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = stock;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //redirection vers la page affichage_stock.xaml
            affichage_stock stock = new affichage_stock();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = stock;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedFournisseur = (dynamic)FournisseurListBox.SelectedItem;

            // Vérifier si un élément est sélectionné
            if (selectedFournisseur != null)
            {
                // Convertir l'élément en chaîne (si nécessaire)
                nomFournisseur = selectedFournisseur.getNom();
                prenomFournisseur = selectedFournisseur.getPrenom();
            }
        }

        private void btnScanner_Click(object sender, EventArgs e)
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
                        CodeBarreStock.Dispatcher.Invoke(() =>
                        {
                            // Afficher le code barre dans le textbox
                            CodeBarreStock.Text = result.ToString();
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

        private void ajouterImage(object sender, RoutedEventArgs e)
        {
            // Ajouter une image à l'article
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Filtre pour les fichiers image
            openFileDialog.Filter = "Fichiers images (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";

            // Utiliser la propriété DialogResult de l'objet OpenFileDialog
            bool? result = openFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                // Maj du chemin du fichier sélectionné
                selectedImagePath = openFileDialog.FileName;

                // Maj de l'image dans le contrôle Image
                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    Uri uri = new Uri(selectedImagePath);
                    Image.Source = new BitmapImage(uri);
                }
            }
        }

    }
}
