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
        }

        public void LoginClick(object sender, RoutedEventArgs e)
        {
            MainWindow main = App.GetService<MainWindow>();
            main.Show();

            this.Close(); 
        }
    }
}
