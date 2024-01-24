using StockXpertise.Models;
using StockXpertise.ViewModels.Pages;
using StockXpertise.Views.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace StockXpertise.Views.Pages
{
    /// <summary>
    /// Logique d'interaction pour StockPage.xaml
    /// </summary>
    public partial class StockPage : Page
    {

        public StockViewModel ViewModel { get; }

        public ObservableCollection<Article> Articles { get; set; }


        public StockPage(StockViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            Articles = new ObservableCollection<Article>();

            var articles = Article.GetAll();

            foreach (var article in articles)
            {
                Articles.Add(article);
                Console.WriteLine(article.Nom);
            }

            var currentUser = App.GetService<MainWindow>().CurrentUser;
            Console.WriteLine(currentUser.mail);

            InitializeComponent();
        }

       
    }
}
