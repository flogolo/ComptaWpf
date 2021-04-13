using System.Windows;
using System.Windows.Data;

namespace MaCompta.Common
{
    public class VisibilityToBoolConverter : IValueConverter
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
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool btn;
            try
            {
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
            catch { throw; }
        }

        /// <summary>
        /// convert value from bool to (Visibility) 
        /// </summary>
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
            catch { throw; }

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
                default:
                    break;
            }
            return rtn;
        }
    }
}