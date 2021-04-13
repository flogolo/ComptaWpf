using System;
using System.Windows;
using System.Windows.Data;

namespace MaCompta.Common
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        private Visibility _falseToVisibility = Visibility.Collapsed;
        public Visibility FalseToVisibility
        {
            get { return _falseToVisibility; }
            set { _falseToVisibility = value; }
        }

        /// <summary>
        /// convert value from (Visibility) to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {

                bool btn;
                if ((Visibility)value == FalseToVisibility)
                {
                    btn = false;
                }
                else
                {
                    btn = true;
                }

                return btn;
            }
            catch (Exception e)
            { System.Diagnostics.Debug.WriteLine("BoolToVisibilityConverter Convertback error " + e.Message); }

            return null;
        }

        /// <summary>
        /// convert value from bool to (Visibility) 
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                Visibility vsi;
                if ((bool)value)
                {
                    vsi = ReverseVisibility(FalseToVisibility);
                }
                else
                {
                    vsi = FalseToVisibility;
                }

                return vsi;
            }
            catch (Exception e)
            { System.Diagnostics.Debug.WriteLine("BoolToVisibilityConverter Convert error " + e.Message); }


            return null;
        }

        private static Visibility ReverseVisibility(Visibility vsi)
        {
            Visibility rtn = Visibility.Visible;
            switch (vsi)
            {
                case Visibility.Collapsed:
                    rtn = Visibility.Visible;
                    break;
                case Visibility.Visible:
                    rtn = Visibility.Collapsed;
                    break;
            }
            return rtn;
        }
    }
}
