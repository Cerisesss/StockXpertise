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

namespace StockXpertise
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ConfigurationDB.ActualisationDB();
            InitializeComponent();
            
            string nomemployes = "SELECT nom FROM employes"; 
            
            ConfigurationDB.ExecuteQuery(nomemployes);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gridMainWidow.Visibility = Visibility.Collapsed;
            mainWindow.Navigate(new Uri("/Connection/Connection.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}
