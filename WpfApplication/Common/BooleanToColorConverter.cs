using MaCompta.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MaCompta.Common
{
    public static class ColorTools
    {
        public static Color GetColor(String colorStr)
        {
            return new Color
            {
                A = 0xFF,
                R = Convert.ToByte(colorStr.Substring(0, 2), 16),
                G = Convert.ToByte(colorStr.Substring(2, 2), 16),
                B = Convert.ToByte(colorStr.Substring(4, 2), 16)
            };

        }

    }

    ///// <summary>
    ///// Convertisseur de booléen vers une couleur
    ///// </summary>
    //public class OperationToColorConverter : IValueConverter
    //{
    //    private const string ColorModified = "FFD9CC";
    //    //"FF6347";

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if( value is OperationViewModel && targetType == typeof(Brush))
    //        {
    //            var dc = value as OperationViewModel;
    //            if( dc.is)
    //        }
    //        if (targetType == typeof(Brush))
    //        {

    //            var c = ColorTools.GetColor(ColorModified);
    //            Brush brush = new SolidColorBrush(c);
    //            var isModified = System.Convert.ToBoolean(value, culture);
    //            if (isModified)
    //                return brush;
    //            //pas de changement de couleur : couleur par défaut en paramètre
    //            return parameter;
    //        }
    //        throw new InvalidOperationException("Converter can only convert to value of type brush.");

    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new InvalidOperationException("BooleanToColorConverter cannot convert back.");
    //    }
    //}

    /// <summary>
    /// Convertisseur de booléen vers une couleur
    /// </summary>
    public class BooleanToModifiedColorConverter : IValueConverter
    {
        private const string ColorModified = "FFD9CC";
        //"FF6347";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Brush))
            {

                var c = ColorTools.GetColor(ColorModified);
                Brush brush = new SolidColorBrush(c);
                var isModified = System.Convert.ToBoolean(value, culture);
                if (isModified)
                    return brush;
                //pas de changement de couleur : couleur par défaut en paramètre
                return parameter;
            }
            throw new InvalidOperationException("Converter can only convert to value of type brush.");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("BooleanToColorConverter cannot convert back.");
        }
    }

    /// <file>Converters.cs</file>
    /// <summary>
    /// Convertisseur de booléen vers une couleur
    /// </summary>
    public class BooleanToColorConverter : IValueConverter
    {
        //private const string ColorModified = "FFD9CC";
        //"FF6347";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Brush))
            {
                Brush brush = new SolidColorBrush(Colors.Black);
                var isColored = System.Convert.ToBoolean(value, culture);
                if (isColored)
                    return parameter;
                //pas de changement de couleur : couleur par défaut (black)
                return brush;
                //return null;
            }
            throw new InvalidOperationException("Converter can only convert to value of type brush.");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("BooleanToColorConverter cannot convert back.");
        }
    }
    /// <file>Converters.cs</file>
    /// <summary>
    /// Convertisseur de booléen vers une couleur
    /// </summary>
    public class BooleanToBackConverter : IValueConverter
    {
        //private const string ColorModified = "FFD9CC";
        //"FF6347";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Brush))
            {
                Brush brush = new SolidColorBrush(Colors.Black);
                var isColored = System.Convert.ToBoolean(value, culture);
                if (isColored)
                    return parameter;
                //pas de changement de couleur : couleur par défaut (black)
                //return brush;
                return null;
            }
            throw new InvalidOperationException("Converter can only convert to value of type brush.");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("BooleanToColorConverter cannot convert back.");
        }
    }
}
