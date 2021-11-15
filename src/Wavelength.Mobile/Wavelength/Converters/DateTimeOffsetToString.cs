using System;
using System.Globalization;
using Xamarin.Forms;

namespace Wavelength.Converters
{
    public class DateTimeOffsetToString
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.ToString("hh:mm:ss.fffffff");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        } 
    }
}