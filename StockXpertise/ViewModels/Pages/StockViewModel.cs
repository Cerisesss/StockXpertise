using StockXpertise.Models;
using System.Collections.ObjectModel;

namespace StockXpertise.ViewModels.Pages
{
    public partial class StockViewModel : ObservableObject
    {
        public ObservableCollection<Article> Articles { get; set; }

        public StockViewModel()
        {
            

        }
    }
}
