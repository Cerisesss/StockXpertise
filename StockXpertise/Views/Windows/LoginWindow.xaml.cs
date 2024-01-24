using StockXpertise.Models;
using StockXpertise.ViewModels.Pages;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace StockXpertise.Views.Windows
{
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : INavigableView<LoginViewModel>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public LoginViewModel ViewModel { get; }

        public LoginWindow(LoginViewModel viewModel)
        {
            Wpf.Ui.Appearance.Watcher.Watch(this);

            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        public void LoginClick(object sender, RoutedEventArgs e)
        {
            var employee = Employes.GetBy("mail", Email);

            if (employee == null)
            {
                MessageBox.Show("Email ou mot de passe incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
               if (employee.mot_de_passe == Password)
                {
                    MainWindow main = App.GetService<MainWindow>();
                    main.Show();
                    main.SetCurrentUser(employee);

                    this.Close();
                } else
                {
                    MessageBox.Show("Email ou mot de passe incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }
    }
}
