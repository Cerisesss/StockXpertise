using System.ComponentModel;

namespace StockXpertise.ViewModels.Pages
{
    public partial class LoginViewModel : ObservableObject, INotifyPropertyChanged
    {

        [ObservableProperty]
        private int _counter = 0;

        [ObservableProperty]
        private string _applicationTitle = "WPF UI - StockXpertise";

    }
}
