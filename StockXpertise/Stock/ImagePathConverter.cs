using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace StockXpertise.Stock
{
    public class ImagePathConverter : IValueConverter
    {
        /*public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string cheminImage && !string.IsNullOrEmpty(cheminImage))
            {
                // En supposant que les images se trouvent dans un dossier nommé "Images" dans votre projet
                string cheminRelatif = $"/StockXpertise;component/Images/{cheminImage}";
                return new BitmapImage(new Uri(cheminRelatif, UriKind.RelativeOrAbsolute));
            }

            return null;
        }*/

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string cheminImage && !string.IsNullOrEmpty(cheminImage))
            {
                string cheminRelatif = $"/StockXpertise;component/Images/{cheminImage}";

                // Ajoutez une sortie de débogage pour vérifier le chemin généré
                Console.WriteLine($"Chemin généré : {cheminRelatif}");

                return new BitmapImage(new Uri(cheminRelatif, UriKind.RelativeOrAbsolute));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

