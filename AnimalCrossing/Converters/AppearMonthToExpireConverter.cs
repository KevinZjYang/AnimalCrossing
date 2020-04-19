using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AnimalCrossing.Converters
{
    public class AppearMonthToExpireConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is List<bool> months)
            {
                var nextMonth = DateTime.Now.Month;
                if (months[nextMonth] == false)
                {
                    return Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    return Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
            else
            {
                return Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
