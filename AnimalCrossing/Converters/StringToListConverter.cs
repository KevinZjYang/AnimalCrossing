using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AnimalCrossing.Converters
{
    public class StringToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.ToString() == "[]")
            {
                return null;
            }
            else
            {
                //['https://patchwiki.biligame.com/images/dongsen/f/f8/cczj5m0oeo0x3ff7qgdi4nb8y6xofb7.jpg']
                var str = value.ToString().Replace("'", "");
                str = str.Replace("]", "");
                str = str.Replace("[", "");

                return new List<string>(str.Split(","));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
