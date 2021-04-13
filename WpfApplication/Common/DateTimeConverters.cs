using System;
using System.Globalization;
using System.Windows.Data;

namespace MaCompta.Common
{
    public class DateTimeToBoolConverter : IValueConverter
    {
        public object Convert(object value,
                   Type targetType,
                   object parameter,
                   CultureInfo culture)
        {
            var date = (DateTime?)value;
            if (date == null)
                return false;
            return true;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            var bValue = (bool)value;
            if (bValue)
                return DateTime.Now;
            return null;
        }

    }

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value,
                   Type targetType,
                   object parameter,
                   CultureInfo culture)
        {
            var date = (DateTime?)value;
            if (date == null)
                return String.Empty;
            return date.Value.ToShortDateString();
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            string strValue = value.ToString();
            var date = strValue.ToDateTime();
            if (date == null)
                return value;
            return date;
         }

    }
}
