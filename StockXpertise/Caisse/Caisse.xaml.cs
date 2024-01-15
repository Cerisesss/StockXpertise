using System;
using System.Collections;
using System.Collections.Generic;
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
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using StockXpertise.Connection;
using StockXpertise.Stock;

namespace StockXpertise.Caisse
{
    /// <summary>
    /// Logique d'interaction pour Caisse.xaml
    /// </summary>
    public partial class Caisse : Page
    {
        List<ImageInfo> listeImages = new List<ImageInfo>();
        List<Article> listeArticles = new List<Article>();

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
                MessageBox.Show("Veuillez remplir tous les champs");
            }
            else
            {
                // Vérifie que la quantité soit positive et qu'il y en a assez dans la bdd
                if (!IsValidQuantite(quantite))
                {
                    MessageBox.Show("Veuillez entrer une quantité valide et positive");
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
                                    MessageBox.Show($"Quantité mise à jour : x{totalQuantite}, code barre : {code_barre}");

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
                                    MessageBox.Show("La quantité totale dépasse la limite autorisée.");
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
                                MessageBox.Show($"Article ajouté à la liste de course : x{quantiteSaisieInt}, code barre : {code_barre}");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Rupture de Stock, nombre de produit en stock : " + quantiteEnStock + ".");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Code barre introuvable.");
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
            MessageBox.Show("Liste de course validée !");

            // Effacez la liste d'articles après validation
            listeArticles.Clear();
        }

        public class Article
        {
            public string CodeBarre { get; set; }
            public int Quantite { get; set; }
            public int QuantiteEnStock { get; set; }
        }
    }
}