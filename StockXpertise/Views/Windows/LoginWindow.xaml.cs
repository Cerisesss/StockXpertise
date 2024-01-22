using Microsoft.Extensions.DependencyInjection;
using StockXpertise.Models;
using StockXpertise.ViewModels.Pages;
using StockXpertise.ViewModels.Windows;
using System.IO;
using Wpf.Ui.Controls;
using Wpf.Ui.Services;

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

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\", "config.xml");
            DBConfig config = DBConfig.LoadFromXml(path);

            Console.WriteLine(config.ToString());
        }

        public void LoginClick(object sender, RoutedEventArgs e)
        {
            //MainWindowViewModel mainWindowViewModel = new();

            //IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();
            //INavigationService navigationService = new NavigationService(serviceProvider);
            //ISnackbarService snackbarService = new SnackbarService();
            //IContentDialogService contentDialogService = new ContentDialogService();

            //MainWindow mainWindow = new (mainWindowViewModel, navigationService, serviceProvider, snackbarService, contentDialogService);
            //mainWindow.Show();

            //this.Close(); 

        }
    }
}
