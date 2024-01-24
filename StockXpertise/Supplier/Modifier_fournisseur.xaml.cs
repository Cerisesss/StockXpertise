using MySql.Data.MySqlClient;
using StockXpertise.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using ZXing;

namespace StockXpertise.Supplier
{
    /// <summary>
    /// Logique d'interaction pour Modifier_fournisseur.xaml
    /// </summary>
    public partial class Modifier_fournisseur : Page
    {
        int id;
        string nom;
        string prenom;
        int num;
        string mail;
        string adresse;

        public Modifier_fournisseur()
        {
            InitializeComponent();

            id = Convert.ToInt32(Application.Current.Properties["Id_Fournisseur_DataGrid"]);
            labelNom.Content = sqlconvert("nom");
            labelPrenom.Content = sqlconvert("prenom");
            labelNum.Content = sqlconvert("numero");
            labelMail.Content = sqlconvert("mail");
            labelAdresse.Content = sqlconvert("adresse");

/*
            labelPrenom.Content = Application.Current.Properties["Prenom_Founisseur_DataGrid"].ToString();
            labelNum.Content = Application.Current.Properties["Numero_Founisseur_DataGrid"].ToString();
            labelMail.Content = Application.Current.Properties["Mail_Founisseur_DataGrid"].ToString();
            labelAdresse.Content = Application.Current.Properties["Adresse_Founisseur_DataGrid"].ToString();

            id = Convert.ToInt32(Application.Current.Properties["Id_Fournisseur_DataGrid"].ToString());
            nom = Application.Current.Properties["Nom_Founisseur_DataGrid"].ToString();
            prenom = Application.Current.Properties["Prenom_Founisseur_DataGrid"].ToString();
            num = Convert.ToInt32(Application.Current.Properties["Numero_Founisseur_DataGrid"].ToString());
            mail = Application.Current.Properties["Mail_Founisseur_DataGrid"].ToString();
            adresse = Application.Current.Properties["Adresse_Founisseur_DataGrid"].ToString();*/
        }

        private string sqlconvert(string columnName)
        {
            // Utiliser des paramètres pour éviter les vulnérabilités SQL comme l'injection
            string query = "SELECT " + columnName + " FROM fournisseur WHERE id_fournisseur = "+id+";";

            using (MySqlDataReader reader= ConfigurationDB.ExecuteQuery(query))
            {
                // Vérifier si le lecteur a des lignes de résultats
                if (reader.Read())
                {
                    // Retourner la valeur de la colonne spécifiée
                    return reader[columnName].ToString();
                }
            }

            // Retourner une valeur par défaut si aucune ligne n'est trouvée
            return string.Empty;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if ((string)labelNum.Content != numTextBox.Text && !string.IsNullOrEmpty(numTextBox.Text))
            {
                if (Int32.TryParse(numTextBox.Text, out var numConverted))
                {
                    num = numConverted;
                }
                else
                {
                    // La conversion a échoué, numero ne contient pas une valeur entière valide
                    MessageBox.Show("Le numéro ne peut contenir que des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if ((string)labelNom.Content != nomTextBox.Text && !string.IsNullOrEmpty(nomTextBox.Text))
            {
                if (!Regex.IsMatch(nomTextBox.Text, "^[a-zA-Z]+$"))
                {
                    MessageBox.Show("Le nom et prénom ne peuvent contenir que des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    nom = nomTextBox.Text;
                }
            }

            if ((string)labelPrenom.Content != prenomTextBox.Text && !string.IsNullOrEmpty(prenomTextBox.Text))
            {
                if (!Regex.IsMatch(prenomTextBox.Text, "^[a-zA-Z]+$"))
                {
                    MessageBox.Show("Le nom et prénom ne peuvent contenir que des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    prenom = prenomTextBox.Text;
                }
            }

            if ((string)labelMail.Content != mailTextBox.Text && !string.IsNullOrEmpty(mailTextBox.Text))
            {
                mail = mailTextBox.Text;
            }
            if (!mail.Contains("@"))
            {
                MessageBox.Show("Votre adresse mail semble incorrecte");
                return;
            }

            if ((string)labelAdresse.Content != adresseTextBox.Text && !string.IsNullOrEmpty(adresseTextBox.Text))
            {
                adresse = adresseTextBox.Text;
            }

            //requete pour modifier les données de la ligne selectionnée
            Query_Fournisseur query_modify = new Query_Fournisseur(id, nom, prenom, num, mail, adresse);
            query_modify.Update_Supplier();

            //creer une nouvelle page 
            fournisseur fournisseur = new fournisseur();
            Window parentWindow = Window.GetWindow(this);

            //affiche le contenu de la page suivante
            if (parentWindow != null)
            {
                parentWindow.Content = fournisseur;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //creer une nouvelle page 
            fournisseur fournisseur = new fournisseur();
            Window parentWindow = Window.GetWindow(this);

            //affiche le contenu de la page suivante
            if (parentWindow != null)
            {
                parentWindow.Content = fournisseur;
            }
        }

        /*private void Button_Supprimer(object sender, RoutedEventArgs e)
        {
            Query_Fournisseur query_modify = new Query_Fournisseur(id, nom, prenom, num, mail, adresse);
            query_modify.Delete_Supplier();

            //creer une nouvelle page 
            fournisseur fournisseur = new fournisseur();
            Window parentWindow = Window.GetWindow(this);

            //affiche le contenu de la page suivante
            if (parentWindow != null)
            {
                parentWindow.Content = fournisseur;
            }
        }*/

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
