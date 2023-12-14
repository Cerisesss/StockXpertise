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



    public partial class Statgénérate : Page
    {
        public bool PrixAchat { get; set; }
        public bool PrixVente { get; set; }
        public bool ArticlesVendus { get; set; }
        public bool Marge { get; set; }
        public bool Top10Produits { get; set; }
        public bool StockNegatif { get; set; }
        public bool TotalVentes { get; set; }

        public Statgénérate()
        {
            InitializeComponent();
        }

        public void GenerateTable()
        {

        }
    }

}
