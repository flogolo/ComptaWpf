using System;
using System.Globalization;
using System.Windows.Data;

namespace MaCompta.Common
{
    public class EnumToBooleanConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            var result = checkValue.Equals(targetValue,
                                           StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return Binding.DoNothing;
            }

            var useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
            {
                var result = Enum.Parse(targetType, targetValue);

                return result;
            }

            return Binding.DoNothing;
        }
    }
}
