using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace My.Labs.Translator.Views.Converters
{

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                          System.Globalization.CultureInfo culture)
        {
            bool Bool = bool.Parse(value.ToString());
            return Bool ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                        System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
