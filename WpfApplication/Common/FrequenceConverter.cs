using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using CommonLibrary.Models;

namespace MaCompta.Common
{
    public class FrequenceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is FrequenceEnum && values[1] is int)
            {
                var frequence = (FrequenceEnum) values[0];
                switch (frequence)
                {
                    case FrequenceEnum.Mensuel:
                        return values[1].ToString();
                    case FrequenceEnum.Annuel:
                        return CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[(int) values[1]];
                        //return "mois " + values[1];
                    case FrequenceEnum.Hebdomadaire:
                        return CultureInfo.CurrentUICulture.DateTimeFormat.DayNames[(int) values[1]];
                    //return "jour " + values[1];
                    case FrequenceEnum.NonDefini:
                        return "Non défini";
                }
            }
            return "FrequenceConverter Error";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// convertir une fréquence en Visibility selon le paramètre
    /// si param = Text et fréquence mensuel ou annuel -> visible
    /// si param = Text et fréquence hebdo  -> collapsed
    /// si param = Combo et fréquence mensuel ou annuel -> collapsed
    /// si param = Text et fréquence hebdo  -> visible
    /// </summary>
    public class FrequenceToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FrequenceEnum && parameter is String)
            {
                var frequence = (FrequenceEnum)value;
                var type = parameter as String;
                switch (frequence)
                {
                    case FrequenceEnum.Mensuel:
                        if (type.Equals("Mensuel"))
                            return Visibility.Visible;
                        break;
                    case FrequenceEnum.Annuel:
                        if (type.Equals("Annuel"))
                            return Visibility.Visible;
                        break;
                    case FrequenceEnum.Hebdomadaire:
                        if (type.Equals("Hebdo"))
                            return Visibility.Visible;
                        break;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
