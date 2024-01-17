using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using AForge.Video;
using AForge.Video.DirectShow;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using StockXpertise.Connection;
using StockXpertise.Stock;
using ZXing;

namespace StockXpertise.Caisse
{
    /// <summary>
    /// Logique d'interaction pour Caisse.xaml
    /// </summary>
    public partial class Caisse : Page
    {
        List<ImageInfo> listeImages = new List<ImageInfo>();
        List<Article> listeArticles = new List<Article>();
        private FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice videoCaptureDevice;

        public Caisse()
        {
            InitializeComponent();
        }

        private void Button_Click_ajouter_article(object sender, RoutedEventArgs e)
        {
            string code_barre = text_code_barre.Text;
            string quantite = text_quantite.Text;

            //si les champs sont vides, on affiche un message d'erreur
            if (string.IsNullOrEmpty(code_barre) || string.IsNullOrEmpty(quantite))
            {
                MessageBox.Show("Veuillez remplir tous les champs", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Vérifie que la quantité soit positive et qu'il y en a assez dans la bdd
                if (!IsValidQuantite(quantite))
                {
                    MessageBox.Show("Veuillez entrer une quantité valide et positive", "Oups", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Query_Caisse query_select = new Query_Caisse(code_barre);
                    MySqlDataReader result = query_select.Compare_Quantite();

                    // Vérifie s'il y a des lignes de résultat
                    if (result.Read())
                    {
                        int quantiteEnStock = result.GetInt32(0);

                        if (int.TryParse(quantite, out int quantiteSaisieInt) && quantiteSaisieInt <= quantiteEnStock)
                        {
                            // Liste cumulative des images
                            List<ImageInfo> newListeImages = new List<ImageInfo>();

                            // Recherche de l'article dans la liste
                            Article existingArticle = listeArticles.FirstOrDefault(a => a.CodeBarre == code_barre);

                            if (existingArticle != null)
                            {
                                // Si l'article existe déjà, ajoutez simplement la quantité, mais vérifiez la limite
                                int totalQuantite = existingArticle.Quantite + quantiteSaisieInt;

                                if (totalQuantite <= existingArticle.QuantiteEnStock)
                                {
                                    existingArticle.Quantite = totalQuantite;
                                    MessageBox.Show($"Quantité mise à jour : x{totalQuantite}, code barre : {code_barre}", "Réussi", MessageBoxButton.OK, MessageBoxImage.Information);

                                    foreach (var existing_Article in listeArticles)
                                    {
                                        string existingQuery = "SELECT nom, image FROM articles WHERE code_barre = '" + existing_Article.CodeBarre + "'";
                                        MySqlDataReader existingReader = ConfigurationDB.ExecuteQuery(existingQuery);

                                        while (existingReader.Read())
                                        {
                                            var existingImageArticle = new ImageInfo()
                                            {
                                                nom_article = existingReader["nom"].ToString(),
                                                image_path = existingReader["image"].ToString().Replace(newChar: '/', oldChar: '\\'),
                                                quantite = existing_Article.Quantite // Utilise la quantité spécifique de l'article existant
                                            };

                                            newListeImages.Add(existingImageArticle);
                                        }
                                    }
                                    // Assigne la liste cumulative à la source de données de la ListBox
                                    listBoxImages.ItemsSource = newListeImages;
                                }
                                else
                                {
                                    MessageBox.Show("La quantité totale dépasse la limite autorisée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                // Ajout de l'article à la liste d'articles
                                var article = new Article()
                                {
                                    CodeBarre = code_barre,
                                    Quantite = quantiteSaisieInt,
                                    QuantiteEnStock = quantiteEnStock
                                };
                                listeArticles.Add(article);

                                foreach (var existing_Article in listeArticles)
                                {
                                    // Ajout de l'article nouvellement ajouté dans la liste d'images
                                    string newQuery = "SELECT nom, image FROM articles WHERE code_barre = '" + existing_Article.CodeBarre + "'";
                                    MySqlDataReader newReader = ConfigurationDB.ExecuteQuery(newQuery);

                                    while (newReader.Read())
                                    {
                                        var newImageArticle = new ImageInfo()
                                        {
                                            nom_article = newReader["nom"].ToString(),
                                            image_path = newReader["image"].ToString().Replace(newChar: '/', oldChar: '\\'),
                                            quantite = existing_Article.Quantite
                                        };
                                        newListeImages.Add(newImageArticle);
                                    }
                                }

                                // Assigne la liste cumulative à la source de données de la ListBox
                                listBoxImages.ItemsSource = newListeImages;

                                // Affiche un message de confirmation
                                MessageBox.Show($"Article ajouté à la liste de course : x{quantiteSaisieInt}, code barre : {code_barre}", "Réussi", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Rupture de Stock, nombre de produit en stock : " + quantiteEnStock + ".", "Oups", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Code barre introuvable.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool IsValidQuantite(string quantite)
        {
            //Vérifie que la quantité est un nombre positif et qu'il n'y a ni symboles ni lettres
            const string pattern = @"^[1-9]\d*$";
            return Regex.IsMatch(quantite, pattern);
        }

        private void Button_Click_Valider(object sender, RoutedEventArgs e)
        {
            //une fois que l'on appuie sur le bouton valider, on supprime tous les articles de la liste de course de la bdd et on affiche un message de confirmation
            // Parcour la liste d'articles et mettez à jour la base de données
            foreach (var article in listeArticles)
            {
                // Met à jour la base de données avec article.CodeBarre et article.Quantite
                Query_Caisse query_update = new Query_Caisse(article.CodeBarre);
                query_update.Maj_Quantite_Stock(article.Quantite);
            }

            // Affichez le message de confirmation
            MessageBox.Show("Liste de course validée !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

            // Effacez la liste d'articles après validation
            listeArticles.Clear();
        }

        public class Article
        {
            public string CodeBarre { get; set; }
            public int Quantite { get; set; }
            public int QuantiteEnStock { get; set; }
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
                        text_code_barre.Dispatcher.Invoke(() =>
                        {
                            // Afficher le code barre dans le textbox
                            text_code_barre.Text = result.ToString();
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