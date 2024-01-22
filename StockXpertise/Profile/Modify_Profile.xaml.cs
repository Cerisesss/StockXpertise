using StockXpertise.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Threading;

namespace StockXpertise.Profile
{
    /// <summary>
    /// Logique d'interaction pour Modify_Profile.xaml
    /// </summary>
    public partial class Modify_Profile : Page
    {
        int id = Convert.ToInt32(Application.Current.Properties["id_employes"]);

        private DispatcherTimer timer;

        public Modify_Profile()
        {
            InitializeComponent();

            ConnexionProgressBar.Visibility = Visibility.Hidden;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Incrémentez la barre de progression
            ConnexionProgressBar.Value += (200.0 / (3000 / timer.Interval.TotalMilliseconds));
        }

        private async void Button_Valider(object sender, RoutedEventArgs e)
        {
            ConnexionProgressBar.Value = 0;

            string password = mdpTextBox.Text;

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(NouveauMdp.Text) || string.IsNullOrEmpty(ConfirmationMdp.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (NouveauMdp.Text != ConfirmationMdp.Text)
                {
                    MessageBox.Show("Les mots de passe ne correspondent pas", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //barre de progression
                ConnexionProgressBar.Visibility = Visibility.Visible;

                timer.Start();

                await Task.Delay(2000);

                //si le mot de passe actuel est incorrect
                Query_Profile verif_mdp_actuel = new Query_Profile(id);

                string passwordDB = verif_mdp_actuel.GetPassword(); //recupere le hash du mdp dans la db

                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, passwordDB);

                if (!isPasswordCorrect)
                {
                    MessageBox.Show("Le mot de passe actuel est incorrect.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);

                    ConnexionProgressBar.Value = 100;
                    timer.Stop();

                    ConnexionProgressBar.Visibility = Visibility.Hidden;

                    return;
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt(15);
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(NouveauMdp.Text, salt);

                Query_Profile query_update = new Query_Profile(hashedPassword, id);
                query_update.Update_Profile();

                ConnexionProgressBar.Value = 100;
                timer.Stop();

                Profile profile = new Profile();
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = profile;
                }
            }
        }

        private void Button_Annuler(object sender, RoutedEventArgs e)
        {
            Profile profile = new Profile();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = profile;
            }
        }

    }
}
