using System;
using System.Collections.Generic;
using System.Data;
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

namespace StockXpertise.User
{
    /// <summary>
    /// Logique d'interaction pour Modify_User.xaml
    /// </summary>
    public partial class Modify_User : Page
    {
        string id;
        string nom;
        string prenom;
        string password;
        string mail;
        string role;

        public Modify_User(object selectedData)
        {
            InitializeComponent();

            labelNom.Content = Application.Current.Properties["nom"].ToString();
            labelPrenom.Content = Application.Current.Properties["prenom"].ToString();
            labelMdp.Content = Application.Current.Properties["mot_de_passe"].ToString();
            labelMail.Content = Application.Current.Properties["mail"].ToString();
            labelRole.Content = Application.Current.Properties["role"].ToString();

            //labelNom.Content = ;
            //labelPrenom.Content = ;
            //labelMdp.Content = ;
            //labelMail.Content = ;
            //labelRole.Content = selectedData.;

            /*if (selectedData is VotreClasse) // Remplacez VotreClasse par le vrai type de selectedData
            {
                var data = selectedData as VotreClasse; // Conversion de selectedData au type réel

                // Utilisation des propriétés de data pour définir le contenu des labels
                labelNom.Content = data.Nom;
                labelPrenom.Content = data.Prenom;
                labelMdp.Content = data.MotDePasse;
                labelMail.Content = data.Mail;
                labelRole.Content = data.Role;
            }*/

            id = Application.Current.Properties["id_employes"].ToString();
            nom = Application.Current.Properties["nom"].ToString();
            prenom = Application.Current.Properties["prenom"].ToString();
            password = Application.Current.Properties["mot_de_passe"].ToString();
            mail = Application.Current.Properties["mail"].ToString();
            role = Application.Current.Properties["role"].ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)labelNom.Content != nom)
            {
                nom = nomTextBox.Text;
            }

            if ((string)labelPrenom.Content != prenom)
            {
                prenom = prenomTextBox.Text;
            }

            if ((string)labelMdp.Content != password)
            {
                password = mdpTextBox.Text;
            }

            if ((string)labelMail.Content != mail)
            {
                mail = mailTextBox.Text;
            }

            if ((string)labelRole.Content != role)
            {
                if (radioAdmin.IsChecked == true)
                {
                    role = radioAdmin.Content.ToString();
                }
                else if (radioPersonnel.IsChecked == true)
                {
                    role = radioPersonnel.Content.ToString();
                }
                else if (radioCaissier.IsChecked == true)
                {
                    role = radioCaissier.Content.ToString();
                }
            }

            int id_employee = Convert.ToInt32(id);

            string query = "UPDATE employes SET nom = '" + nom + "', prenom = '" + prenom + "', mot_de_passe = '" + password + "', mail = '" + mail + "', role = '" + role + "' WHERE id_employes = " + id + ";";

            ConfigurationDB.ExecuteQuery(query);

            MessageBox.Show("Modifié avec succès.");

            User user = new User();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = user;
            }

            gridModifyUser.Visibility = Visibility.Collapsed;
            modifyUser.Navigate(new Uri("/User/User.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            User user = new User();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = user;
            }

            gridModifyUser.Visibility = Visibility.Collapsed;
            modifyUser.Navigate(new Uri("/User/User.xaml", UriKind.RelativeOrAbsolute));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Supprimer(object sender, RoutedEventArgs e)
        {

        }
    }
}
