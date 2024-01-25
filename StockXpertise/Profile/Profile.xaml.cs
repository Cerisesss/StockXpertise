using StockXpertise.Connection;
using StockXpertise.User;
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

namespace StockXpertise.Profile
{
    /// <summary>
    /// Logique d'interaction pour Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public Profile()
        {
            InitializeComponent();

            // autre methode pour recuperer les donnees de l'utilisateur connecte : singleton, methode static
            ProfileFirstName.Content = Application.Current.Properties["nom"];
            ProfileLastName.Content = Application.Current.Properties["prenom"];
            ProfileMail.Content = Application.Current.Properties["mail"];
            ProfileStatus.Content = Application.Current.Properties["role"];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Etes-vous sûr de vouloir vous déconnecter ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Properties["id_employes"] = null; ;
                Application.Current.Properties["nom"] = null;
                Application.Current.Properties["prenom"] = null;
                Application.Current.Properties["mot_de_passe"] = null;
                Application.Current.Properties["mail"] = null;
                Application.Current.Properties["role"] = null;
                Application.Current.Properties["mdp"] = null;

                Connection.Connection connection = new Connection.Connection();
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = connection;
                }

                gridProfile.Visibility = Visibility.Collapsed;
                profile.Navigate(new Uri("/Connection/Connection.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void Button_Modifier(object sender, RoutedEventArgs e)
        {
            //redirection vers la page User.xaml
            Modify_Profile modifyProfile = new Modify_Profile();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = modifyProfile;
            }
        }

    }
}
