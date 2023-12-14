using System;
using System.Collections.Generic;
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
        public Profile(int test)
        {
            InitializeComponent();

            // autre methode pour recuperer les donnees de l'utilisateur connecte : singleton, methode static
            ProfileFirstName.Content = Application.Current.Properties["nom"];
            ProfileLastName.Content = Application.Current.Properties["prenom"];
            ProfileMail.Content = Application.Current.Properties["mail"];
            ProfileStatus.Content = Application.Current.Properties["role"];
        }
    }
}
