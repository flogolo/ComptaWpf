using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MaCompta.Common
{
    /// <summary>
    /// Convertisseur de monnaie
    /// </summary>
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var inputDouble = value as double?;
            if (inputDouble.HasValue)
            {
                return inputDouble.Value.ToString("c2", CultureInfo.CurrentCulture);
            }

            var inputDecimal = value as decimal?;
            if (inputDecimal.HasValue)
            {
                return inputDecimal.Value.ToString("c2", CultureInfo.CurrentCulture);
            }
            return String.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value as string;
            if (input != null)
            {
                input = input.Replace(".", ",");
                if (targetType == typeof(double))
                {
                    return input.ToDouble();
                }
                if (targetType == typeof(decimal))
                {
                    return input.ToDecimal();
                }
            }
            return value;
        }
    }

    /// <file>Converters.cs</file>
    /// <summary>
    /// Convertisseur d'un decimal vers une couleur
    /// </summary>
    public class DecimalToColorConverter : IValueConverter
    {
        const string ColorPositif = "MediumSeaGreen";
        const string ColorNegatif = "Tomato"; //EA0000

        private static SolidColorBrush tomato = new SolidColorBrush(Colors.Tomato);
        private static SolidColorBrush green = new SolidColorBrush(Colors.MediumSeaGreen);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Brush))
            {
                var montant = System.Convert.ToDecimal(value, culture);
                if (montant < 0)
                    //return new SolidColorBrush(ColorTools.GetColor(ColorNegatif));
                    return tomato;
                if (montant > 0)
                    return green;
                //return new SolidColorBrush(ColorTools.GetColor(ColorPositif));

                return new SolidColorBrush(Colors.DarkGray);
            }
            throw new InvalidOperationException("Converter can only convert to value of type brush.");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("DecimalToColorConverter cannot convert back.");
        }
    }

   
}
