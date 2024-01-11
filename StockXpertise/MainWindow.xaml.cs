using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Core;

namespace StockXpertise
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();

        }
        private async void InitializeAsync()
        {
            string htmlFilePath = "./wwwroot/index.html"; // Replace with the actual path
            string modifiedHtmlContent = File.ReadAllText(htmlFilePath);
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.WebMessageReceived += HandleWebMessageReceived;
            webView.CoreWebView2.NavigateToString(modifiedHtmlContent); // Load your HTML content here
        }
        private void HandleWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string message = args.TryGetWebMessageAsString();
            if (message == "navigateToConnection")
            {
                NavigateToConnection();
            }
        }

        private void NavigateToConnection()
        {
            // Code to navigate to the Connection page
            // Replace this with your actual navigation logic
        }
    }
}
